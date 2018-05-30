using bitCoinMonitor.model;
using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using bitCoinMonitor.api.objetos_tapi;

namespace bitCoinMonitor.control
{
    class clsCtrLivroOrdens
    {
        private clsModLivroOrdens _ObjDados;

        public clsCtrLivroOrdens()
        {
            this._ObjDados = new clsModLivroOrdens();
        }



        public void registrar(clsModConsulta aObjConsulta, clsApiOrderbook aObjOrderBook)
        {
           

            try
            {
                
                this._ObjDados.pConsulta = aObjConsulta;                
                this._ObjDados.pIDUltimaOrdemNegociada= aObjOrderBook.response_data.orderbook.latest_order_id;
                
                //--incluindo a lista de compras (bids)
                foreach (clsApiOrderbook_bids_asks_data obj in aObjOrderBook.response_data.orderbook.bids)
                {
                    this._ObjDados.pIDOrdem = obj.order_id;
                    this._ObjDados.pTipoOrdem = clsModLivroOrdens.enumTipoOrdem.Compra;
                    this._ObjDados.pProprietario = obj.is_owner;
                    this._ObjDados.pQtdNegociada = Convert.ToDouble(clsTooUtil.converterStringDecimal_US(obj.quantity));
                    this._ObjDados.pVlrLimite = clsTooUtil.converterStringDecimal_US(obj.limit_price);
                    this._ObjDados.incluir();
                }

                //--incluindo a lista de vendas(asks)
                foreach (clsApiOrderbook_bids_asks_data obj in aObjOrderBook.response_data.orderbook.asks)
                {
                    this._ObjDados.pIDOrdem = obj.order_id;
                    this._ObjDados.pTipoOrdem = clsModLivroOrdens.enumTipoOrdem.Venda;
                    this._ObjDados.pProprietario = obj.is_owner;
                    this._ObjDados.pQtdNegociada = Convert.ToDouble(clsTooUtil.converterStringDecimal_US(obj.quantity));
                    this._ObjDados.pVlrLimite = clsTooUtil.converterStringDecimal_US(obj.limit_price);
                    this._ObjDados.incluir();
                }

            }
            catch
            {
                throw;
            }
        }

        public DataTable consultarCompras(clsModConsulta aObjConsulta)
        {
            return this._ObjDados.listarOrdens(aObjConsulta, clsModLivroOrdens.enumTipoOrdem.Compra);
        }

        public DataTable consultarVendas(clsModConsulta aObjConsulta)
        {
            return this._ObjDados.listarOrdens(aObjConsulta, clsModLivroOrdens.enumTipoOrdem.Venda);
        }


    }
}
