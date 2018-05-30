using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Data;
using static bitCoinMonitor.model.clsModMinhasOrdens;

namespace bitCoinMonitor.model
{
    class clsModConsulta
    {

        private long _ID;
        private decimal _Maximo;
        private decimal _Minimo;
        private decimal _Volume;
        private decimal _Ultimo;
        private decimal _Compra;
        private decimal _Venda;
        private DateTime _Data;

        private clsTooConexaoBD _ObjConexao;

        public clsModConsulta()
        {
            try
            {

                instanciarConexao();
            }
            catch
            {
                throw;
            }
        }

        public clsModConsulta(clsApiTicker aObjTicker)
        {

            try
            {
                this.pCompra = aObjTicker.ticker.sell; //--invertido propositalmente
                this.pData = aObjTicker.ticker.verDataHora();
                this.pMaximo = aObjTicker.ticker.high;
                this.pMinimo = aObjTicker.ticker.low;
                this.pUltimo = aObjTicker.ticker.last;
                this.pVenda = aObjTicker.ticker.buy; //--invertido propositalmente
                this.pVolume = aObjTicker.ticker.vol;

                instanciarConexao();
            }
            catch
            {
                throw;
            }

        }

        public clsModConsulta(long aIntID)
        {
            try
            {
                instanciarConexao();
                this.buscarConsulta(aIntID);
            }
            catch
            {
                throw;
            }

        }
        private void instanciarConexao()
        {
            try
            {
                this._ObjConexao = new clsTooConexaoBD();
            }
            catch
            {
                throw;
            }

        }

        public long pID {
            get { return this._ID; }
        }

        public decimal pMaximo
        {
            get { return this._Maximo; }
            set { this._Maximo = value; }
        }

        public decimal pMinimo
        {
            get { return this._Minimo; }
            set { this._Minimo = value; }
        }
        public decimal pVolume
        {
            get { return this._Volume; }
            set { this._Volume = value; }
        }
        public decimal pUltimo
        {
            get { return this._Ultimo; }
            set { this._Ultimo = value; }
        }
        public decimal pCompra
        {
            get { return this._Compra; }
            set { this._Compra = value; }
        }
        public decimal pVenda
        {
            get { return this._Venda; }
            set { this._Venda = value; }
        }
        public DateTime pData
        {
            get { return this._Data; }
            set { this._Data = value; }
        }

