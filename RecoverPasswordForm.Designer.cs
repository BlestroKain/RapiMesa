namespace Rapimesa
{
    partial class RecoverPasswordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblPregunta;
        private System.Windows.Forms.Panel pnlPregunta;
        private System.Windows.Forms.TextBox txtRespuesta;
        private System.Windows.Forms.Button btnVerificar;

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

        /// <summary>
        /// Required method for Designer support
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblPregunta = new System.Windows.Forms.Label();
            this.pnlPregunta = new System.Windows.Forms.Panel();
            this.txtRespuesta = new System.Windows.Forms.TextBox();
            this.btnVerificar = new System.Windows.Forms.Button();

            this.pnlPregunta.SuspendLayout();
            this.SuspendLayout();

            // txtUsuario
            this.txtUsuario.Location = new System.Drawing.Point(20, 20);
            this.txtUsuario.Width = 200;

            // btnBuscar
            this.btnBuscar.Location = new System.Drawing.Point(230, 18);
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.Width = 100;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);

            // lblPregunta
            this.lblPregunta.Location = new System.Drawing.Point(10, 10);
            this.lblPregunta.Width = 300;

            // txtRespuesta
            this.txtRespuesta.Location = new System.Drawing.Point(10, 35);
            this.txtRespuesta.Width = 300;

            // btnVerificar
            this.btnVerificar.Location = new System.Drawing.Point(10, 70);
            this.btnVerificar.Text = "Verificar";
            this.btnVerificar.Width = 100;
            this.btnVerificar.Click += new System.EventHandler(this.btnVerificar_Click);

            // pnlPregunta
            this.pnlPregunta.Location = new System.Drawing.Point(20, 60);
            this.pnlPregunta.Size = new System.Drawing.Size(350, 110);
            this.pnlPregunta.Visible = false;
            this.pnlPregunta.Controls.Add(this.lblPregunta);
            this.pnlPregunta.Controls.Add(this.txtRespuesta);
            this.pnlPregunta.Controls.Add(this.btnVerificar);

            // RecoverPasswordForm
            this.ClientSize = new System.Drawing.Size(400, 190);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.pnlPregunta);
            this.Text = "Recuperar Contraseña";
            this.pnlPregunta.ResumeLayout(false);
            this.pnlPregunta.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
