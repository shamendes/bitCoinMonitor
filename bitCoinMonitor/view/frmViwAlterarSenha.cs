using bitCoinMonitor.control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bitCoinMonitor.view
{
    public partial class frmViwAlterarSenha : Form
    {
        public frmViwAlterarSenha()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string vStrMensagem = String.Empty;

            clsCtrSenha vObjSenha;


            try
            {
                vObjSenha = new clsCtrSenha();

                vStrMensagem = vObjSenha.atualizarSenha(this.txtSenhaAtual.Text, this.txtNovaSenha.Text, this.txtConfirmaNovaSenha.Text);

                if (vStrMensagem != String.Empty)
                    MessageBox.Show(vStrMensagem, "Alteração de senha", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    MessageBox.Show("Senha alterada com sucesso!", "Alteração de senha", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViwAlterarSenha_Load(object sender, EventArgs e)
        {
            this.lblSenhaAtual.Enabled = Program.Parametros.pPossuiSenha;
            this.txtSenhaAtual.Enabled = Program.Parametros.pPossuiSenha;
        }
    }
}
