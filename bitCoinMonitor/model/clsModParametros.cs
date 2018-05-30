using bitCoinMonitor.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace bitCoinMonitor.model
{
    class clsModParametros
    {

        private clsTooConexaoBD _ObjConexao;



        public double pPctTaxaCompra { get; set; }
        public double pPctTaxaVenda { get; set; }
        public int pQtdRegistrosAnteriores { get; set; }
        public double pPctDisponivelCompra { get; set; }
        public string pIdtTAPI { get; set; }
        public string pIdtSegredoTAPI { get; set; }
        public decimal pVlrDifCompraXVenda { get; set; }
        public double pPctDistanciaCompraDoMax { get; set; }
        public int pIdtCorretora { get; set; }
        public decimal pVlrDifMaxMin { get; set; }
        public string pMrcNegociacaoAtiva { get; set; }


        public clsModParametros()
        {
            try
            {
                this.instanciarConexao();
                this.buscar();
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

        public void buscar()
        {
            const string cStrSQL = "SELECT * FROM TB_PARAMETROS";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                {

                    this.pPctTaxaCompra = Convert.ToDouble(vObjDados.Rows[0]["pct_taxa_compra"]);
                    this.pPctTaxaVenda = Convert.ToDouble(vObjDados.Rows[0]["pct_taxa_venda"]);
                    this.pQtdRegistrosAnteriores = Convert.ToInt32(vObjDados.Rows[0]["qtd_registros_anterior"]);
                    this.pPctDisponivelCompra = Convert.ToDouble(vObjDados.Rows[0]["pct_disponivel_compra"]);
                    this.pIdtTAPI = Convert.ToString(vObjDados.Rows[0]["idt_tapi"]);
                    this.pIdtSegredoTAPI = Convert.ToString(vObjDados.Rows[0]["idt_segredo_tapi"]);
                    this.pVlrDifCompraXVenda = Convert.ToDecimal(vObjDados.Rows[0]["vlr_dif_compra_x_venda"]);
                    this.pPctDistanciaCompraDoMax = Convert.ToDouble(vObjDados.Rows[0]["pct_ditancia_compra_max"]);
                    this.pIdtCorretora = Convert.ToInt32(vObjDados.Rows[0]["idt_corretora"]);
                    this.pVlrDifMaxMin = Convert.ToDecimal(vObjDados.Rows[0]["vlr_dif_max_min"]);
                    this.pMrcNegociacaoAtiva = Convert.ToString(vObjDados.Rows[0]["mrc_negociacao_ativa"]);

                }
                else //--Inicializa com os valores padrão
                {
                    this.pPctTaxaCompra = 0.007;
                    this.pPctTaxaVenda = 0.007;
                    this.pQtdRegistrosAnteriores = 2500;
                    this.pPctDisponivelCompra = 1;
                    this.pIdtTAPI = "";
                    this.pIdtSegredoTAPI = "";
                    this.pVlrDifCompraXVenda = 150;
                    this.pPctDistanciaCompraDoMax = 0.1;
                    this.pIdtCorretora = 1;
                    this.pVlrDifMaxMin = 3000;
                    this.pMrcNegociacaoAtiva = "N";

                    this.incluir();
                }
            }
            catch
            {
                throw;
            }

        }

        public int incluir()
        {
            const string cStrSqlInsert = "INSERT INTO TB_PARAMETROS VALUES(@PctTaxaCompra, @PctTaxaVenda,@PctDisponivel,@IdtTAPI,@IdtSegredoTAPI,@QtdRegistros,@VlrDifCompraVenda,@PctDistanciaCompraMax,@IdCorretora, @VlrDifMaxMin, @AtivaNegociacao, @Senha)";


            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                //--Campos não nulos:
                vObjParametros.Add(new clsTooParametros("@PctTaxaCompra", this.pPctTaxaCompra));
                vObjParametros.Add(new clsTooParametros("@PctTaxaVenda", this.pPctTaxaVenda));
                vObjParametros.Add(new clsTooParametros("@QtdRegistros", this.pQtdRegistrosAnteriores));
                vObjParametros.Add(new clsTooParametros("@PctDisponivel", this.pPctDisponivelCompra));
                vObjParametros.Add(new clsTooParametros("@IdtTAPI", this.pIdtTAPI));
                vObjParametros.Add(new clsTooParametros("@IdtSegredoTAPI", this.pIdtSegredoTAPI));
                vObjParametros.Add(new clsTooParametros("@VlrDifCompraVenda", this.pVlrDifCompraXVenda));
                vObjParametros.Add(new clsTooParametros("@PctDistanciaCompraMax", this.pPctDistanciaCompraDoMax));
                vObjParametros.Add(new clsTooParametros("@IdCorretora", this.pIdtCorretora));
                vObjParametros.Add(new clsTooParametros("@VlrDifMaxMin", this.pVlrDifMaxMin));
                vObjParametros.Add(new clsTooParametros("@AtivaNegociacao", this.pMrcNegociacaoAtiva));
                vObjParametros.Add(new clsTooParametros("@Senha", DBNull.Value));

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
            const string cStrSqlUpdate = "UPDATE TB_PARAMETROS " +
                                         "SET    pct_taxa_compra = @PctTaxaCompra, " +
                                         "       pct_taxa_venda = @PctTaxaVenda, " +
                                         "       qtd_registros_anterior = @QtdRegistros," +
                                         "       pct_disponivel_compra = @PctDisponivel," +
                                         "       idt_tapi = @IdtTAPI," +
                                         "       idt_segredo_tapi = @IdtSegredoTAPI, " +
                                         "       vlr_dif_compra_x_venda = @VlrDifCompraVenda, " +
                                         "       pct_ditancia_compra_max = @PctDistanciaCompraMax, " +
                                         "       idt_corretora = @IdCorretora, " +
                                         "       vlr_dif_max_min = @VlrDifMaxMin, " +
                                         "       mrc_negociacao_ativa = @AtivaNegociacao ";

            List <clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                vObjParametros.Add(new clsTooParametros("@PctTaxaCompra", this.pPctTaxaCompra));
                vObjParametros.Add(new clsTooParametros("@PctTaxaVenda", this.pPctTaxaVenda));
                vObjParametros.Add(new clsTooParametros("@QtdRegistros", this.pQtdRegistrosAnteriores));
                vObjParametros.Add(new clsTooParametros("@PctDisponivel", this.pPctDisponivelCompra));
                vObjParametros.Add(new clsTooParametros("@IdtTAPI", this.pIdtTAPI));
                vObjParametros.Add(new clsTooParametros("@IdtSegredoTAPI", this.pIdtSegredoTAPI));
                vObjParametros.Add(new clsTooParametros("@VlrDifCompraVenda", this.pVlrDifCompraXVenda));
                vObjParametros.Add(new clsTooParametros("@PctDistanciaCompraMax", this.pPctDistanciaCompraDoMax));
                vObjParametros.Add(new clsTooParametros("@IdCorretora", this.pIdtCorretora));
                vObjParametros.Add(new clsTooParametros("@VlrDifMaxMin", this.pVlrDifMaxMin));
                vObjParametros.Add(new clsTooParametros("@AtivaNegociacao", this.pMrcNegociacaoAtiva));


                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlUpdate, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;
        }

        public string buscarSenha()
        {
            string vStrSenha = String.Empty;

            const string cStrSQL = "SELECT dcr_senha FROM TB_PARAMETROS";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            DataTable vObjDados;

            try
            {

                vObjDados = this._ObjConexao.executarSelect(cStrSQL, vObjParametros);

                if (vObjDados.Rows.Count > 0)
                    vStrSenha = Convert.ToString((vObjDados.Rows[0]["dcr_senha"] == DBNull.Value)? String.Empty: vObjDados.Rows[0]["dcr_senha"]);
            }
            catch{throw;}

            return vStrSenha;
        }

        public int atualizarSenha(string aStrNovaSenha)
        {
            const string cStrSqlUpdate = "UPDATE TB_PARAMETROS SET dcr_senha = @Senha";

            List<clsTooParametros> vObjParametros = new List<clsTooParametros>();
            int vIntRetorno = 0;

            try
            {
                vObjParametros.Add(new clsTooParametros("@Senha", aStrNovaSenha));

                vIntRetorno = this._ObjConexao.executarNonQuery(cStrSqlUpdate, vObjParametros);
            }
            catch
            {
                throw;
            }
            return vIntRetorno;
        }

    }
}
