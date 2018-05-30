using bitCoinMonitor.api;
using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.tools;
using System;

namespace bitCoinMonitor.control
{
    class clsCtrCarteira
    {
        private decimal _SaldoMoeda;
        private decimal _SaldoBitcoins;

        public decimal pSaldoMoeda { get { return this._SaldoMoeda; } }
        public decimal pSaldoBitcoins { get { return this._SaldoBitcoins; } }

        public clsCtrCarteira()
        {
            try
            {
                if (Program.Parametros.pSimulando)
                {
                    this._SaldoMoeda = 0;
                    this._SaldoBitcoins = Convert.ToDecimal(0.07051);
                }
                else
                    this.atualizarCarteira();
            }
            catch
            {
                throw;
            }
        }

        public void atualizarCarteira()
        {
            clsApiMercadoBitcon vObjAPI = new clsApiMercadoBitcon();
            clsApiAccountInfo vObjInfo;

            try
            {
                if (Program.Parametros.pSimulando == false)
                {
                    vObjInfo = vObjAPI.buscarInfoAccount();

                    this._SaldoMoeda = clsTooUtil.converterStringDecimal_US(vObjInfo.response_data.balance.brl.available);
                    this._SaldoBitcoins = clsTooUtil.converterStringDecimal_US(vObjInfo.response_data.balance.btc.available);

                    this._SaldoMoeda = Math.Truncate(this._SaldoMoeda * 100) / 100;
                    this._SaldoBitcoins = Math.Truncate(this._SaldoBitcoins * 100000) / 100000;

                }
            }
            catch
            {
                throw;
            }
        }

        public void atualizarCarteiraSimulado(decimal aDecVlrMoeda, decimal aDecQtdBitcoins)
        {
            if(Program.Parametros.pSimulando)
            {
                this._SaldoMoeda = aDecVlrMoeda;
                this._SaldoBitcoins = aDecQtdBitcoins;
            }
        }

    }
}