        private long buscarMaxID()
        {
            const string cStrSQL = "SELECT MAX(idt_consulta) idt_consulta FROM TB_CONSULTA";
            long vLngRetorno = 0;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                    vLngRetorno = Convert.ToInt32(vObjDados.Rows[0]["idt_consulta"]);
                else
                    vLngRetorno = 0;
            }
            catch
            {
                throw;
            }
            return vLngRetorno;

        }
        private void buscarConsulta(long aLngID)
        {
            const string cStrSQL = "SELECT * FROM TB_CONSULTA WHERE IDT_CONSULTA = @ID";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@ID", aLngID));

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                {
                    this._ID = Convert.ToInt32(vObjDados.Rows[0]["idt_consulta"]);
                    this._Maximo = Convert.ToDecimal(vObjDados.Rows[0]["vlr_maximo"]);
                    this._Minimo = Convert.ToDecimal(vObjDados.Rows[0]["vlr_minimo"]);
                    this._Volume = Convert.ToDecimal(vObjDados.Rows[0]["vlr_volume"]);
                    this._Ultimo = Convert.ToDecimal(vObjDados.Rows[0]["vlr_ultimo"]);
                    this._Compra = Convert.ToDecimal(vObjDados.Rows[0]["vlr_compra"]);
                    this._Venda = Convert.ToDecimal(vObjDados.Rows[0]["vlr_venda"]);
                    this._Data = Convert.ToDateTime(vObjDados.Rows[0]["dat_consulta"]);
                }
                else
                {
                    this._ID = 0;
                    this._Maximo = 0;
                    this._Minimo = 0;
                    this._Volume = 0;
                    this._Ultimo = 0;
                    this._Compra = 0;
                    this._Venda = 0;
                    this._Data = new DateTime(1900, 1, 1);
                }
            }
            catch
            {
                throw;
            }

        }

        public int incluir()
        {
            const string cStrSqlInsert = "INSERT INTO TB_CONSULTA VALUES(@Data, @Maximo,@Minimo,@Ultimo,@Volume,@Compra,@Venda)";

            

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {

                vObjParametros.Add(new clsTooParametros("@Data", this._Data));
                vObjParametros.Add(new clsTooParametros("@Maximo", this._Maximo));
                vObjParametros.Add(new clsTooParametros("@Minimo", this._Minimo));
                vObjParametros.Add(new clsTooParametros("@Ultimo", this._Ultimo));
                vObjParametros.Add(new clsTooParametros("@Volume", this._Volume));
                vObjParametros.Add(new clsTooParametros("@Compra", this._Compra));
                vObjParametros.Add(new clsTooParametros("@Venda", this._Venda));

                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlInsert, vObjParametros);

                if (vIntRetorno > 0)
                    this._ID = buscarMaxID();
            }
            catch
            {
                throw;
            }

            return vIntRetorno;

        }        
        public void buscarUltimaConsulta()
        {
            try { this.buscarConsulta(this.buscarMaxID()); }
            catch { throw; }
        }
        public DataTable listarConsultasPorHora(DateTime aDatInicio, DateTime aDatFim)
        {
            const string cStrSQL = "SELECT * FROM VW_CONSULTAS_POR_HORA WHERE DAT_CONSULTA BETWEEN @DataInicio AND @DataFim ORDER BY IDT_CONSULTA DESC";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vObjDados;

        }
        public DataTable analisarOperacao(enumTipoOrdem aEnumTipoOrdem)
        {            
            const string cStrNomeSPCompra = "SP_ANALISAR_COMPRA";
            const string cStrNomeSPVenda= "SP_ANALISAR_VENDA";

            string vStrSQL = String.Empty;
            
            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            
            try
            {
                vStrSQL = (aEnumTipoOrdem == enumTipoOrdem.Compra) ? cStrNomeSPCompra : cStrNomeSPVenda;
                vStrSQL = "EXEC " + vStrSQL + " @Id ";

                vObjParametros.Add(new clsTooParametros("@Id", this.pID));

                return this._ObjConexao.executarSelect(vStrSQL, vObjParametros);                
            }
            catch
            {
                throw;
            }

        }
        
        public void verMaxMinPeriodo(int aIntQtdDias)
        {

            const string cStrSQL = "SELECT MIN(vlr_ultimo) vlr_minimo, MAX(vlr_ultimo) vlr_maximo " +
                                   "FROM TB_CONSULTA " +
                                   "WHERE dat_consulta BETWEEN DATEADD(DAY,@Dias, GETDATE()) AND GETDATE()";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@Dias", aIntQtdDias));

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                {
                    this._ID = 0;
                    this._Maximo = Convert.ToDecimal(vObjDados.Rows[0]["vlr_maximo"]);
                    this._Minimo = Convert.ToDecimal(vObjDados.Rows[0]["vlr_minimo"]);
                    this._Volume = 0;
                    this._Ultimo = 0;
                    this._Compra = 0;
                    this._Venda = 0;
                    this._Data = new DateTime(1900, 1, 1);
                }
                else
                {
                    this._ID = 0;
                    this._Maximo = 0;
                    this._Minimo = 0;
                    this._Volume = 0;
                    this._Ultimo = 0;
                    this._Compra = 0;
                    this._Venda = 0;
                    this._Data = new DateTime(1900, 1, 1);
                }
            }
            catch
            {
                throw;
            }            

        }

        
    }
}
