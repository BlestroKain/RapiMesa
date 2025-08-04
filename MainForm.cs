using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class MainForm : Form
    {
        private string path = "inventario.xlsx";
        private DataTable _dtInventario;
        private User _usuarioActual;

        public MainForm(User usuario)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _usuarioActual = usuario;

            InitializeComponent();
            Text = $"RapyMesa - Sesión de {_usuarioActual.NombreCompleto} ({_usuarioActual.Rol})";

            if (_usuarioActual.Rol == "Supervisor")
            {
                btnRegistrarUsuario.Visible = true;
                btnAgregarProducto.Visible = true;

                btnRegistrarUsuario.Click += (s, e) =>
                {
                    var formRegistro = new RegisterUserForm();
                    formRegistro.ShowDialog();
                };

                btnAgregarProducto.Click += (s, e) =>
                {
                    var ingresoForm = new IngresoProductoForm(path);
                    ingresoForm.FormClosed += (s2, e2) => LoadInventory();
                    ingresoForm.ShowDialog();
                };
            }
            else
            {
                btnRegistrarUsuario.Visible = false;
                btnAgregarProducto.Visible = false;
            }

            EnsureInventoryExists();
            LoadInventory();
        }

        private void EnsureInventoryExists()
        {
            if (!File.Exists(path))
            {
                using (var package = new ExcelPackage())
                {
                    var ws = package.Workbook.Worksheets.Add("Inventario");
                    ws.Cells[1, 1].Value = "ID";
                    ws.Cells[1, 2].Value = "Producto";
                    ws.Cells[1, 3].Value = "Stock";
                    ws.Cells[1, 4].Value = "PrecioUnidad";
                    package.Workbook.Worksheets.Add("Historia");
                    package.SaveAs(new FileInfo(path));
                }
            }
        }

        private void LoadInventory()
        {
            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets["Inventario"];
                _dtInventario = new DataTable();

                for (int col = 1; ws.Cells[1, col].Value != null; col++)
                {
                    _dtInventario.Columns.Add(ws.Cells[1, col].Text);
                }

                int row = 2;
                while (ws.Cells[row, 1].Value != null)
                {
                    var dataRow = _dtInventario.NewRow();
                    for (int col = 1; col <= _dtInventario.Columns.Count; col++)
                    {
                        dataRow[col - 1] = ws.Cells[row, col].Text;
                    }
                    _dtInventario.Rows.Add(dataRow);
                    row++;
                }

                dataGridView1.DataSource = _dtInventario;
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filtro))
            {
                dataGridView1.DataSource = _dtInventario;
            }
            else
            {
                var resultados = _dtInventario.AsEnumerable()
                    .Where(r => r.Field<string>("Producto").ToLower().Contains(filtro));
                dataGridView1.DataSource = resultados.Any() ? resultados.CopyToDataTable() : null;
            }
        }

        private void btnEgresar_Click(object sender, EventArgs e)
        {
            ActualizarStock(ingreso: false);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            ActualizarStock(ingreso: true);
        }

        private void ActualizarStock(bool ingreso)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            int cantidad = (int)numCantidad.Value;
            if (!ingreso) cantidad *= -1;

            var fila = dataGridView1.SelectedRows[0];
            int id = int.Parse(fila.Cells["ID"].Value.ToString());

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var ws = package.Workbook.Worksheets["Inventario"];

                int filaExcel = 2;
                while (ws.Cells[filaExcel, 1].Value != null)
                {
                    if (int.Parse(ws.Cells[filaExcel, 1].Text) == id)
                    {
                        int stockActual = int.Parse(ws.Cells[filaExcel, 3].Text);
                        int nuevoStock = stockActual + cantidad;

                        if (nuevoStock < 0)
                        {
                            MessageBox.Show("No hay suficiente stock.");
                            return;
                        }

                        ws.Cells[filaExcel, 3].Value = nuevoStock;

                        string producto = ws.Cells[filaExcel, 2].Text;
                        double precio = double.Parse(ws.Cells[filaExcel, 4].Text);

                        RegistrarHistorial(package, producto, cantidad, precio, ingreso ? "Ingreso" : "Egreso");

                        package.Save();

                        break;
                    }
                    filaExcel++;
                }
            }

            LoadInventory();
            MessageBox.Show($"{(ingreso ? "Ingreso" : "Egreso")} registrado.");
        }

        private void RegistrarHistorial(ExcelPackage package, string producto, int cantidad, double precio, string tipo)
        {
            var ws = package.Workbook.Worksheets["Historia"];
            if (ws.Dimension == null)
            {
                ws.Cells[1, 1].Value = "Fecha";
                ws.Cells[1, 2].Value = "Usuario";
                ws.Cells[1, 3].Value = "Producto";
                ws.Cells[1, 4].Value = "Cantidad";
                ws.Cells[1, 5].Value = "PrecioUnidad";
                ws.Cells[1, 6].Value = "Total";
                ws.Cells[1, 7].Value = "Movimiento";
            }

            int row = ws.Dimension.End.Row + 1;

            ws.Cells[row, 1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ws.Cells[row, 2].Value = _usuarioActual.Username;
            ws.Cells[row, 3].Value = producto;
            ws.Cells[row, 4].Value = cantidad;
            ws.Cells[row, 5].Value = precio;
            ws.Cells[row, 6].Value = Math.Abs(precio * cantidad);
            ws.Cells[row, 7].Value = tipo;
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var fila = dataGridView1.SelectedRows[0];
            double precio = double.Parse(fila.Cells["PrecioUnidad"].Value.ToString());
            int cantidad = (int)numCantidad.Value;
            lblPrecio.Text = $"Precio: ${precio}";
            lblTotal.Text = $"Total: ${precio * cantidad}";
        }

        private void numCantidad_ValueChanged(object sender, EventArgs e)
        {
            dataGridView1_SelectionChanged(sender, e);
        }
        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            var formRegistro = new RegisterUserForm();
            formRegistro.ShowDialog();
        }

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            var ingresoForm = new IngresoProductoForm(path);
            ingresoForm.FormClosed += (s2, e2) => LoadInventory(); // Refresca al cerrar
            ingresoForm.ShowDialog();
        }

    }
}
