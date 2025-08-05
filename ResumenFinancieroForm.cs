using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class ResumenFinancieroForm : Form
    {
        private string archivoExcel = "inventario.xlsx";

        public ResumenFinancieroForm()
        {
            InitializeComponent();
            cmbPeriodo.Items.AddRange(new[] { "Diario", "Mensual" });
            cmbPeriodo.SelectedIndex = 0;
            dtpFecha.Value = DateTime.Today;
        }

        private void btnVerResumen_Click(object sender, EventArgs e)
        {
            if (!File.Exists(archivoExcel))
            {
                MessageBox.Show("Archivo de inventario no encontrado.");
                return;
            }

            var ventasTotales = 0.0;
            var egresosTotales = 0.0;

            var tabla = new DataTable();
            tabla.Columns.Add("Fecha");
            tabla.Columns.Add("Producto");
            tabla.Columns.Add("Cantidad");
            tabla.Columns.Add("PrecioUnitario");
            tabla.Columns.Add("Tipo");
            tabla.Columns.Add("Total");

            using (var package = new ExcelPackage(new FileInfo(archivoExcel)))
            {
                var ws = package.Workbook.Worksheets["Historia"];
                if (ws == null) return;

                int filas = ws.Dimension.End.Row;
                for (int i = 2; i <= filas; i++)
                {
                    DateTime fecha = ws.Cells[i, 1].GetValue<DateTime>();
                    string producto = ws.Cells[i, 3].Text;
                    int cantidad = ws.Cells[i, 4].GetValue<int>();
                    double precio = ws.Cells[i, 5].GetValue<double>();
                    string tipo = ws.Cells[i, 7].Text;

                    // Omitir tipos no válidos
                    if (tipo != "Ingreso" && tipo != "Venta")
                        continue;

                    if (cmbPeriodo.SelectedItem.ToString() == "Diario" && fecha.Date != dtpFecha.Value.Date)
                        continue;
                    if (cmbPeriodo.SelectedItem.ToString() == "Mensual" && (fecha.Year != dtpFecha.Value.Year || fecha.Month != dtpFecha.Value.Month))
                        continue;

                    double total = cantidad * precio;

                    if (tipo == "Ingreso")
                        egresosTotales += total;
                    else if (tipo == "Venta")
                        ventasTotales += total;

                    tabla.Rows.Add(fecha.ToShortDateString(), producto, cantidad, precio.ToString("C0"), tipo, total.ToString("C0"));
                }
            }


            dgvResumen.DataSource = tabla;

            lblVentasTotales.Text = $"Ventas Totales: {ventasTotales:C0}";
            lblEgresosTotales.Text = $"Egresos (Compras): {egresosTotales:C0}";
            lblUtilidadBruta.Text = $"Utilidad Bruta: {(ventasTotales - egresosTotales):C0}";
        }

    }
}
