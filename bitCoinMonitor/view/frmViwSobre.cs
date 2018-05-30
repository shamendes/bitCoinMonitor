using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bitCoinMonitor.view
{
    public partial class frmViwSobre : Form
    {
        public frmViwSobre()
        {
            InitializeComponent();
        }

        private void frmViwSobre_Load(object sender, EventArgs e)
        {
            DateTime vDatBuild = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;

            this.lblSistema.Text = Application.ProductName;
            this.lblVersao.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.lblBuild.Text = vDatBuild.ToString();
            
        }
    }
}
