using bitCoinMonitor.model;
using System;
using System.Collections.Generic;
using System.Data;
using bitCoinMonitor.tools;
using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.api;

namespace bitCoinMonitor.control
{
    class clsCtrOperacao
    {

        private clsApiMercadoBitcon _ObjAPI;
        private clsApiOrders _ObjOrdemAPI;
        private decimal _QtdNegociada = 0;
        private decimal _ValorLimite = 0;
        private bool _AguardarAposCompra = false;
        private bool _Operar = false;


        public clsModConsulta pConsulta;

        public clsCtrOperacao()
        {
            this._ObjAPI = new clsApiMercadoBitcon();
            this._ObjOrdemAPI = new clsApiOrders();
            this.pConsulta = null;
        }

        public bool comprar(bool aBooOperacaoForcada = false)
        {
            try
            {
                if (aBooOperacaoForcada)
                    this.operarValorAtual(clsModMinhasOrdens.enumTipoOrdem.Compra);
                else
                    this.analisarOperacao(clsModMinhasOrdens.enumTipoOrdem.Compra);

                return executarOperacao(clsApiMercadoBitcon.enumTipoOrdem.Compra);
            }
            catch { throw; }
        }
        public bool vender(bool aBooOperacaoForcada = false)
        {
            try
            {
                if (aBooOperacaoForcada)
                    this.operarValorAtual(clsModMinhasOrdens.enumTipoOrdem.Venda);
                else
                    this.analisarOperacao(clsModMinhasOrdens.enumTipoOrdem.Venda);

                return executarOperacao(clsApiMercadoBitcon.enumTipoOrdem.Venda);
            }
            catch { throw; }
        }
        public DataTable listarOperacoes(DateTime aDatInicio, DateTime aDatFim)
        {
            clsModMinhasOrdens vObjOrdens;
            try
            {
                vObjOrdens = new clsModMinhasOrdens();
                return vObjOrdens.listarOrdens(aDatInicio, aDatFim);
            }
            catch
            {
                throw;
            }

        }
        public void atualizar()
        {
            clsModMinhasOrdens vObjOrdenAtual;

            try
            {
                vObjOrdenAtual = new clsModMinhasOrdens();

                vObjOrdenAtual.carregarOrdemAtual();

                if (vObjOrdenAtual.pID > 0)
                {
                    if (Program.Parametros.pSimulando)
                        this.preencherValoresSimulado(vObjOrdenAtual);
                    else
                        this._ObjOrdemAPI = this._ObjAPI.buscarOrder(clsApiBaseRetornoTAPI.enumParMoedas.BRLBTC, vObjOrdenAtual.pID);

                    this.gravarOrdem(false);
                }

            }
            catch { throw;}
        }
        public void cancelarOrdem(long aLngOrderID)
        {
            try
            {
                if(Program.Parametros.pSimulando == false)
                    this._ObjAPI.cancelarOrder(clsApiBaseRetornoTAPI.enumParMoedas.BRLBTC, aLngOrderID);
            }
            catch { throw; }

        }


