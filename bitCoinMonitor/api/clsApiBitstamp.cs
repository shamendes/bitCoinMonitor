using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bitCoinMonitor.api
{
    class clsApiBitstamp
    {
        protected string chamarMetodoAPI_Dados()
        {
            string vStrRetorno = String.Empty;
            string vStrURL = "https://www.bitstamp.net/api/ticker/";

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

        public clsApiTickerBitstamp buscarTicker()
        {
            string vStrRetornoAPI;
            clsApiTickerBitstamp vObjTicker;

            try
            {
                vStrRetornoAPI = this.chamarMetodoAPI_Dados();
                if (vStrRetornoAPI == String.Empty)
                    vObjTicker = new clsApiTickerBitstamp();
                else
                    vObjTicker = new clsApiTickerBitstamp(vStrRetornoAPI);
            }
            catch
            {
                throw;
            }
            return vObjTicker;

        }

    }


}
