using System;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using bitCoinMonitor.tools;
using System.Collections.Generic;

using System.Text;
using bitCoinMonitor.api.objetos_tapi;

namespace bitCoinMonitor.api
{
    abstract class clsApiBase
    {


        public enum enumTipoMoeda { BTC, LTC, BCH, BTS }
        public enum enumTipoOrdem { Compra, Venda, Ambas }
        protected enum enumTipoMetodo { ticker, orderbook, trades }
        protected enum enumTipoTapiMetodo { get_account_info, get_order, list_orders, list_orderbook, orderbook, place_buy_order, place_sell_order, cancel_order }



        protected abstract string chamarMetodoAPI_Dados(enumTipoMoeda aEnumMoeda, enumTipoMetodo aEnumMetodo);
        protected abstract string chamarMetodoAPI_Negociacao(List<clsTooParametros> aObjParametros);
       
        protected abstract void tratarErro(int aIntErroCode, string aStrMsgErro);

        public clsApiTicker buscarTicker(enumTipoMoeda aEnumMoeda)
        {
            string vStrRetornoAPI;
            clsApiTicker vObjTicker;

            try
            {
                vStrRetornoAPI = this.chamarMetodoAPI_Dados(aEnumMoeda, enumTipoMetodo.ticker);
                if (vStrRetornoAPI == String.Empty)
                    vObjTicker = new clsApiTicker();
                else
                    vObjTicker = new clsApiTicker(vStrRetornoAPI);
            }
            catch
            {
                throw;
            }
            return vObjTicker;

        }
        public abstract clsApiOrderbook buscarOrderBook(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, bool aBooCompleto = false);
        public abstract clsApiOrders buscarOrders(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, enumTipoOrdem aEnumTipordem);
        public abstract clsApiOrders enviarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, enumTipoOrdem aEnumTipordem, decimal aQtdMoeda, decimal qVlrLimite);
        public abstract clsApiOrders cancelarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, long aLngOrderID);
        public abstract clsApiAccountInfo buscarInfoAccount();
        public abstract clsApiOrders buscarOrder(clsApiBaseRetornoTAPI.enumParMoedas aEnumParMoedas, long aLngOrderID);

    }

}





