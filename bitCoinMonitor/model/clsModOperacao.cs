using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace bitCoinMonitor.model
{
    class clsModOperacao
    {
        private long _ID;
        private long _IDOrdem;
        private DateTime _DataOperacao;
        private decimal _QtdMoeda;
        private decimal _VlrOperacao;
        private decimal _VlrTaxa;

        public long pID { get { return this._ID; } set { this._ID = value; } }
        public clsModMinhasOrdens pOrdem
        {
            get
            {
                try
                {
                    return new clsModMinhasOrdens(this._IDOrdem);
                }
                catch
                {
                    throw;
                }
            }
            set { this._IDOrdem = value.pID; }
        }
        public DateTime pDataOperacao { get { return this._DataOperacao; } set { this._DataOperacao = value; } }
        public decimal pQtdMoeda { get { return this._QtdMoeda; } set { this._QtdMoeda = value; } }
        public decimal pVlrOperacao { get { return this._VlrOperacao; } set { this._VlrOperacao = value; } }
        public decimal pVlrTaxa { get { return this._VlrTaxa; } set { this._VlrTaxa = value; } }


        private clsTooConexaoBD _ObjConexao;

        public clsModOperacao()
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

        public int incluir()
        {
            const string cStrSqlInsert = "INSERT INTO TB_OPERACAO VALUES(@IdOrdem,@Id,@Data,@Quantidade,@Valor,@Taxa)";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                vObjParametros.Add(new clsTooParametros("@IdOrdem", this._IDOrdem));
                vObjParametros.Add(new clsTooParametros("@Id", this._ID));
                vObjParametros.Add(new clsTooParametros("@Data", this._DataOperacao));
                vObjParametros.Add(new clsTooParametros("@Quantidade", this._QtdMoeda));
                vObjParametros.Add(new clsTooParametros("@Valor", this._VlrOperacao)); 
                vObjParametros.Add(new clsTooParametros("@Taxa", this._VlrTaxa));

                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlInsert, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;

        }

        public DataTable listarOperacoes(clsModMinhasOrdens aObjOrdem)
        {
            string vStrSQL = "SELECT * FROM TB_OPERACAO WHERE IDT_ORDEM = @IdOrdem ORDER BY IDT_OPERACAO  ";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@IdOrdem", aObjOrdem.pID));
                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }

    }
}
