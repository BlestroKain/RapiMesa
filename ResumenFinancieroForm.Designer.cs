namespace Rapimesa
{
    partial class ResumenFinancieroForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblPeriodo;
        private System.Windows.Forms.ComboBox cmbPeriodo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Button btnVerResumen;
        private System.Windows.Forms.DataGridView dgvResumen;
        private System.Windows.Forms.Label lblVentasTotales;
        private System.Windows.Forms.Label lblEgresosTotales;
        private System.Windows.Forms.Label lblUtilidadBruta;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPeriodo = new System.Windows.Forms.Label();
            this.cmbPeriodo = new System.Windows.Forms.ComboBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.btnVerResumen = new System.Windows.Forms.Button();
            this.dgvResumen = new System.Windows.Forms.DataGridView();
            this.lblVentasTotales = new System.Windows.Forms.Label();
            this.lblEgresosTotales = new System.Windows.Forms.Label();
            this.lblUtilidadBruta = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumen)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPeriodo
            // 
            this.lblPeriodo.AutoSize = true;
            this.lblPeriodo.Location = new System.Drawing.Point(20, 20);
            this.lblPeriodo.Name = "lblPeriodo";
            this.lblPeriodo.Size = new System.Drawing.Size(52, 13);
            this.lblPeriodo.Text = "Resumen:";
            // 
            // cmbPeriodo
            // 
            this.cmbPeriodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriodo.Items.AddRange(new object[] {
            "Diario",
            "Mensual"});
            this.cmbPeriodo.Location = new System.Drawing.Point(80, 17);
            this.cmbPeriodo.Name = "cmbPeriodo";
            this.cmbPeriodo.Size = new System.Drawing.Size(100, 21);
            // 
            // dtpFecha
            // 
            this.dtpFecha.Location = new System.Drawing.Point(200, 17);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(200, 20);
            // 
            // btnVerResumen
            // 
            this.btnVerResumen.Location = new System.Drawing.Point(420, 15);
            this.btnVerResumen.Name = "btnVerResumen";
            this.btnVerResumen.Size = new System.Drawing.Size(100, 23);
            this.btnVerResumen.Text = "Ver Resumen";
            this.btnVerResumen.Click += new System.EventHandler(this.btnVerResumen_Click);
            // 
            // dgvResumen
            // 
            this.dgvResumen.AllowUserToAddRows = false;
            this.dgvResumen.AllowUserToDeleteRows = false;
            this.dgvResumen.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvResumen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResumen.Location = new System.Drawing.Point(20, 60);
            this.dgvResumen.Name = "dgvResumen";
            this.dgvResumen.ReadOnly = true;
            this.dgvResumen.Size = new System.Drawing.Size(600, 300);
            // 
            // lblVentasTotales
            // 
            this.lblVentasTotales.AutoSize = true;
            this.lblVentasTotales.Location = new System.Drawing.Point(20, 375);
            this.lblVentasTotales.Size = new System.Drawing.Size(90, 13);
            this.lblVentasTotales.Text = "Ventas Totales: $0";
            // 
            // lblEgresosTotales
            // 
            this.lblEgresosTotales.AutoSize = true;
            this.lblEgresosTotales.Location = new System.Drawing.Point(20, 400);
            this.lblEgresosTotales.Size = new System.Drawing.Size(101, 13);
            this.lblEgresosTotales.Text = "Egresos (Compras): $0";
            // 
            // lblUtilidadBruta
            // 
            this.lblUtilidadBruta.AutoSize = true;
            this.lblUtilidadBruta.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblUtilidadBruta.Location = new System.Drawing.Point(20, 425);
            this.lblUtilidadBruta.Size = new System.Drawing.Size(122, 15);
            this.lblUtilidadBruta.Text = "Utilidad Bruta: $0";
            // 
            // ResumenFinancieroForm
            // 
            this.ClientSize = new System.Drawing.Size(650, 470);
            this.Controls.Add(this.lblUtilidadBruta);
            this.Controls.Add(this.lblEgresosTotales);
            this.Controls.Add(this.lblVentasTotales);
            this.Controls.Add(this.dgvResumen);
            this.Controls.Add(this.btnVerResumen);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.cmbPeriodo);
            this.Controls.Add(this.lblPeriodo);
            this.Text = "Resumen Financiero";
            ((System.ComponentModel.ISupportInitialize)(this.dgvResumen)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
