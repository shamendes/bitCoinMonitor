namespace bitCoinMonitor.view
{
    partial class frmViwParametros
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViwParametros));
            this.grbMercadoBitcoin = new System.Windows.Forms.GroupBox();
            this.cmbCorretora = new System.Windows.Forms.ComboBox();
            this.lblCorretora = new System.Windows.Forms.Label();
            this.txtSegredoTapi = new System.Windows.Forms.TextBox();
            this.lblSegredoTapi = new System.Windows.Forms.Label();
            this.txtTapi = new System.Windows.Forms.TextBox();
            this.lblTapi = new System.Windows.Forms.Label();
            this.txtTaxaVenda = new System.Windows.Forms.TextBox();
            this.txtTaxaCompra = new System.Windows.Forms.TextBox();
            this.lblTaxaVenda = new System.Windows.Forms.Label();
            this.lblTaxaCompra = new System.Windows.Forms.Label();
            this.grbEstatisticas = new System.Windows.Forms.GroupBox();
            this.txtQtdRegistros = new System.Windows.Forms.TextBox();
            this.lblQtdRegistros = new System.Windows.Forms.Label();
            this.grbNegociacao = new System.Windows.Forms.GroupBox();
            this.txtDifMaxMin = new System.Windows.Forms.TextBox();
            this.lblDifMaxMin = new System.Windows.Forms.Label();
            this.txtDistanciaMax = new System.Windows.Forms.TextBox();
            this.lblDistanciaMax = new System.Windows.Forms.Label();
            this.txtDifCompraVenda = new System.Windows.Forms.TextBox();
            this.txtPctDisponivelCompra = new System.Windows.Forms.TextBox();
            this.lblDifCompraVenda = new System.Windows.Forms.Label();
            this.lblPctDisponivelCompra = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkNegociacaoAtiva = new System.Windows.Forms.CheckBox();
            this.grbMercadoBitcoin.SuspendLayout();
            this.grbEstatisticas.SuspendLayout();
            this.grbNegociacao.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbMercadoBitcoin
            // 
            this.grbMercadoBitcoin.Controls.Add(this.cmbCorretora);
            this.grbMercadoBitcoin.Controls.Add(this.lblCorretora);
            this.grbMercadoBitcoin.Controls.Add(this.txtSegredoTapi);
            this.grbMercadoBitcoin.Controls.Add(this.lblSegredoTapi);
            this.grbMercadoBitcoin.Controls.Add(this.txtTapi);
            this.grbMercadoBitcoin.Controls.Add(this.lblTapi);
            this.grbMercadoBitcoin.Controls.Add(this.txtTaxaVenda);
            this.grbMercadoBitcoin.Controls.Add(this.txtTaxaCompra);
            this.grbMercadoBitcoin.Controls.Add(this.lblTaxaVenda);
            this.grbMercadoBitcoin.Controls.Add(this.lblTaxaCompra);
            this.grbMercadoBitcoin.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbMercadoBitcoin.Location = new System.Drawing.Point(12, 85);
            this.grbMercadoBitcoin.Name = "grbMercadoBitcoin";
            this.grbMercadoBitcoin.Size = new System.Drawing.Size(486, 177);
            this.grbMercadoBitcoin.TabIndex = 0;
            this.grbMercadoBitcoin.TabStop = false;
            this.grbMercadoBitcoin.Text = "Corretora";
            // 
            // cmbCorretora
            // 
            this.cmbCorretora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCorretora.FormattingEnabled = true;
            this.cmbCorretora.Items.AddRange(new object[] {
            "1 - Mercado Bitcoin"});
            this.cmbCorretora.Location = new System.Drawing.Point(334, 25);
            this.cmbCorretora.Name = "cmbCorretora";
            this.cmbCorretora.Size = new System.Drawing.Size(137, 26);
            this.cmbCorretora.TabIndex = 4;
            // 
            // lblCorretora
            // 
            this.lblCorretora.AutoSize = true;
            this.lblCorretora.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorretora.Location = new System.Drawing.Point(20, 34);
            this.lblCorretora.Name = "lblCorretora";
            this.lblCorretora.Size = new System.Drawing.Size(60, 14);
            this.lblCorretora.TabIndex = 10;
            this.lblCorretora.Text = "Corretora:";
            // 
            // txtSegredoTapi
            // 
            this.txtSegredoTapi.Location = new System.Drawing.Point(334, 144);
            this.txtSegredoTapi.Name = "txtSegredoTapi";
            this.txtSegredoTapi.Size = new System.Drawing.Size(137, 25);
            this.txtSegredoTapi.TabIndex = 8;
            this.txtSegredoTapi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSegredoTapi
            // 
            this.lblSegredoTapi.AutoSize = true;
            this.lblSegredoTapi.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSegredoTapi.Location = new System.Drawing.Point(20, 150);
            this.lblSegredoTapi.Name = "lblSegredoTapi";
            this.lblSegredoTapi.Size = new System.Drawing.Size(79, 14);
            this.lblSegredoTapi.TabIndex = 8;
            this.lblSegredoTapi.Text = "Segredo TAPI:";
            // 
            // txtTapi
            // 
            this.txtTapi.Location = new System.Drawing.Point(334, 114);
            this.txtTapi.Name = "txtTapi";
            this.txtTapi.Size = new System.Drawing.Size(137, 25);
            this.txtTapi.TabIndex = 7;
            this.txtTapi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTapi
            // 
            this.lblTapi.AutoSize = true;
            this.lblTapi.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTapi.Location = new System.Drawing.Point(20, 123);
            this.lblTapi.Name = "lblTapi";
            this.lblTapi.Size = new System.Drawing.Size(47, 14);
            this.lblTapi.TabIndex = 4;
            this.lblTapi.Text = "TAPI ID:";
            // 
            // txtTaxaVenda
            // 
            this.txtTaxaVenda.Location = new System.Drawing.Point(334, 85);
            this.txtTaxaVenda.Name = "txtTaxaVenda";
            this.txtTaxaVenda.Size = new System.Drawing.Size(137, 25);
            this.txtTaxaVenda.TabIndex = 6;
            this.txtTaxaVenda.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTaxaCompra
            // 
            this.txtTaxaCompra.Location = new System.Drawing.Point(334, 56);
            this.txtTaxaCompra.Name = "txtTaxaCompra";
            this.txtTaxaCompra.Size = new System.Drawing.Size(137, 25);
            this.txtTaxaCompra.TabIndex = 5;
            this.txtTaxaCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTaxaVenda
            // 
            this.lblTaxaVenda.AutoSize = true;
            this.lblTaxaVenda.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxaVenda.Location = new System.Drawing.Point(20, 95);
            this.lblTaxaVenda.Name = "lblTaxaVenda";
            this.lblTaxaVenda.Size = new System.Drawing.Size(144, 14);
            this.lblTaxaVenda.TabIndex = 1;
            this.lblTaxaVenda.Text = "% de comissão na venda:";
            // 
            // lblTaxaCompra
            // 
            this.lblTaxaCompra.AutoSize = true;
            this.lblTaxaCompra.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaxaCompra.Location = new System.Drawing.Point(20, 65);
            this.lblTaxaCompra.Name = "lblTaxaCompra";
            this.lblTaxaCompra.Size = new System.Drawing.Size(151, 14);
            this.lblTaxaCompra.TabIndex = 0;
            this.lblTaxaCompra.Text = "% de comissão na compra:";
            // 
            // grbEstatisticas
            // 
            this.grbEstatisticas.Controls.Add(this.txtQtdRegistros);
            this.grbEstatisticas.Controls.Add(this.lblQtdRegistros);
            this.grbEstatisticas.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbEstatisticas.Location = new System.Drawing.Point(12, 273);
            this.grbEstatisticas.Name = "grbEstatisticas";
            this.grbEstatisticas.Size = new System.Drawing.Size(486, 72);
            this.grbEstatisticas.TabIndex = 1;
            this.grbEstatisticas.TabStop = false;
            this.grbEstatisticas.Text = "Estatísticas";
            // 
            // txtQtdRegistros
            // 
            this.txtQtdRegistros.Location = new System.Drawing.Point(334, 23);
            this.txtQtdRegistros.Name = "txtQtdRegistros";
            this.txtQtdRegistros.Size = new System.Drawing.Size(137, 25);
            this.txtQtdRegistros.TabIndex = 9;
            this.txtQtdRegistros.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblQtdRegistros
            // 
            this.lblQtdRegistros.AutoSize = true;
            this.lblQtdRegistros.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQtdRegistros.Location = new System.Drawing.Point(20, 34);
            this.lblQtdRegistros.Name = "lblQtdRegistros";
            this.lblQtdRegistros.Size = new System.Drawing.Size(181, 14);
            this.lblQtdRegistros.TabIndex = 0;
            this.lblQtdRegistros.Text = "Analisar as últimas X consultas:";
            // 
            // grbNegociacao
            // 
            this.grbNegociacao.Controls.Add(this.txtDifMaxMin);
            this.grbNegociacao.Controls.Add(this.lblDifMaxMin);
            this.grbNegociacao.Controls.Add(this.txtDistanciaMax);
            this.grbNegociacao.Controls.Add(this.lblDistanciaMax);
            this.grbNegociacao.Controls.Add(this.txtDifCompraVenda);
            this.grbNegociacao.Controls.Add(this.txtPctDisponivelCompra);
            this.grbNegociacao.Controls.Add(this.lblDifCompraVenda);
            this.grbNegociacao.Controls.Add(this.lblPctDisponivelCompra);
            this.grbNegociacao.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbNegociacao.Location = new System.Drawing.Point(12, 351);
            this.grbNegociacao.Name = "grbNegociacao";
            this.grbNegociacao.Size = new System.Drawing.Size(486, 156);
            this.grbNegociacao.TabIndex = 3;
            this.grbNegociacao.TabStop = false;
            this.grbNegociacao.Text = "Negociação:";
            // 
            // txtDifMaxMin
            // 
            this.txtDifMaxMin.Location = new System.Drawing.Point(334, 118);
            this.txtDifMaxMin.Name = "txtDifMaxMin";
            this.txtDifMaxMin.Size = new System.Drawing.Size(137, 25);
            this.txtDifMaxMin.TabIndex = 13;
            this.txtDifMaxMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDifMaxMin
            // 
            this.lblDifMaxMin.AutoSize = true;
            this.lblDifMaxMin.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifMaxMin.Location = new System.Drawing.Point(20, 127);
            this.lblDifMaxMin.Name = "lblDifMaxMin";
            this.lblDifMaxMin.Size = new System.Drawing.Size(310, 14);
            this.lblDifMaxMin.TabIndex = 9;
            this.lblDifMaxMin.Text = "Diferença mínima entre Máximo e Mínimo para compra:";
            // 
            // txtDistanciaMax
            // 
            this.txtDistanciaMax.Location = new System.Drawing.Point(334, 86);
            this.txtDistanciaMax.Name = "txtDistanciaMax";
            this.txtDistanciaMax.Size = new System.Drawing.Size(137, 25);
            this.txtDistanciaMax.TabIndex = 12;
            this.txtDistanciaMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDistanciaMax
            // 
            this.lblDistanciaMax.AutoSize = true;
            this.lblDistanciaMax.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistanciaMax.Location = new System.Drawing.Point(20, 96);
            this.lblDistanciaMax.Name = "lblDistanciaMax";
            this.lblDistanciaMax.Size = new System.Drawing.Size(277, 14);
            this.lblDistanciaMax.TabIndex = 5;
            this.lblDistanciaMax.Text = "Comprar quando estiver abaixo de X% do Máximo:";
            // 
            // txtDifCompraVenda
            // 
            this.txtDifCompraVenda.Location = new System.Drawing.Point(334, 54);
            this.txtDifCompraVenda.Name = "txtDifCompraVenda";
            this.txtDifCompraVenda.Size = new System.Drawing.Size(137, 25);
            this.txtDifCompraVenda.TabIndex = 11;
            this.txtDifCompraVenda.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPctDisponivelCompra
            // 
            this.txtPctDisponivelCompra.Location = new System.Drawing.Point(334, 24);
            this.txtPctDisponivelCompra.Name = "txtPctDisponivelCompra";
            this.txtPctDisponivelCompra.Size = new System.Drawing.Size(137, 25);
            this.txtPctDisponivelCompra.TabIndex = 10;
            this.txtPctDisponivelCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblDifCompraVenda
            // 
            this.lblDifCompraVenda.AutoSize = true;
            this.lblDifCompraVenda.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifCompraVenda.Location = new System.Drawing.Point(20, 64);
            this.lblDifCompraVenda.Name = "lblDifCompraVenda";
            this.lblDifCompraVenda.Size = new System.Drawing.Size(305, 14);
            this.lblDifCompraVenda.TabIndex = 3;
            this.lblDifCompraVenda.Text = "Comprar quando diferença Compra e Venda maior que:";
            // 
            // lblPctDisponivelCompra
            // 
            this.lblPctDisponivelCompra.AutoSize = true;
            this.lblPctDisponivelCompra.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPctDisponivelCompra.Location = new System.Drawing.Point(20, 35);
            this.lblPctDisponivelCompra.Name = "lblPctDisponivelCompra";
            this.lblPctDisponivelCompra.Size = new System.Drawing.Size(214, 14);
            this.lblPctDisponivelCompra.TabIndex = 3;
            this.lblPctDisponivelCompra.Text = "% da carteira permitido para compras:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(219, 528);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkNegociacaoAtiva);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 66);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sistema:";
            // 
            // chkNegociacaoAtiva
            // 
            this.chkNegociacaoAtiva.AutoSize = true;
            this.chkNegociacaoAtiva.Font = new System.Drawing.Font("Calibri", 9F);
            this.chkNegociacaoAtiva.Location = new System.Drawing.Point(22, 31);
            this.chkNegociacaoAtiva.Name = "chkNegociacaoAtiva";
            this.chkNegociacaoAtiva.Size = new System.Drawing.Size(313, 18);
            this.chkNegociacaoAtiva.TabIndex = 3;
            this.chkNegociacaoAtiva.Text = "Ativar a negociação sempre que o sistema for aberto";
            this.chkNegociacaoAtiva.UseVisualStyleBackColor = true;
            // 
            // frmViwParametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 573);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grbNegociacao);
            this.Controls.Add(this.grbEstatisticas);
            this.Controls.Add(this.grbMercadoBitcoin);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmViwParametros";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parâmetros Gerais do Sistema";
            this.Load += new System.EventHandler(this.frmViwParametros_Load);
            this.grbMercadoBitcoin.ResumeLayout(false);
            this.grbMercadoBitcoin.PerformLayout();
            this.grbEstatisticas.ResumeLayout(false);
            this.grbEstatisticas.PerformLayout();
            this.grbNegociacao.ResumeLayout(false);
            this.grbNegociacao.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbMercadoBitcoin;
        private System.Windows.Forms.TextBox txtTaxaVenda;
        private System.Windows.Forms.TextBox txtTaxaCompra;
        private System.Windows.Forms.Label lblTaxaVenda;
        private System.Windows.Forms.Label lblTaxaCompra;
        private System.Windows.Forms.GroupBox grbEstatisticas;
        private System.Windows.Forms.TextBox txtQtdRegistros;
        private System.Windows.Forms.Label lblQtdRegistros;
        private System.Windows.Forms.GroupBox grbNegociacao;
        private System.Windows.Forms.TextBox txtPctDisponivelCompra;
        private System.Windows.Forms.Label lblPctDisponivelCompra;
        private System.Windows.Forms.TextBox txtTapi;
        private System.Windows.Forms.Label lblTapi;
        private System.Windows.Forms.TextBox txtSegredoTapi;
        private System.Windows.Forms.Label lblSegredoTapi;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtDifCompraVenda;
        private System.Windows.Forms.Label lblDifCompraVenda;
        private System.Windows.Forms.TextBox txtDistanciaMax;
        private System.Windows.Forms.Label lblDistanciaMax;
        private System.Windows.Forms.ComboBox cmbCorretora;
        private System.Windows.Forms.Label lblCorretora;
        private System.Windows.Forms.TextBox txtDifMaxMin;
        private System.Windows.Forms.Label lblDifMaxMin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkNegociacaoAtiva;
    }
}