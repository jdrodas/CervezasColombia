namespace CervezasColombia_NoSQL_WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            cbxNombreCervecerias = new ComboBox();
            label2 = new Label();
            groupBox1 = new GroupBox();
            txtUbicacionCerveceria = new TextBox();
            label6 = new Label();
            txtInstagramCerveceria = new TextBox();
            label5 = new Label();
            txtSitioWebCerveceria = new TextBox();
            label4 = new Label();
            txtNombreCerveceria = new TextBox();
            label3 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Comic Sans MS", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Purple;
            label1.Location = new Point(142, 23);
            label1.Name = "label1";
            label1.Size = new Size(519, 41);
            label1.TabIndex = 0;
            label1.Text = "Cervezas Artesanales de Colombia";
            // 
            // cbxNombreCervecerias
            // 
            cbxNombreCervecerias.FormattingEnabled = true;
            cbxNombreCervecerias.Location = new Point(237, 105);
            cbxNombreCervecerias.Name = "cbxNombreCervecerias";
            cbxNombreCervecerias.Size = new Size(358, 28);
            cbxNombreCervecerias.TabIndex = 1;
            cbxNombreCervecerias.SelectedIndexChanged += cbxNombreCervecerias_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(144, 108);
            label2.Name = "label2";
            label2.Size = new Size(87, 20);
            label2.TabIndex = 2;
            label2.Text = "Cervecerías:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtUbicacionCerveceria);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(txtInstagramCerveceria);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtSitioWebCerveceria);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtNombreCerveceria);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(115, 190);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(546, 224);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Información de la cervecería seleccionada:";
            // 
            // txtUbicacionCerveceria
            // 
            txtUbicacionCerveceria.BackColor = Color.White;
            txtUbicacionCerveceria.Enabled = false;
            txtUbicacionCerveceria.Location = new Point(172, 167);
            txtUbicacionCerveceria.Name = "txtUbicacionCerveceria";
            txtUbicacionCerveceria.Size = new Size(326, 27);
            txtUbicacionCerveceria.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(78, 170);
            label6.Name = "label6";
            label6.Size = new Size(78, 20);
            label6.TabIndex = 6;
            label6.Text = "Ubicación:";
            // 
            // txtInstagramCerveceria
            // 
            txtInstagramCerveceria.BackColor = Color.White;
            txtInstagramCerveceria.Enabled = false;
            txtInstagramCerveceria.Location = new Point(172, 127);
            txtInstagramCerveceria.Name = "txtInstagramCerveceria";
            txtInstagramCerveceria.Size = new Size(326, 27);
            txtInstagramCerveceria.TabIndex = 5;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(78, 130);
            label5.Name = "label5";
            label5.Size = new Size(78, 20);
            label5.TabIndex = 4;
            label5.Text = "Instagram:";
            // 
            // txtSitioWebCerveceria
            // 
            txtSitioWebCerveceria.BackColor = Color.White;
            txtSitioWebCerveceria.Enabled = false;
            txtSitioWebCerveceria.Location = new Point(172, 88);
            txtSitioWebCerveceria.Name = "txtSitioWebCerveceria";
            txtSitioWebCerveceria.Size = new Size(326, 27);
            txtSitioWebCerveceria.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(87, 91);
            label4.Name = "label4";
            label4.Size = new Size(69, 20);
            label4.TabIndex = 2;
            label4.Text = "SitioWeb";
            // 
            // txtNombreCerveceria
            // 
            txtNombreCerveceria.BackColor = Color.White;
            txtNombreCerveceria.Enabled = false;
            txtNombreCerveceria.Location = new Point(172, 44);
            txtNombreCerveceria.Name = "txtNombreCerveceria";
            txtNombreCerveceria.Size = new Size(326, 27);
            txtNombreCerveceria.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(89, 47);
            label3.Name = "label3";
            label3.Size = new Size(67, 20);
            label3.TabIndex = 0;
            label3.Text = "Nombre:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Controls.Add(label2);
            Controls.Add(cbxNombreCervecerias);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Cervezas Artesanales de Colombia";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private ComboBox cbxNombreCervecerias;
        private Label label2;
        private GroupBox groupBox1;
        private TextBox txtUbicacionCerveceria;
        private Label label6;
        private TextBox txtInstagramCerveceria;
        private Label label5;
        private TextBox txtSitioWebCerveceria;
        private Label label4;
        private TextBox txtNombreCerveceria;
        private Label label3;
    }
}