using OfficeOpenXml;
using System;
using System.IO;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class IngresoProductoForm : Form
    {
        private string path = "inventario.xlsx";

        public IngresoProductoForm(string path)
        {
            InitializeComponent();
            this.path = path;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var cantidad = (int)numCantidad.Value;
            var precio = (double)numPrecio.Value;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("Nombre del producto es requerido.");
                return;
            }

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets["Inventario"];

                int nuevaFila = ws.Dimension?.End.Row + 1 ?? 2;
                int nuevoId = 1;

                // Buscar último ID si existe
                if (nuevaFila > 2)
                {
                    var ultimaId = int.Parse(ws.Cells[nuevaFila - 1, 1].Text);
                    nuevoId = ultimaId + 1;
                }

                ws.Cells[nuevaFila, 1].Value = nuevoId;
                ws.Cells[nuevaFila, 2].Value = nombre;
                ws.Cells[nuevaFila, 3].Value = cantidad;
                ws.Cells[nuevaFila, 4].Value = precio;

                package.Save();
            }

            MessageBox.Show("Producto registrado correctamente.");
            this.Close();
        }

        private void Cancelbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
