using bitCoinMonitor.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitCoinMonitor.control
{

    class clsCtrParametros
    {

        private clsModParametros _Parametros;
        private int _IdtCorretora;
        private string _MrcNegociacaoAtiva;

        public enum enumCorretora { Indefinido,MercadoBitcoin, FoxBit}


        public double pPctTaxaCompra { get; set; }
        public double pPctTaxaVenda { get; set; }
        public int pQtdHorasAnalise { get; set; }
        public int pQtdRegistrosAnteriores { get; set; }
        public double pPctDisponivelCompra { get; set; }
        public string pIdtTAPI { get; set; }
        public string pIdtSegredoTAPI { get; set; }
        public decimal pVlrDifCompraXVenda { get; set; }
        public double pPctDistanciaCompraDoMax { get; set; }
        public bool pSimulando { get; set; }
        public enumCorretora pCorretora {
            get { return (this._IdtCorretora == 1) ? enumCorretora.MercadoBitcoin : (this._IdtCorretora == 2)?enumCorretora.FoxBit:enumCorretora.Indefinido; }
            set { this._IdtCorretora = (value == enumCorretora.MercadoBitcoin) ? 1 : (value == enumCorretora.FoxBit)? 2:0; }
        }
        public decimal pVlrDifMaxMin { get; set; }
        public bool pMrcNegociacaoAtiva { get { return this._MrcNegociacaoAtiva == "S"; } set { this._MrcNegociacaoAtiva = (value) ? "S" : "N"; } }
        public bool pPossuiSenha
        {
            get { return (this._Parametros.buscarSenha() != String.Empty); }
        }


        public void carregar()
        {
            try
            {
                this._Parametros = new clsModParametros();

                this.pPctTaxaCompra = this._Parametros.pPctTaxaCompra;
                this.pPctTaxaVenda = this._Parametros.pPctTaxaVenda;
                this.pQtdRegistrosAnteriores = this._Parametros.pQtdRegistrosAnteriores;
                this.pPctDisponivelCompra = this._Parametros.pPctDisponivelCompra;
                this.pIdtTAPI = this._Parametros.pIdtTAPI;
                this.pIdtSegredoTAPI = this._Parametros.pIdtSegredoTAPI;
                this.pVlrDifCompraXVenda = this._Parametros.pVlrDifCompraXVenda;
                this.pPctDistanciaCompraDoMax = this._Parametros.pPctDistanciaCompraDoMax;
                this._IdtCorretora = this._Parametros.pIdtCorretora;
                this.pVlrDifMaxMin = this._Parametros.pVlrDifMaxMin;
                this._MrcNegociacaoAtiva = this._Parametros.pMrcNegociacaoAtiva;
                this.pSimulando = false;

            }
            catch
            {
                throw;
            }
        }
        public void gravar()
        {
            try
            {
                this._Parametros.pPctTaxaCompra = this.pPctTaxaCompra;
                this._Parametros.pPctTaxaVenda = this.pPctTaxaVenda;
                this._Parametros.pQtdRegistrosAnteriores = this.pQtdRegistrosAnteriores;
                this._Parametros.pPctDisponivelCompra = this.pPctDisponivelCompra;
                this._Parametros.pIdtTAPI = this.pIdtTAPI;
                this._Parametros.pIdtSegredoTAPI = this.pIdtSegredoTAPI;
                this._Parametros.pVlrDifCompraXVenda = this.pVlrDifCompraXVenda;
                this._Parametros.pPctDistanciaCompraDoMax = this.pPctDistanciaCompraDoMax;
                this._Parametros.pIdtCorretora = this._IdtCorretora;
                this._Parametros.pVlrDifMaxMin = this.pVlrDifMaxMin;
                this._Parametros.pMrcNegociacaoAtiva = this._MrcNegociacaoAtiva;

                this._Parametros.atualizar();
            }
            catch
            {
                throw;
            }
        }

    }
}
