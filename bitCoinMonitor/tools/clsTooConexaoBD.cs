using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;

namespace bitCoinMonitor.tools
{
 
    class clsTooParametros
    {
        public clsTooParametros(string aStrNome, object aObjValor)
        {
            this.pNome = aStrNome;
            this.pValor = aObjValor;
        }

        public string pNome { get; set; }
        public object pValor { get; set; }

    }


    class clsTooConexaoBD
    {

        private SqlConnection _ObjConexao;

        public clsTooConexaoBD()
        {
            try
            {
                this._ObjConexao = new SqlConnection(Properties.Settings.Default.dbConnectionString);
            }
            catch
            {
                throw;
            }
        }


        private void abrir()
        {
            try
            {
                this._ObjConexao.Open();
            }
            catch
            {
                throw;
            }
        }
        private void fechar()
        {
            try
            {
                this._ObjConexao.Close();
            }
            catch
            {
                throw;
            }
        }

        public int executarNonQuery(string aStrSQL, List<clsTooParametros> aObjParametros)
        {
            SqlCommand vObjCommand;
            int vIntRows =0 ;

            try
            {
                //--Abrindo conexao;
                this.abrir();

                vObjCommand = new SqlCommand(aStrSQL, this._ObjConexao);

                //--Abastecendo os parâmetros;
                foreach (clsTooParametros vObjParametros in aObjParametros)
                    vObjCommand.Parameters.AddWithValue(vObjParametros.pNome, vObjParametros.pValor);


                //--Executando o comando no banco
                vIntRows = vObjCommand.ExecuteNonQuery();


            }
            catch(Exception ex)
            {
                string message;
                message = ex.Message;
            }
            finally{
                //--Fechando conexao;
                this.fechar();                
            }
            return vIntRows;
        }
        public DataTable executarSelect(string aStrSQL, List<clsTooParametros> aObjParametros)
        {
            SqlCommand vObjCommand;
            DataTable vObjDados;
            SqlDataAdapter vObjDataAdapter;

            try
            {
                //--Abrindo conexao;
                this.abrir();

                vObjCommand = new SqlCommand(aStrSQL, this._ObjConexao);                

                //--Abastecendo os parâmetros;
                foreach (clsTooParametros vObjParametros in aObjParametros)
                    vObjCommand.Parameters.AddWithValue(vObjParametros.pNome, vObjParametros.pValor);

                vObjDataAdapter = new SqlDataAdapter(vObjCommand);
                vObjDados = new DataTable();

                //--Preenchendo o DataTable com os dados retornados pelo SQLCommand
                vObjDataAdapter.Fill(vObjDados);

            }
            finally
            {
                //--Fechando conexao;
                this.fechar();
            }
            return vObjDados;
        }

    }


    
}
