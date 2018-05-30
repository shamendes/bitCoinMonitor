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
    public partial class frmViwEntrar : Form
    {
        private string _Senha;

        public frmViwEntrar()
        {
            InitializeComponent();
        } 
        
        public string pegarSenha()
        {
            this._Senha = String.Empty;
            this.ShowDialog();
            return this._Senha;
        }

        private void txtSenha_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this._Senha = txtSenha.Text;
                txtSenha.Text = String.Empty;
                this.Close();
            }
        }
    }
}
