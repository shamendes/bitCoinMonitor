using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Data;

namespace bitCoinMonitor.model
{
    class clsModMinhasOrdens
    {
        private long _ID;
        private string _TipoOrdem;
        private int _Status;
        private DateTime _DataCriacao;
        private DateTime _DataAtualizacao;
        private long _IDConsulta;
        private decimal _VlrLimite;
        private decimal _QtdMoeda;
        private decimal _QtdExecutada;
        private decimal _VlrMedioExecutado;
        private decimal _VlrTaxa;
        private string _MrcAguardar;

        public enum enumTipoOrdem { Compra, Venda }
        public enum enumTipoStatus { Aberta, Cancelada, Finalizada }

        public long pID { get { return this._ID; } set { this._ID = value; } }
        public enumTipoOrdem pTipoOrdem
        {
            get { return this.definirTipoOrdem(this._TipoOrdem); }
            set { this._TipoOrdem = this.definirTipoOrdem(value); }
        }
        public enumTipoStatus pStatus
        {
            get { return this.definirTipoStatus(this._Status); }
            set { this._Status = this.definirTipoStatus(value); }
        }
        public DateTime pDataCriacao { get { return this._DataCriacao; } set { this._DataCriacao = value; } }
        public DateTime pDataAtualizacao { get { return this._DataAtualizacao; } set { this._DataAtualizacao = value; } }
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
        public decimal pVlrLimite { get { return this._VlrLimite; } set { this._VlrLimite = value; } }
        public decimal pQtdMoeda { get { return this._QtdMoeda; } set { this._QtdMoeda = value; } }
        public decimal pQtdExecutada { get { return this._QtdExecutada; } set { this._QtdExecutada = value; } }
        public decimal pVlrMedioExecutado { get { return this._VlrMedioExecutado; } set { this._VlrMedioExecutado = value; } }
        public decimal pVlrTaxa { get { return this._VlrTaxa; } set { this._VlrTaxa = value; } }
        public bool pAguardar { get { return this._MrcAguardar == "S"; } set { this._MrcAguardar = (value) ? "S" : "N"; } }


        private clsTooConexaoBD _ObjConexao;


        public clsModMinhasOrdens()
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

