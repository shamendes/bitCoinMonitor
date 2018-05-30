using bitCoinMonitor.model;
using System.Data;
using System;
using System.Timers;
using bitCoinMonitor.api;
using bitCoinMonitor.api.objetos_tapi;

namespace bitCoinMonitor.control
{
    class clsCtrMonitor
    {        
        private clsApiBase _API;
        private Timer _Timer;
        private DateTime _DataInicio = new DateTime(1984,3,27);
        private DateTime _DataFim = new DateTime(2200,1,1);
        private bool _NegociacaoAtiva = false;
        private bool _OperacaoForcada = false;
        

        public enum enumAcao { Coletando, MonitorandoCompra, MonitorandoVenda, Comprando, Vendendo };

        public decimal pValorCompra { get; set; }
        public decimal pValorVenda { get; set; }
        public int pPeriodoMaxMin { get; set; }
        public enumAcao pAcaoAtual { get; private set; }
        public double pIntervalo { get { return this._Timer.Interval; } set { this._Timer.Interval = value; } }
        public DateTime pDataInicio { get{ return this._DataInicio; } set { this._DataInicio = value;}}
        public DateTime pDataFim{ get { return this._DataFim; } set {this._DataFim = value;} }
        public bool pAtivaNegociacao{ get { return this._NegociacaoAtiva; } set {  this._NegociacaoAtiva = value;}}
        public DataTable pObjHistorico { get; private set; }
        public clsApiTicker pObjTickerMaxMin { get; private set; }
        public clsApiTickerBitstamp pObjTickerMaxMinBS { get; private set; }
        public clsApiTicker pObjTickerAtual { get; private set; }
        public clsApiTickerBitstamp pObjTickerBSAtual { get; private set; }
        public clsApiOrderbook pObjOrderBookAtual { get; private set; }
        public Exception pErro;

        public clsCtrMonitor()
        {
            try
            {

                if (Program.Parametros.pCorretora == clsCtrParametros.enumCorretora.MercadoBitcoin)
                    this._API = new clsApiMercadoBitcon();
                else
                    this._API = new clsApiFoxBit();

                this.pObjTickerMaxMin = new clsApiTicker();
                this.pObjTickerMaxMin.ticker = new clsApiTicker_data();
                this.pObjTickerMaxMinBS = new clsApiTickerBitstamp();
                this.pObjTickerMaxMinBS.ticker = new clsApiTickerBitstamp_data();


                this._Timer = new Timer();
                this._Timer.Elapsed += new ElapsedEventHandler(executarCiclo);


            }
            catch
            {
                throw;
            }
        }

        
        public void iniciar()
        {
            try
            {
                this.pAcaoAtual = enumAcao.Coletando;
                this._Timer.Start();
            }
            catch
            {
                throw;
            }
        }
        public void parar()
        {
            this._Timer.Stop();
        }

        //--Método para verificar a situação do movimento. Se existe venda pendente ou se começa um ciclo novo
        //--Caso tenha venda pendente, retorna a cotação comprada.
        public void buscarAcaoAtual()
        {
            clsModMinhasOrdens vObjOrdens;
            enumAcao vEnumAcao = enumAcao.Coletando;
            try
            {
                if (this.pAtivaNegociacao)
                {
                    vObjOrdens = new clsModMinhasOrdens();
                    
                    vObjOrdens.carregarOrdemAtual();

                    if (vObjOrdens.pID == 0)
                        vEnumAcao = (Program.Carteira.pSaldoBitcoins != 0) ? enumAcao.MonitorandoVenda : enumAcao.MonitorandoCompra ;
                    else
                    {
                        //--Verificando em que situação está a movimentação (se está comprando ou em vendendo)
                        switch (vObjOrdens.pStatus)
                        {
                            case clsModMinhasOrdens.enumTipoStatus.Aberta:
                                vEnumAcao = (vObjOrdens.pTipoOrdem == clsModMinhasOrdens.enumTipoOrdem.Compra) ? enumAcao.Comprando : enumAcao.Vendendo;
                                break;
                            case clsModMinhasOrdens.enumTipoStatus.Finalizada:
                                vEnumAcao = (vObjOrdens.pTipoOrdem == clsModMinhasOrdens.enumTipoOrdem.Compra) ? enumAcao.MonitorandoVenda : enumAcao.MonitorandoCompra;
                                break;
                            default:
                                vEnumAcao = enumAcao.MonitorandoCompra;
                                break;
                        }
                    }

                }

                //--Se teve alteração de ação, chama função para atualizar carteira.
                if (vEnumAcao != this.pAcaoAtual)
                {
                    System.Threading.Thread.Sleep(20000);
                    Program.Carteira.atualizarCarteira();
                    System.Threading.Thread.Sleep(10000);
                }
                this.pAcaoAtual = vEnumAcao;
            }
            catch
            {
                throw;
            }
        }
        public void enviarOrdem()
        {
            this._OperacaoForcada = true;
        }
        
