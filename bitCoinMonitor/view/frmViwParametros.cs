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
    public partial class frmViwParametros : Form
    {
        public frmViwParametros()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Program.Parametros.pVlrDifCompraXVenda = Convert.ToDecimal(this.txtDifCompraVenda.Text.Replace("R$", ""));
            Program.Parametros.pPctDistanciaCompraDoMax  = Convert.ToDouble( this.txtDistanciaMax.Text.Replace("%", ""))/100;
            Program.Parametros.pPctDisponivelCompra = Convert.ToDouble(this.txtPctDisponivelCompra.Text.Replace("%", ""))/100;
            Program.Parametros.pQtdRegistrosAnteriores = Convert.ToInt32(this.txtQtdRegistros.Text);
            Program.Parametros.pIdtSegredoTAPI = this.txtSegredoTapi.Text;
            Program.Parametros.pIdtTAPI= this.txtTapi.Text;
            Program.Parametros.pPctTaxaCompra = Convert.ToDouble(this.txtTaxaCompra.Text.Replace("%", ""))/100;
            Program.Parametros.pPctTaxaVenda = Convert.ToDouble(this.txtTaxaVenda.Text.Replace("%", ""))/100;
            Program.Parametros.pCorretora = (this.cmbCorretora.SelectedIndex == 0) ? control.clsCtrParametros.enumCorretora.MercadoBitcoin : control.clsCtrParametros.enumCorretora.FoxBit;
            Program.Parametros.pVlrDifMaxMin = Convert.ToDecimal(this.txtDifMaxMin.Text.Replace("R$", ""));
            Program.Parametros.pMrcNegociacaoAtiva = this.chkNegociacaoAtiva.Checked;


            Program.Parametros.gravar();

            this.Close();
        }

        private void frmViwParametros_Load(object sender, EventArgs e)
        {
            this.txtDifCompraVenda.Text = Program.Parametros.pVlrDifCompraXVenda.ToString("C");
            this.txtDistanciaMax.Text = Program.Parametros.pPctDistanciaCompraDoMax.ToString("P5");
            this.txtPctDisponivelCompra.Text = Program.Parametros.pPctDisponivelCompra.ToString("P5");
            this.txtQtdRegistros.Text = Program.Parametros.pQtdRegistrosAnteriores.ToString();
            this.txtSegredoTapi.Text = Program.Parametros.pIdtSegredoTAPI.ToString();
            this.txtTapi.Text = Program.Parametros.pIdtTAPI.ToString();
            this.txtTaxaCompra.Text = Program.Parametros.pPctTaxaCompra.ToString("P5");
            this.txtTaxaVenda.Text = Program.Parametros.pPctTaxaVenda.ToString("P5");
            this.cmbCorretora.SelectedIndex = (Program.Parametros.pCorretora == control.clsCtrParametros.enumCorretora.MercadoBitcoin)?0: (Program.Parametros.pCorretora == control.clsCtrParametros.enumCorretora.FoxBit)?1:-1;
            this.txtDifMaxMin.Text = Program.Parametros.pVlrDifMaxMin.ToString("C");
            this.chkNegociacaoAtiva.Checked = Program.Parametros.pMrcNegociacaoAtiva;
        }
    }
}
