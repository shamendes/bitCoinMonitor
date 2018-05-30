using bitCoinMonitor.model;
using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace bitCoinMonitor.control
{
    class clsCtrResultado
    {
        private clsModMinhasOrdens _ObjMinhasOrdens;

        public decimal pValorInicial;
        public decimal pGanhos;
        public decimal pPerdas;
        public DataTable pObjDadosAnual;
        public DataTable pObjDadosMensal;
        public DataTable pObjDadosRentabilidade;
        public DataTable pObjDadosTaxas;

        public clsCtrResultado()
        {
            this._ObjMinhasOrdens = new clsModMinhasOrdens();
        }

        public void carregarValoresAnual(int aIntAno)
        {
            DateTime vDatInicio = new DateTime(aIntAno, 1, 1);
            DateTime vDatFim = new DateTime(aIntAno, 12, 31);
            try
            {

                //--Pegando o valor inicial investido
                this._ObjMinhasOrdens.carregarPrimeiraOrdem();
                this.pValorInicial = (this._ObjMinhasOrdens.pQtdExecutada * this._ObjMinhasOrdens.pVlrMedioExecutado);

                this.pObjDadosAnual = this._ObjMinhasOrdens.listarResultado(vDatInicio, vDatFim);
                this.pObjDadosRentabilidade = this._ObjMinhasOrdens.listarRentabilidade(vDatInicio, vDatFim);
            }
            catch { throw; }

        }
        public void carregarValoresMensal(int aIntAno, int aIntMes)
        {
            DataTable vObjGanhosPerdas;

            DateTime vDatInicio = new DateTime(aIntAno, aIntMes, 1);
            DateTime vDatFim = new DateTime(aIntAno, aIntMes, clsTooUtil.retornarUltimoDiaMes(new DateTime(aIntAno,aIntMes, 15)));
            try
            {
                this.pObjDadosMensal = this._ObjMinhasOrdens.listarResultado(vDatInicio, vDatFim);
                this.pObjDadosTaxas = this._ObjMinhasOrdens.listarTaxas(vDatInicio, vDatFim);
                vObjGanhosPerdas = this._ObjMinhasOrdens.buscarGanhosPerdas(vDatInicio, vDatFim);

                if(vObjGanhosPerdas.Rows.Count > 0)
                {
                    this.pGanhos = Convert.ToDecimal(vObjGanhosPerdas.Rows[0]["VLR_GANHO"]);
                    this.pPerdas = Convert.ToDecimal(vObjGanhosPerdas.Rows[0]["VLR_PERDA"]);
                }
                else
                {
                    this.pGanhos = 0;
                    this.pPerdas = 0;
                }
            }
            catch { throw; }

        }
    }
}