        //--Método que é executado em loop - Consulta o valor atual e carrega histórico
        private void executarCiclo(object source, ElapsedEventArgs e)
        {
            try
            {
                this._Timer.Stop();
                
                this.buscarAcaoAtual();
                this.consultarBitCoin();
                this.carregarHistorico();
                this.buscarMaxMinPeriodo();
                this._Timer.Start();
            }
            catch(Exception aObjErro)
            {
                this.pErro = aObjErro;
                this.parar();                
                this.iniciar();
            }
        }

        //--Método que consulta no MercadoBitcoin e grava os dados retornados no repositório de consultas
        private void consultarBitCoin()
        {
            clsModConsulta vObjConsulta;
            //clsCtrLivroOrdens vObjLivro;
            clsApiTicker vObjTicker;

            try
            {
                //--Se o usuário optou por forçar a operação não faz consulta do ticker, vai direto para a negociação utilizando o último ticker consultado.
                if (this._OperacaoForcada == false)
                {

                    System.Threading.Thread.Sleep(15000);
                    //--Buscando as cotações;
                    vObjTicker = this._API.buscarTicker(clsApiMercadoBitcon.enumTipoMoeda.BTC);

                    //--Se tanto a compra quanto a venda estiverem zeradas, significa que o MercadoBitcoin não respondeu nossa consulta.
                    if (vObjTicker.ticker.buy == 0 && vObjTicker.ticker.sell == 0) return;

                    //--Lógica incluída para minimizar problemas conhecidos do serviço do MercadoBitcoin:
                    if (this.pObjTickerAtual != null)
                    {
                        //--Se o valor de mínimo está zerado, pega o mínimo da consulta anterior (erro do MercadoBitcoin)
                        if (vObjTicker.ticker.low == 0 ) vObjTicker.ticker.low = this.pObjTickerAtual.ticker.low;

                        //--Retornar os mesmos dados em requisições diferentes ou retornar valor 1 na cotação
                        if (this.pObjTickerAtual.ticker.verDataHora() == vObjTicker.ticker.verDataHora() || vObjTicker.ticker.sell == 1 || vObjTicker.ticker.buy == 1)
                            return;
                    }

                    //--Buscando o livro de ordens:
                    this.pObjOrderBookAtual = this._API.buscarOrderBook(clsApiBaseRetornoTAPI.enumParMoedas.BRLBTC);

                    //--Gravando no Banco de Dados a consulta e o livro de ordens
                    vObjConsulta = new clsModConsulta(vObjTicker);
                    vObjConsulta.incluir();
                    //vObjLivro = new clsCtrLivroOrdens();
                    //vObjLivro.registrar(vObjConsulta, this.pObjOrderBookAtual);
                    this.pObjTickerAtual = vObjTicker;

                    //--Consultando bitstamp
                    this.consultarBitstamp();

                }
                else
                {
                    vObjConsulta = new clsModConsulta();
                    vObjConsulta.buscarUltimaConsulta();
                }


                //--Chamando função que irá verificar se negocia ou não com a cotação consultada
                if (this.pAtivaNegociacao)
                    this.negociar(vObjConsulta);
            }
            catch
            {
                throw;
            }

        }
        private void consultarBitstamp()
        {
            clsModConsultaBitstamp vObjConsulta;
            clsApiBitstamp vObjBitstamp;
            clsApiTickerBitstamp vObjTicker;
            
            try
            {

                vObjBitstamp = new clsApiBitstamp();
                vObjTicker = vObjBitstamp.buscarTicker();

                //--Gravando no Banco de Dados a consulta do Bitstamp
                vObjConsulta = new clsModConsultaBitstamp(vObjTicker);
                vObjConsulta.incluir();

                this.pObjTickerBSAtual = vObjTicker;
            }
            catch { throw; }

        }
        private void negociar(clsModConsulta aObjConsulta)
        {
            clsCtrOperacao vObjOperacao; ;

            try
            {
                vObjOperacao = new clsCtrOperacao();
                vObjOperacao.pConsulta = aObjConsulta;

                switch (this.pAcaoAtual)
                {
                    case enumAcao.MonitorandoCompra: //--Compra bitcoins
                        if(this.validarNegociacao()) vObjOperacao.comprar(this._OperacaoForcada);
                        break;
                    case enumAcao.MonitorandoVenda://--Vende bitcoins
                        if (this.validarNegociacao()) vObjOperacao.vender(this._OperacaoForcada);
                        break;
                    case enumAcao.Comprando: //--Aguardando execução da ordem de compra
                        System.Threading.Thread.Sleep(10000);
                        vObjOperacao.atualizar();
                        System.Threading.Thread.Sleep(10000);
                        break;
                    case enumAcao.Vendendo:
                        System.Threading.Thread.Sleep(10000);
                        vObjOperacao.atualizar();//--Aguardando execução da ordem de venda
                        System.Threading.Thread.Sleep(10000);
                        break;
                    default: //nada fazer    
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                this._OperacaoForcada = false;
            }
        }
        private void carregarHistorico()
        {
            try
            {
                clsModConsulta vObjConsulta = new clsModConsulta();

                //--Recuperando o histórico
                this.pObjHistorico = vObjConsulta.listarConsultasPorHora(this._DataInicio, this._DataFim);
            }
            catch
            {
                throw;
            }
        }
        private bool validarNegociacao()
        {
            bool vBooRetorno = false;

            vBooRetorno = (this.pValorCompra > 0 && this.pValorVenda > 0);
            vBooRetorno = vBooRetorno && ((this.pAcaoAtual == enumAcao.MonitorandoCompra && pObjTickerAtual.ticker.sell < this.pValorCompra) ||
                                          (this.pAcaoAtual == enumAcao.MonitorandoVenda && pObjTickerAtual.ticker.buy > this.pValorVenda));
           return vBooRetorno;;
        }

        public void buscarMaxMinPeriodo()
        {
            try
            {
                clsModConsulta vObjConsulta = new clsModConsulta();
                clsModConsultaBitstamp vObjConsultaBitstamp = new clsModConsultaBitstamp();

                //--Recuperando o Máximo e mínimo dentro o período informado
                vObjConsulta.verMaxMinPeriodo(this.pPeriodoMaxMin * -1);
                vObjConsultaBitstamp.verMaxMinPeriodo(this.pPeriodoMaxMin * -1);
                this.pObjTickerMaxMin.ticker.low = vObjConsulta.pMinimo;
                this.pObjTickerMaxMin.ticker.high = vObjConsulta.pMaximo;
                this.pObjTickerMaxMinBS.ticker.low = vObjConsultaBitstamp.pMinimo;
                this.pObjTickerMaxMinBS.ticker.high = vObjConsultaBitstamp.pMaximo;

            }
            catch { throw; }
        }


    }
}
