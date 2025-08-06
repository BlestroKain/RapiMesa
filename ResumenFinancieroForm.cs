using Rapimesa.Data;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class ResumenFinancieroForm : Form
    {
        public ResumenFinancieroForm()
        {
            InitializeComponent();
            cmbPeriodo.Items.AddRange(new[] { "Diario", "Mensual" });
            cmbPeriodo.SelectedIndex = 0;
            dtpFecha.Value = DateTime.Today;
        }

        private void btnVerResumen_Click(object sender, EventArgs e)
        {
            var historial = HistoryManager.GetAll();

            var ventasTotales = 0.0;
            var egresosTotales = 0.0;

            var tabla = new DataTable();
            tabla.Columns.Add("Fecha");
            tabla.Columns.Add("Producto");
            tabla.Columns.Add("Cantidad");
            tabla.Columns.Add("PrecioUnitario");
            tabla.Columns.Add("Tipo");
            tabla.Columns.Add("Total");

            foreach (var entry in historial)
            {
                DateTime fecha = entry.Fecha; // Ya es DateTime, no string

                if (cmbPeriodo.SelectedItem.ToString() == "Diario" && fecha.Date != dtpFecha.Value.Date)
                    continue;

                if (cmbPeriodo.SelectedItem.ToString() == "Mensual" &&
                    (fecha.Year != dtpFecha.Value.Year || fecha.Month != dtpFecha.Value.Month))
                    continue;

                double total = entry.PrecioUnidad * entry.Cantidad;

                if (entry.Movimiento == "Ingreso")
                    egresosTotales += total;
                else if (entry.Movimiento == "Venta")
                    ventasTotales += total;
                else
                    continue; // Omitir tipo desconocido

                tabla.Rows.Add(
                    fecha.ToShortDateString(),
                    entry.Producto,
                    entry.Cantidad,
                    entry.PrecioUnidad.ToString("C0"),
                    entry.Movimiento,
                    total.ToString("C0")
                );
            }

            dgvResumen.DataSource = tabla;

            lblVentasTotales.Text = $"Ventas Totales: {ventasTotales:C0}";
            lblEgresosTotales.Text = $"Egresos (Compras): {egresosTotales:C0}";
            lblUtilidadBruta.Text = $"Utilidad Bruta: {(ventasTotales - egresosTotales):C0}";
        }
    }
}