        public clsModMinhasOrdens(long aLngID)
        {
            try
            {
                instanciarConexao();
                this.buscarOrdem(aLngID);
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
        public enumTipoOrdem definirTipoOrdem(int aIntTipoOrdem)
        {
            switch (aIntTipoOrdem)
            {
                case 1:
                    return enumTipoOrdem.Compra;
                case 2:
                    return enumTipoOrdem.Venda;
                default:
                    return enumTipoOrdem.Compra;
            }
        }


        private int definirTipoStatus(enumTipoStatus aEnumTipoStatus)
        {
            switch (aEnumTipoStatus)
            {
                case enumTipoStatus.Aberta:
                    return 2;
                case enumTipoStatus.Cancelada:
                    return 3;
                case enumTipoStatus.Finalizada:
                    return 4;
                default:
                    return 4;
            }
        }
        public enumTipoStatus definirTipoStatus(int aIntTipoStatus)
        {
            switch (aIntTipoStatus)
            {
                case 2:
                    return enumTipoStatus.Aberta;
                case 3:
                    return enumTipoStatus.Cancelada;
                case 4:
                    return enumTipoStatus.Finalizada;
                default:
                    return enumTipoStatus.Finalizada;
            }
        }

        public int incluir()
        {
            const string cStrSqlInsert = "INSERT INTO TB_MINHAS_ORDENS VALUES(@Id, @TipoOrdem,@Status,@DataCriacao,@DataAtualizacao,@IDConsulta,@VlrLimite,@QtdMoeda,@QtdExecutada,@VlrMedioExecutado,@VlrTaxa, @MrcAguardar)";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {

                vObjParametros.Add(new clsTooParametros("@Id", this._ID));
                vObjParametros.Add(new clsTooParametros("@TipoOrdem", this._TipoOrdem));
                vObjParametros.Add(new clsTooParametros("@Status", this._Status));
                vObjParametros.Add(new clsTooParametros("@DataCriacao", this._DataCriacao));
                vObjParametros.Add(new clsTooParametros("@DataAtualizacao", this._DataCriacao)); //--mesma datahora da criacao
                vObjParametros.Add(new clsTooParametros("@IDConsulta", this._IDConsulta));
                vObjParametros.Add(new clsTooParametros("@VlrLimite", this._VlrLimite));
                vObjParametros.Add(new clsTooParametros("@QtdMoeda", this._QtdMoeda));
                vObjParametros.Add(new clsTooParametros("@QtdExecutada", this._QtdExecutada));
                vObjParametros.Add(new clsTooParametros("@VlrMedioExecutado", this._VlrMedioExecutado));
                vObjParametros.Add(new clsTooParametros("@VlrTaxa", this._VlrTaxa));
                vObjParametros.Add(new clsTooParametros("@MrcAguardar", this._MrcAguardar));

                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlInsert, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;

        }      

        public int atualizar()
        {
            const string cStrSqlInsert = "UPDATE TB_MINHAS_ORDENS " +
                                         "SET    IDT_STATUS = @Status, " +
                                         "       DAT_ATUALIZACAO = @DataAtualizacao, " +
                                         "       QTD_EXECUTADA = @QtdExecutada, " +
                                         "       VLR_MEDIO_EXECUTADO = @VlrMedioExecutado," +
                                         "       VLR_TAXA = @VlrTaxa " +
                                         "WHERE IDT_ORDEM = @Id";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                vObjParametros.Add(new clsTooParametros("@Id", this._ID));
                vObjParametros.Add(new clsTooParametros("@Status", this._Status));
                vObjParametros.Add(new clsTooParametros("@DataAtualizacao", this._DataCriacao)); //--mesma datahora da criacao
                vObjParametros.Add(new clsTooParametros("@QtdExecutada", this._QtdExecutada));
                vObjParametros.Add(new clsTooParametros("@VlrMedioExecutado", this._VlrMedioExecutado));
                vObjParametros.Add(new clsTooParametros("@VlrTaxa", this._VlrTaxa));

                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlInsert, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;
        }
        private void buscarOrdem(long aLngID)
        {
            const string cStrSQL = "SELECT * FROM TB_MINHAS_ORDENS WHERE IDT_ORDEM = @ID";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@ID", aLngID));

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                {
                    this._ID = Convert.ToInt32(vObjDados.Rows[0]["IDT_ORDEM"]);
                    this._TipoOrdem = Convert.ToString(vObjDados.Rows[0]["IDT_TIPO_ORDEM"]);
                    this._Status = Convert.ToInt32(vObjDados.Rows[0]["IDT_STATUS"]);
                    this._DataCriacao = Convert.ToDateTime(vObjDados.Rows[0]["DAT_CRIACAO"]);
                    this._DataAtualizacao = Convert.ToDateTime(vObjDados.Rows[0]["DAT_ATUALIZACAO"]);
                    this._IDConsulta = Convert.ToInt32(vObjDados.Rows[0]["IDT_CONSULTA_ORIGEM"]);
                    this._VlrLimite = Convert.ToDecimal(vObjDados.Rows[0]["VLR_LIMITE"]);
                    this._QtdMoeda = Convert.ToDecimal(vObjDados.Rows[0]["QTD_MOEDA"]);
                    this._QtdExecutada = Convert.ToDecimal(vObjDados.Rows[0]["QTD_EXECUTADA"]);
                    this._VlrMedioExecutado = Convert.ToDecimal(vObjDados.Rows[0]["VLR_MEDIO_EXECUTADO"]);
                    this._VlrTaxa = Convert.ToDecimal(vObjDados.Rows[0]["VLR_TAXA"]);
                    this._MrcAguardar = Convert.ToString(vObjDados.Rows[0]["MRC_AGUARDAR"]);
                }
                else
                {

                    this._ID = 0;
                    this._TipoOrdem = String.Empty;
                    this._Status = 0;
                    this._DataCriacao = new DateTime(1900, 1, 1);
                    this._DataAtualizacao = new DateTime(1900, 1, 1);
                    this._IDConsulta = 0;
                    this._VlrLimite = 0;
                    this._QtdMoeda = 0;
                    this._QtdExecutada = 0;
                    this._VlrMedioExecutado = 0;
                    this._VlrTaxa = 0;
                    this._MrcAguardar = "N";
                }

            }
            catch
            {
                throw;
            }

        }

        public DataTable listarOrdens( DateTime aDatInicio, DateTime aDatFim)
        {
            string vStrSQL = "SELECT	IDT_ORDEM, " +
                             "          CASE WHEN IDT_TIPO_ORDEM = 'C' THEN 'Compra' ELSE 'Venda' END TIPO_ORDEM," +
                             "          CASE IDT_STATUS WHEN 2 THEN 'Aberta' WHEN 3 THEN 'Cancelada'  ELSE 'Finalizada' END STATUS," +
                             "          DAT_CRIACAO," +
                             "          DAT_ATUALIZACAO," +
                             "          IDT_CONSULTA_ORIGEM," +
                             "          VLR_LIMITE," +
                             "          QTD_MOEDA," +
                             "          QTD_EXECUTADA," +
                             "          VLR_MEDIO_EXECUTADO," +
                             "          VLR_TAXA " +
                             "FROM      TB_MINHAS_ORDENS WHERE DAT_CRIACAO BETWEEN @DataInicio AND @DataFim ORDER BY DAT_CRIACAO DESC  ";
            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }

        public void carregarOrdemAtual()
        {
            const string cStrSQL = "SELECT TOP 1 IDT_ORDEM FROM TB_MINHAS_ORDENS ORDER BY DAT_CRIACAO DESC";
            long vLngId;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                    vLngId = Convert.ToInt32(vObjDados.Rows[0]["IDT_ORDEM"]);
                else
                    vLngId = 0;

                this.buscarOrdem(vLngId);
            }
            catch
            {
                throw;
            }


        }
        public void carregarPrimeiraOrdem()
        {
            const string cStrSQL = "SELECT TOP 1 IDT_ORDEM FROM TB_MINHAS_ORDENS ORDER BY DAT_CRIACAO";
            long vLngId;

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                    vLngId = Convert.ToInt32(vObjDados.Rows[0]["IDT_ORDEM"]);
                else
                    vLngId = 0;

                this.buscarOrdem(vLngId);
            }
            catch
            {
                throw;
            }


        }


