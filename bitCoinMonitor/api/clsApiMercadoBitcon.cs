using System;
using System.Net;
using System.IO;
using bitCoinMonitor.tools;
using System.Collections.Generic;
using System.Text;
using bitCoinMonitor.api.objetos_tapi;

namespace bitCoinMonitor.api
{

    class clsApiMercadoBitcon:clsApiBase
    {

        protected override string chamarMetodoAPI_Dados(enumTipoMoeda aEnumMoeda, enumTipoMetodo aEnumMetodo)
        {
            string vStrRetorno = String.Empty;
            string vStrURL = "https://www.mercadobitcoin.net/api/" + aEnumMoeda.ToString() + "/" + aEnumMetodo.ToString() + "/";


            try
            {
                var vVarRequisicao = WebRequest.CreateHttp(vStrURL);
                vVarRequisicao.Method = "GET";
                vVarRequisicao.UserAgent = "ProjetoBitCoin";

                using (var vVarResposta = vVarRequisicao.GetResponse())
                {
                    var vVarStreamDados = vVarResposta.GetResponseStream();
                    StreamReader vObjReader = new StreamReader(vVarStreamDados);
                    object vObjResponse = vObjReader.ReadToEnd();

                    vStrRetorno = vObjResponse.ToString();

                    vVarStreamDados.Close();
                    vVarResposta.Close();
                }

                
            }
            catch
            {
                vStrRetorno = String.Empty;
            }
            return vStrRetorno;
        }

        protected override string chamarMetodoAPI_Negociacao(List<clsTooParametros> aObjParametros)
        {
            string vStrRetorno = String.Empty;
            string vStrPath = "/tapi/v3/";
            string vStrNonce = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
            string vStrURL = "https://www.mercadobitcoin.net" + vStrPath;
            string vStrParametros = String.Empty;
            string vStrTAPIMAC = String.Empty;
            byte[] vBtyParametros;

            Stream vObjStreamDados;
            WebRequest vObjRequest;
            WebResponse vObjResponse;
            StreamReader vObjReader;

            try
            {
                //--Incluindo o TapiNonce
                aObjParametros.Add(new clsTooParametros("tapi_nonce", vStrNonce));

                //--Transformando os parâmetros em URL-Encode;
                foreach (clsTooParametros vObjParametros in aObjParametros)
                {
                    if (vStrParametros != String.Empty) vStrParametros += "&";
                    vStrParametros += vObjParametros.pNome + "=" + vObjParametros.pValor;
                }

                //--Gerando o TAPI-MAC 
                vStrTAPIMAC = vStrPath + "?" + vStrParametros;
                //--Gerando o TAPI-MAC (Criptografado)
                vStrTAPIMAC = clsTooCriptografia.criptografarHMACSHA512(vStrTAPIMAC, Program.Parametros.pIdtSegredoTAPI);

                //--Transformando os parâmetros em bytes:
                vBtyParametros = Encoding.UTF8.GetBytes(vStrParametros);

                //--Criando a requisição 
                vObjRequest = WebRequest.CreateHttp(vStrURL);
                vObjRequest.Method = "POST";
                vObjRequest.ContentType = "application/x-www-form-urlencoded";
                vObjRequest.Headers.Add("TAPI-ID", Program.Parametros.pIdtTAPI);
                vObjRequest.Headers.Add("TAPI-MAC", vStrTAPIMAC);
                vObjRequest.ContentLength = vBtyParametros.Length;

                //--Enviando os dados
                vObjStreamDados = vObjRequest.GetRequestStream();
                vObjStreamDados.Write(vBtyParametros, 0, vBtyParametros.Length);
                vObjStreamDados.Close();

                //--Pegando o retorno
                vObjResponse = vObjRequest.GetResponse();
                vObjStreamDados = vObjResponse.GetResponseStream();
                vObjReader = new StreamReader(vObjStreamDados);
                vStrRetorno = vObjReader.ReadToEnd();

                vObjReader.Close();
                vObjStreamDados.Close();
                vObjResponse.Close();

                vStrRetorno = WebUtility.UrlDecode(vStrRetorno);
            }
            catch
            {
                throw;
            }

            return vStrRetorno;
        }


        protected override void tratarErro(int aIntErroCode, string aStrMsgErro)
        {
            try
            {
                switch (aIntErroCode)
                {
                    case 100: //--Sucesso
                        break;
                    case 199: //-- Erro: "API de Negociações desativada temporariamente."
                        System.Threading.Thread.Sleep(600000);
                        throw new Exception(aStrMsgErro);
                    case 203: //--Erro: "Valor do *tapi_nonce* inválido."
                        throw new Exception(aStrMsgErro);
                    case 212: //--Erro: "Não foi possível cancelar a ordem. Já foi processada ou cancelada."
                        throw new Exception(aStrMsgErro);
                    case 429: //--Erro: "Taxa de requisições excedeu o limite de requisições no intervalo."
                        System.Threading.Thread.Sleep(30000);
                        throw new Exception(aStrMsgErro);
                    case 430: //--Erro: "Requisição negada: taxa de requisições elevada ou requisição inválida."
                        System.Threading.Thread.Sleep(30000);
                        throw new Exception(aStrMsgErro);
                    case 431: //--Erro: "Requisições bloqueadas temporariamente."
                        System.Threading.Thread.Sleep(300000);
                        throw new Exception(aStrMsgErro);
                    default:
                        throw new Exception(aStrMsgErro);
                }
            }
            catch { throw; }

        }
        
