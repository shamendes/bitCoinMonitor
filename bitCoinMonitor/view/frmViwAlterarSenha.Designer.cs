namespace bitCoinMonitor.view
{
    partial class frmViwAlterarSenha
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtConfirmaNovaSenha = new System.Windows.Forms.TextBox();
            this.lblConfirmaNovaSenha = new System.Windows.Forms.Label();
            this.txtNovaSenha = new System.Windows.Forms.TextBox();
            this.lblNovaSenha = new System.Windows.Forms.Label();
            this.txtSenhaAtual = new System.Windows.Forms.TextBox();
            this.lblSenhaAtual = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(81, 146);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(193, 146);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 5;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtConfirmaNovaSenha);
            this.groupBox1.Controls.Add(this.lblConfirmaNovaSenha);
            this.groupBox1.Controls.Add(this.txtNovaSenha);
            this.groupBox1.Controls.Add(this.lblNovaSenha);
            this.groupBox1.Controls.Add(this.txtSenhaAtual);
            this.groupBox1.Controls.Add(this.lblSenhaAtual);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(328, 116);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informe os dados abaixo:";
            // 
            // txtConfirmaNovaSenha
            // 
            this.txtConfirmaNovaSenha.Font = new System.Drawing.Font("Calibri", 9F);
            this.txtConfirmaNovaSenha.Location = new System.Drawing.Point(152, 78);
            this.txtConfirmaNovaSenha.MaxLength = 20;
            this.txtConfirmaNovaSenha.Name = "txtConfirmaNovaSenha";
            this.txtConfirmaNovaSenha.PasswordChar = '*';
            this.txtConfirmaNovaSenha.Size = new System.Drawing.Size(165, 22);
            this.txtConfirmaNovaSenha.TabIndex = 3;
            this.txtConfirmaNovaSenha.UseSystemPasswordChar = true;
            // 
            // lblConfirmaNovaSenha
            // 
            this.lblConfirmaNovaSenha.AutoSize = true;
            this.lblConfirmaNovaSenha.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmaNovaSenha.Location = new System.Drawing.Point(15, 81);
            this.lblConfirmaNovaSenha.Name = "lblConfirmaNovaSenha";
            this.lblConfirmaNovaSenha.Size = new System.Drawing.Size(124, 14);
            this.lblConfirmaNovaSenha.TabIndex = 25;
            this.lblConfirmaNovaSenha.Text = "Confirma Nova Senha:";
            // 
            // txtNovaSenha
            // 
            this.txtNovaSenha.Font = new System.Drawing.Font("Calibri", 9F);
            this.txtNovaSenha.Location = new System.Drawing.Point(152, 51);
            this.txtNovaSenha.MaxLength = 20;
            this.txtNovaSenha.Name = "txtNovaSenha";
            this.txtNovaSenha.PasswordChar = '*';
            this.txtNovaSenha.Size = new System.Drawing.Size(165, 22);
            this.txtNovaSenha.TabIndex = 2;
            this.txtNovaSenha.UseSystemPasswordChar = true;
            // 
            // lblNovaSenha
            // 
            this.lblNovaSenha.AutoSize = true;
            this.lblNovaSenha.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNovaSenha.Location = new System.Drawing.Point(15, 54);
            this.lblNovaSenha.Name = "lblNovaSenha";
            this.lblNovaSenha.Size = new System.Drawing.Size(74, 14);
            this.lblNovaSenha.TabIndex = 23;
            this.lblNovaSenha.Text = "Nova Senha:";
            // 
            // txtSenhaAtual
            // 
            this.txtSenhaAtual.Font = new System.Drawing.Font("Calibri", 9F);
            this.txtSenhaAtual.Location = new System.Drawing.Point(152, 25);
            this.txtSenhaAtual.MaxLength = 20;
            this.txtSenhaAtual.Name = "txtSenhaAtual";
            this.txtSenhaAtual.PasswordChar = '*';
            this.txtSenhaAtual.Size = new System.Drawing.Size(165, 22);
            this.txtSenhaAtual.TabIndex = 1;
            this.txtSenhaAtual.UseSystemPasswordChar = true;
            // 
            // lblSenhaAtual
            // 
            this.lblSenhaAtual.AutoSize = true;
            this.lblSenhaAtual.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSenhaAtual.Location = new System.Drawing.Point(15, 29);
            this.lblSenhaAtual.Name = "lblSenhaAtual";
            this.lblSenhaAtual.Size = new System.Drawing.Size(76, 14);
            this.lblSenhaAtual.TabIndex = 22;
            this.lblSenhaAtual.Text = "Senha Atual:";
            // 
            // frmViwAlterarSenha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 181);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnOk);
            this.Name = "frmViwAlterarSenha";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alteração de Senha";
            this.Load += new System.EventHandler(this.frmViwAlterarSenha_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtConfirmaNovaSenha;
        private System.Windows.Forms.Label lblConfirmaNovaSenha;
        private System.Windows.Forms.TextBox txtNovaSenha;
        private System.Windows.Forms.Label lblNovaSenha;
        private System.Windows.Forms.TextBox txtSenhaAtual;
        private System.Windows.Forms.Label lblSenhaAtual;
    }
}