        public void limparOperacoes()
        {
            string vStrSQL = "DELETE TB_OPERACAO WHERE IDT_ORDEM = @IdOrdem";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();

            try
            {
                vObjParametros.Add(new clsTooParametros("@IdOrdem", this.pID));
                this._ObjConexao.executarNonQuery(vStrSQL, vObjParametros);
            }
            catch { throw; }
        }

        public DataTable listarResultado(DateTime aDatInicio, DateTime aDatFim)
        {
            string vStrSQL = "SELECT DADOS.* FROM (" +
                                            "   SELECT    * " +
                                            "   FROM VW_RESULTADO_OPERACAO " +
                                            "   UNION " +
                                            "   SELECT  GETDATE(),O.VLR_LIMITE* O.QTD_MOEDA, NULL, NULL " +
                                            "   FROM    TB_MINHAS_ORDENS O INNER JOIN(SELECT MAX(DAT_CRIACAO) DAT_CRIACAO FROM TB_MINHAS_ORDENS) MAX_DATA " +
                                            "               ON(MAX_DATA.DAT_CRIACAO = O.DAT_CRIACAO) " +
                                            "   WHERE   O.IDT_TIPO_ORDEM = 'C' " +
                                            ") DADOS " +
                             "WHERE DADOS.DAT_ORDEM BETWEEN @DataInicio AND @DataFim " +
                             "ORDER BY DADOS.DAT_ORDEM DESC ";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }
        public DataTable buscarGanhosPerdas(DateTime aDatInicio, DateTime aDatFim)
        {
            string vStrSQL = "SELECT    ISNULL(ABS(SUM(CASE WHEN VLR_INVESTIDO - VLR_RECEBIDO < 0 THEN VLR_INVESTIDO - VLR_RECEBIDO ELSE 0 END)),0) VLR_GANHO, " +
                             "          ISNULL(ABS(SUM(CASE WHEN VLR_INVESTIDO - VLR_RECEBIDO > 0 THEN VLR_INVESTIDO - VLR_RECEBIDO ELSE 0 END)),0) VLR_PERDA " +
                             "FROM      VW_RESULTADO_OPERACAO " +
                             "WHERE     DAT_ORDEM BETWEEN @DataInicio AND @DataFim";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }
        public DataTable listarRentabilidade(DateTime aDatInicio, DateTime aDatFim)
        {
            string vStrSQL = "SELECT	M.MES, M.NOME, " +
                             "          SUM(R.VLR_RECEBIDO) / SUM(R.VLR_INVESTIDO) - 1 PCT_RENTABILIDADE " +
                             "FROM      SIS_TB_MESES M LEFT JOIN VW_RESULTADO_OPERACAO R " +
                             "              ON(MONTH(R.DAT_ORDEM) = M.MES AND R.DAT_ORDEM BETWEEN @DataInicio AND @DataFim) " +
                             "GROUP BY  M.MES, M.NOME " +
                             "ORDER BY M.MES ";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

                vObjDados = this._ObjConexao.executarSelect(vStrSQL, vObjParametros);
            }
            catch
            {
                throw;
            }

            return vObjDados;
        }
        public DataTable listarTaxas(DateTime aDatInicio, DateTime aDatFim)
        {
            string vStrSQL = "SELECT    CONVERT(DECIMAL(6,3),ROUND(VLR_TAXA/CASE WHEN IDT_TIPO_ORDEM = 'C' THEN QTD_EXECUTADA ELSE QTD_EXECUTADA * VLR_MEDIO_EXECUTADO END,3)) VLR_TAXA, " +
                             "          COUNT(1) QTD_VEZES " +
                             "FROM      TB_MINHAS_ORDENS " +
                             "WHERE     DAT_ATUALIZACAO BETWEEN @DataInicio AND @DataFim " +
                             "AND       CASE WHEN IDT_TIPO_ORDEM = 'C' THEN QTD_EXECUTADA ELSE QTD_EXECUTADA *VLR_MEDIO_EXECUTADO END > 0 " +
                             "GROUP BY  ROUND(VLR_TAXA / CASE WHEN IDT_TIPO_ORDEM = 'C' THEN QTD_EXECUTADA ELSE QTD_EXECUTADA * VLR_MEDIO_EXECUTADO END, 3)";


            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {
                vObjParametros.Add(new clsTooParametros("@DataInicio", aDatInicio));
                vObjParametros.Add(new clsTooParametros("@DataFim", aDatFim));

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