        public override clsApiOrderbook buscarOrderBook(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas,bool aBooCompleto = false)
        {
            string vStrRetornoAPI = string.Empty;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiOrderbook vObjOrderBook = new clsApiOrderbook();

            try
            {

                vObjParametros.Add(new clsTooParametros("tapi_method", enumTipoTapiMetodo.list_orderbook.ToString()));
                vObjParametros.Add(new clsTooParametros("coin_pair", aEnumParMoedas.ToString()));
                vObjParametros.Add(new clsTooParametros("full", aBooCompleto.ToString()));

                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjOrderBook = new clsApiOrderbook(vStrRetornoAPI);

                this.tratarErro(vObjOrderBook.status_code, vObjOrderBook.error_message);
            }
            catch
            {
                throw;
            }
            return vObjOrderBook;

        }
        public override clsApiOrders buscarOrders(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, enumTipoOrdem aEnumTipordem)
        {
            string vStrRetornoAPI = string.Empty;
            int vIntTipoOrdem = (aEnumTipordem == enumTipoOrdem.Compra)?1:2;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiOrders vObjOrders = new clsApiOrders();
            try
            {
                vObjParametros.Add(new clsTooParametros("tapi_method", enumTipoTapiMetodo.list_orders.ToString()));
                vObjParametros.Add(new clsTooParametros("coin_pair", aEnumParMoedas.ToString()));
                if (aEnumTipordem != enumTipoOrdem.Ambas) vObjParametros.Add(new clsTooParametros("order_type", vIntTipoOrdem.ToString()));


                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjOrders = new clsApiOrders(vStrRetornoAPI);

                this.tratarErro(vObjOrders.status_code, vObjOrders.error_message);
            }
            catch
            {
                throw;
            }
            return vObjOrders;
        }
        public override clsApiOrders enviarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, enumTipoOrdem aEnumTipordem, decimal aQtdMoeda, decimal qVlrLimite)
        {
            string vStrRetornoAPI = string.Empty;
            string vStrTapiMethod = String.Empty;
            int vIntTipoOrdem = (aEnumTipordem == enumTipoOrdem.Compra) ? 1 : 2;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiOrders vObjOrders = new clsApiOrders();
            try
            {
                if (aEnumTipordem == enumTipoOrdem.Ambas) throw new Exception("Tipo de Ordem inválida: " + aEnumTipordem.ToString());

                vStrTapiMethod = (aEnumTipordem == enumTipoOrdem.Compra) ? enumTipoTapiMetodo.place_buy_order.ToString() : enumTipoTapiMetodo.place_sell_order.ToString();

                vObjParametros.Add(new clsTooParametros("tapi_method", vStrTapiMethod));
                vObjParametros.Add(new clsTooParametros("coin_pair", aEnumParMoedas.ToString()));
                vObjParametros.Add(new clsTooParametros("quantity", clsTooUtil.converterDecimalString_US(Math.Round(aQtdMoeda,8))));
                vObjParametros.Add(new clsTooParametros("limit_price", clsTooUtil.converterDecimalString_US(qVlrLimite)));

                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjOrders = new clsApiOrders(vStrRetornoAPI);

                this.tratarErro(vObjOrders.status_code, vObjOrders.error_message);

            }
            catch
            {
                throw;
            }
            return vObjOrders;
        }
        public override clsApiOrders cancelarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, long aLngOrderID)
        {
            string vStrRetornoAPI = String.Empty;
            string vStrTapiMethod = String.Empty;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiOrders vObjOrder = new clsApiOrders();
            try
            {
                vStrTapiMethod = enumTipoTapiMetodo.cancel_order.ToString();

                vObjParametros.Add(new clsTooParametros("tapi_method", vStrTapiMethod));
                vObjParametros.Add(new clsTooParametros("coin_pair", aEnumParMoedas.ToString()));
                vObjParametros.Add(new clsTooParametros("order_id", aLngOrderID.ToString()));

                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjOrder = new clsApiOrders(vStrRetornoAPI);

                this.tratarErro(vObjOrder.status_code, vObjOrder.error_message);

            }
            catch
            {
                throw;
            }
            return vObjOrder;
        }
        public override clsApiAccountInfo buscarInfoAccount()
        {
            string vStrRetornoAPI = String.Empty;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiAccountInfo vObjInfoAccount = new clsApiAccountInfo();
            try
            {
                vObjParametros.Add(new clsTooParametros("tapi_method", enumTipoTapiMetodo.get_account_info.ToString()));

                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjInfoAccount = new clsApiAccountInfo(vStrRetornoAPI);

                this.tratarErro(vObjInfoAccount.status_code, vObjInfoAccount.error_message);

            }
            catch
            {
                throw;
            }
            return vObjInfoAccount;
        }
        public override clsApiOrders buscarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, long aLngOrderID)
        {
            string vStrRetornoAPI = String.Empty;
            string vStrTapiMethod = String.Empty;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            clsApiOrders vObjOrder = new clsApiOrders();
            try
            {
                vStrTapiMethod = enumTipoTapiMetodo.get_order.ToString();

                vObjParametros.Add(new clsTooParametros("tapi_method", vStrTapiMethod));
                vObjParametros.Add(new clsTooParametros("coin_pair", aEnumParMoedas.ToString()));
                vObjParametros.Add(new clsTooParametros("order_id", aLngOrderID.ToString()));

                vStrRetornoAPI = this.chamarMetodoAPI_Negociacao(vObjParametros);
                vObjOrder = new clsApiOrders(vStrRetornoAPI);

                this.tratarErro(vObjOrder.status_code, vObjOrder.error_message);
            }
            catch
            {
                throw;
            }
            return vObjOrder;

        }




    }
}
