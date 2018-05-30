using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace bitCoinMonitor.model
{
    class clsModLivroOrdens
    {
        private long _ID;
        private long _IDConsulta;
        private string _TipoOrdem;
        private long _IDOrdem;
        private string _FlagProprietario;
        private double _QtdNegociada;
        private decimal _VlrLimite;
        private long _IDUltimaOrdemNegociada;

        public enum enumTipoOrdem { Compra,Venda}

        public long pID { get { return this._ID; } private set { this._ID = value; } }
        public clsModConsulta pConsulta
        {
            get
            {
                try
                {
                    return new clsModConsulta(this._IDConsulta);
                }
                catch
                {
                    throw;
                }
            }
            set { this._IDConsulta = value.pID; }
        }
        public enumTipoOrdem pTipoOrdem
        {
            get { return this.definirTipoOrdem(this._TipoOrdem); }
            set { this._TipoOrdem = this.definirTipoOrdem(value); }
        }
        public long pIDOrdem { get { return this._IDOrdem; } set { this._IDOrdem = value; } }
        public bool pProprietario
        {
            get { return this._FlagProprietario == "S"; }
            set { this._FlagProprietario = (value) ? "S" : "N"; }
        }
        public double pQtdNegociada { get { return this._QtdNegociada; } set { this._QtdNegociada = value; } }
        public decimal pVlrLimite { get { return this._VlrLimite; } set { this._VlrLimite = value; } }
        public long pIDUltimaOrdemNegociada { get { return this._IDUltimaOrdemNegociada; } set { this._IDUltimaOrdemNegociada = value; } }


        private clsTooConexaoBD _ObjConexao;


        public clsModLivroOrdens()
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
        
        private string definirTipoOrdem(enumTipoOrdem aEnumTipoOrdem)
        {
            switch (aEnumTipoOrdem)
            {
                case enumTipoOrdem.Compra:
                    return "C";
                case enumTipoOrdem.Venda:
                    return "V";
                default:
                    return "C";
            }
        }
        private enumTipoOrdem definirTipoOrdem(string aStrTipoOrdem)
        {
            switch (aStrTipoOrdem)
            {
                case "C":
                    return enumTipoOrdem.Compra;
                case "V":
                    return enumTipoOrdem.Venda;
                default:
                    return enumTipoOrdem.Compra; 
            }
        }

        public int incluir()
        {
            const string cStrSqlInsert = "INSERT INTO TB_LIVRO_ORDENS VALUES(@IdConsulta, @TipoOrdem,@IdOrdem,@MrcProprietario,@QtdNegociada,@VlrLimite,@IdUltima)";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                //--Campos não nulos:
                vObjParametros.Add(new clsTooParametros("@IdConsulta", this._IDConsulta));
                vObjParametros.Add(new clsTooParametros("@TipoOrdem", this._TipoOrdem));
                vObjParametros.Add(new clsTooParametros("@IdOrdem", this._IDOrdem));
                vObjParametros.Add(new clsTooParametros("@MrcProprietario", this._FlagProprietario));
                vObjParametros.Add(new clsTooParametros("@QtdNegociada", this._QtdNegociada));
                vObjParametros.Add(new clsTooParametros("@VlrLimite", this._VlrLimite));
                vObjParametros.Add(new clsTooParametros("@IdUltima", this._IDUltimaOrdemNegociada));


                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlInsert, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;

        }
     
        public DataTable listarOrdens(clsModConsulta aObjConsulta, enumTipoOrdem aEnumTipoOrdem)
        {
            string vStrSQL = "SELECT * FROM TB_LIVRO_ORDENS WHERE IDT_CONSULTA = @IdConsulta AND COD_TIPO_ORDEM = @TipoOrdem ORDER BY VLR_PRECO_LIMITE ";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                if (aEnumTipoOrdem == enumTipoOrdem.Compra)
                    vStrSQL += "DESC";
                else
                    vStrSQL += "ASC";

                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                {
                    this._ID = Convert.ToInt32(vObjDados.Rows[0]["idt_registro"]);
                    this._IDConsulta = Convert.ToInt32(vObjDados.Rows[0]["idt_consulta_compra"]);
                    this._TipoOrdem = Convert.ToString(vObjDados.Rows[0]["cod_tipo_ordem"]);
                    this._IDOrdem = Convert.ToInt32(vObjDados.Rows[0]["idt_ordem"]);
                    this._FlagProprietario = Convert.ToString(vObjDados.Rows[0]["mrc_proprietario"]);
                    this._QtdNegociada = Convert.ToDouble(vObjDados.Rows[0]["qtd_negociada"]);
                    this._VlrLimite = Convert.ToDecimal(vObjDados.Rows[0]["vlr_preco_limite"]);
                    this._IDUltimaOrdemNegociada = Convert.ToInt32(vObjDados.Rows[0]["idt_ultima_negociada"]);
                }
                else
                {
                    this._ID = 0;
                    this._IDConsulta = 0;
                    this._TipoOrdem = String.Empty;
                    this._IDOrdem = 0;
                    this._FlagProprietario = String.Empty;
                    this._QtdNegociada = 0;
                    this._VlrLimite = 0;
                    this._IDUltimaOrdemNegociada = 0;
                }
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }
    }
}