        private bool executarOperacao(clsApiMercadoBitcon.enumTipoOrdem aEnumTipoOrdem)
        {
            try
            {
                //--Quem define se será feito a operação é o método analisarOperacao chamada dentro de cada método de operação (Comprar/Vender)
                if (this._Operar)
                {
                    System.Threading.Thread.Sleep(10000);
                    if (Program.Parametros.pSimulando)
                        this.preencherValoresSimulado(aEnumTipoOrdem);
                    else
                        this._ObjOrdemAPI = _ObjAPI.enviarOrder(clsApiBaseRetornoTAPI.enumParMoedas.BRLBTC, aEnumTipoOrdem, this._QtdNegociada, this._ValorLimite);

                    //--Gravando no BD a operação
                    this.gravarOrdem(true);
                }
            }
            catch{ throw; }
            finally
            {
                this._QtdNegociada = 0;
                this._ValorLimite = 0;
                this.pConsulta = null;
            }
            return true;
        }
        private void gravarOrdem(bool aBooNovo)
        {
            clsModMinhasOrdens vObjMinhaOrdem;

            try
            {
                vObjMinhaOrdem = new clsModMinhasOrdens();

                vObjMinhaOrdem.pID = this._ObjOrdemAPI.response_data.order.order_id;
                vObjMinhaOrdem.pConsulta = this.pConsulta;
                vObjMinhaOrdem.pDataCriacao = clsTooUtil.converterUnixTimeStamp(Convert.ToDecimal(this._ObjOrdemAPI.response_data.order.created_timestamp));
                vObjMinhaOrdem.pQtdExecutada = clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.executed_quantity);
                vObjMinhaOrdem.pQtdMoeda = clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.quantity);
                vObjMinhaOrdem.pStatus = vObjMinhaOrdem.definirTipoStatus(this._ObjOrdemAPI.response_data.order.status);
                vObjMinhaOrdem.pTipoOrdem = vObjMinhaOrdem.definirTipoOrdem(this._ObjOrdemAPI.response_data.order.order_type);
                vObjMinhaOrdem.pVlrLimite = this._ValorLimite;
                vObjMinhaOrdem.pVlrMedioExecutado = clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.executed_price_avg);
                vObjMinhaOrdem.pVlrTaxa = clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.fee);
                vObjMinhaOrdem.pAguardar = this._AguardarAposCompra;

                if (aBooNovo)
                    vObjMinhaOrdem.incluir();
                else
                    vObjMinhaOrdem.atualizar();

                this.gravarOperacoes(vObjMinhaOrdem);
            }
            catch { throw; }

        }
        private void gravarOperacoes(clsModMinhasOrdens aObjMinhaOrdem)
        {
            bool vBooPossuiOperacao = false;

            clsModOperacao vObjOperacao;

            try
            {
                vBooPossuiOperacao = this._ObjOrdemAPI.response_data.order.has_fills;

                if (vBooPossuiOperacao == false) return;

                //--Apagando as operações atuais para incluir novamente todas (isso para caso tenha surgido novas operações)
                aObjMinhaOrdem.limparOperacoes();

                //--Percorrendo as operações da ordem
                foreach (clsApiOrders_operations_data vObj in this._ObjOrdemAPI.response_data.order.operations)
                {
                    vObjOperacao = new clsModOperacao();

                    vObjOperacao.pDataOperacao = clsTooUtil.converterUnixTimeStamp(Convert.ToDecimal(vObj.executed_timestamp));
                    vObjOperacao.pID = vObj.operation_id;
                    vObjOperacao.pOrdem = aObjMinhaOrdem;
                    vObjOperacao.pQtdMoeda = clsTooUtil.converterStringDecimal_US(vObj.quantity);
                    vObjOperacao.pVlrOperacao = clsTooUtil.converterStringDecimal_US(vObj.price);
                    vObjOperacao.pVlrTaxa = clsTooUtil.converterStringDecimal_US(vObj.fee_rate);

                    vObjOperacao.incluir();

                }
            }
            catch { throw; }
        }
        private void analisarOperacao(clsModMinhasOrdens.enumTipoOrdem aEnumTipoOrdem)
        {
            DataTable vObjDados;
            try
            {
                vObjDados = this.pConsulta.analisarOperacao(aEnumTipoOrdem);

                if (vObjDados.Rows.Count > 0)
                {
                    this._Operar = (Convert.ToString(vObjDados.Rows[0]["MRC_OPERAR"]) == "S");
                    this._ValorLimite = Convert.ToDecimal(vObjDados.Rows[0]["VLR_LIMITE"]);
                    this._AguardarAposCompra = (Convert.ToString(vObjDados.Rows[0]["MRC_AGUARDAR"]) == "S");

                    if (this._Operar)
                    {
                        if (aEnumTipoOrdem == clsModMinhasOrdens.enumTipoOrdem.Compra)
                            this._QtdNegociada = (Program.Carteira.pSaldoMoeda * Convert.ToDecimal(Program.Parametros.pPctDisponivelCompra)) / this._ValorLimite;
                        else
                            this._QtdNegociada = Math.Round(Program.Carteira.pSaldoBitcoins, 8);

                        this._QtdNegociada = Math.Truncate(this._QtdNegociada * 100000000)/ 100000000;
                    }

                }
                else
                    this._Operar = false;
            }
            catch { throw; }

        }
        private void operarValorAtual(clsModMinhasOrdens.enumTipoOrdem aEnumTipoOrdem)
        {
            this._Operar = true;
            if (aEnumTipoOrdem == clsModMinhasOrdens.enumTipoOrdem.Compra)
            {
                this._ValorLimite = this.pConsulta.pCompra; 
                this._QtdNegociada = (Program.Carteira.pSaldoMoeda * Convert.ToDecimal(Program.Parametros.pPctDisponivelCompra)) / this._ValorLimite;
            }
            else
            {
                this._ValorLimite = this.pConsulta.pVenda;
                this._QtdNegociada = Program.Carteira.pSaldoBitcoins;
            }

            this._QtdNegociada = Math.Truncate(this._QtdNegociada * 100000000) / 100000000;
            this._AguardarAposCompra = false;

        }
        private void preencherValoresSimulado(clsApiMercadoBitcon.enumTipoOrdem aEnumTipoOrdem)
        {
            clsApiOrders_operations_data vObjOperacao;

            try
            {

                Random vRandNum = new Random();
                this._ObjOrdemAPI.response_data = new clsApiOrders_data();
                this._ObjOrdemAPI.response_data.order = new clsApiOrders_orders_data();
                this._ObjOrdemAPI.response_data.order.operations = new List<clsApiOrders_operations_data>();
                vObjOperacao = new clsApiOrders_operations_data();

                this._ObjOrdemAPI.response_data.order.order_id = vRandNum.Next();
                this._ObjOrdemAPI.response_data.order.created_timestamp = Convert.ToString((Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                this._ObjOrdemAPI.response_data.order.executed_quantity = clsTooUtil.converterDecimalString_US(this._QtdNegociada);
                this._ObjOrdemAPI.response_data.order.quantity = clsTooUtil.converterDecimalString_US(this._QtdNegociada);
                this._ObjOrdemAPI.response_data.order.status = 2;
                this._ObjOrdemAPI.response_data.order.order_type = (aEnumTipoOrdem == clsApiMercadoBitcon.enumTipoOrdem.Compra) ? 1 : 2;
                this._ObjOrdemAPI.response_data.order.limit_price = clsTooUtil.converterDecimalString_US(this._ValorLimite);
                this._ObjOrdemAPI.response_data.order.executed_price_avg = clsTooUtil.converterDecimalString_US(this._ValorLimite);
                this._ObjOrdemAPI.response_data.order.has_fills = true;

                if (aEnumTipoOrdem == clsApiMercadoBitcon.enumTipoOrdem.Compra)
                {
                    this._ObjOrdemAPI.response_data.order.fee = clsTooUtil.converterDecimalString_US(this._QtdNegociada * Convert.ToDecimal(Program.Parametros.pPctTaxaCompra));
                    Program.Carteira.atualizarCarteiraSimulado(Program.Carteira.pSaldoMoeda - (Program.Carteira.pSaldoMoeda * Convert.ToDecimal(Program.Parametros.pPctDisponivelCompra)), this._QtdNegociada - clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.fee));
                }
                else
                {
                    this._ObjOrdemAPI.response_data.order.fee = clsTooUtil.converterDecimalString_US((this._QtdNegociada * this._ValorLimite) * Convert.ToDecimal(Program.Parametros.pPctTaxaVenda));
                    Program.Carteira.atualizarCarteiraSimulado((this._QtdNegociada * this._ValorLimite) - clsTooUtil.converterStringDecimal_US(this._ObjOrdemAPI.response_data.order.fee), 0);
                }

                vObjOperacao.operation_id = 1;
                vObjOperacao.quantity = this._ObjOrdemAPI.response_data.order.quantity;
                vObjOperacao.price = this._ObjOrdemAPI.response_data.order.limit_price;
                vObjOperacao.fee_rate = this._ObjOrdemAPI.response_data.order.fee;
                vObjOperacao.executed_timestamp = this._ObjOrdemAPI.response_data.order.created_timestamp;

                this._ObjOrdemAPI.response_data.order.operations.Add(vObjOperacao);
            }
            catch { throw; }

        }
        private void preencherValoresSimulado(clsModMinhasOrdens aObjOrdem)
        {
            clsApiOrders_operations_data vObjOperacao;

            try
            {
                this._ObjOrdemAPI.response_data = new clsApiOrders_data();
                this._ObjOrdemAPI.response_data.order = new clsApiOrders_orders_data();
                this._ObjOrdemAPI.response_data.order.operations = new List<clsApiOrders_operations_data>();
                vObjOperacao = new clsApiOrders_operations_data();

                this._ObjOrdemAPI.response_data.order.order_id = Convert.ToInt32(aObjOrdem.pID);
                this._ObjOrdemAPI.response_data.order.created_timestamp = Convert.ToString((aObjOrdem.pDataCriacao.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
                this._ObjOrdemAPI.response_data.order.executed_quantity = clsTooUtil.converterDecimalString_US(aObjOrdem.pQtdExecutada);
                this._ObjOrdemAPI.response_data.order.quantity = clsTooUtil.converterDecimalString_US(aObjOrdem.pQtdMoeda);
                this._ObjOrdemAPI.response_data.order.status = 4;
                this._ObjOrdemAPI.response_data.order.order_type = (aObjOrdem.pTipoOrdem == clsModMinhasOrdens.enumTipoOrdem.Compra) ? 1 : 2;
                this._ObjOrdemAPI.response_data.order.limit_price = clsTooUtil.converterDecimalString_US(aObjOrdem.pVlrLimite);
                this._ObjOrdemAPI.response_data.order.executed_price_avg = clsTooUtil.converterDecimalString_US(aObjOrdem.pVlrLimite);
                this._ObjOrdemAPI.response_data.order.has_fills = true;

                this._ObjOrdemAPI.response_data.order.fee = clsTooUtil.converterDecimalString_US(aObjOrdem.pVlrTaxa);

                vObjOperacao.operation_id = 1;
                vObjOperacao.quantity = this._ObjOrdemAPI.response_data.order.quantity;
                vObjOperacao.price = this._ObjOrdemAPI.response_data.order.limit_price;
                vObjOperacao.fee_rate = this._ObjOrdemAPI.response_data.order.fee;
                vObjOperacao.executed_timestamp = this._ObjOrdemAPI.response_data.order.created_timestamp;

                this._ObjOrdemAPI.response_data.order.operations.Add(vObjOperacao);
            }
            catch { throw; }

        }


    }
}
