using bitCoinMonitor.api;
using bitCoinMonitor.api.objetos_tapi;
using bitCoinMonitor.control;
using bitCoinMonitor.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bitCoinMonitor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static clsCtrParametros Parametros;
        public static clsCtrCarteira Carteira;

        [STAThread]
        static void Main()
        {
                       
            try
            {
                Parametros = new clsCtrParametros();
                Parametros.carregar();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //--Pedindo para o usuário informar qual é a corretora
                if (Parametros.pCorretora == clsCtrParametros.enumCorretora.Indefinido)
                {
                    using (frmViwParametros vObjJanelaParametros = new frmViwParametros())
                        vObjJanelaParametros.ShowDialog();
                }

                Carteira = new clsCtrCarteira();


                Application.Run(new frmViwMonitor());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ops!",MessageBoxButtons.OK,MessageBoxIcon.Error);                
            }
        }
    }
}
