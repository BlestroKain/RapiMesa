using Rapimesa.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class MainForm : Form
    {
        private List<Product> _productos;
        private User _usuarioActual;

        public MainForm(User usuario)
        {
            _usuarioActual = usuario;
            InitializeComponent();
            Text = $"RapyMesa - Sesión de {_usuarioActual.NombreCompleto} ({_usuarioActual.Rol})";

            btnAgregarProducto.Visible = _usuarioActual.Rol == "Supervisor";
            btnGestionUsuarios.Visible = _usuarioActual.Rol == "Supervisor";
            btnResumenFinanciero.Visible = _usuarioActual.Rol == "Supervisor";

            LoadInventoryFromDb();
        }

        private void LoadInventoryFromDb()
        {
            _productos = ProductManager.GetAll();
            dataGridView1.DataSource = _productos.Select(p => new
            {
                p.Id,
                Producto = p.Nombre,
                Stock = p.Stock,
                PrecioUnidad = p.PrecioUnidad
            }).ToList();

            ResaltarStockBajo();
        }

        private void ResaltarStockBajo()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (int.TryParse(row.Cells["Stock"].Value?.ToString(), out int stock) && stock < 5)
                {
                    row.DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();

            var filtrados = _productos
                .Where(p => p.Nombre.ToLower().Contains(filtro))
                .Select(p => new
                {
                    p.Id,
                    Producto = p.Nombre,
                    Stock = p.Stock,
                    PrecioUnidad = p.PrecioUnidad
                }).ToList();

            dataGridView1.DataSource = filtrados;
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
            int id = Convert.ToInt32(fila.Cells["Id"].Value);
            var producto = ProductManager.GetById(id);

            if (producto == null) return;

            int nuevoStock = producto.Stock + cantidad;

            if (nuevoStock < 0)
            {
                MessageBox.Show("No hay suficiente stock.");
                return;
            }

            ProductManager.UpdateStock(id, nuevoStock);

            var historial = new HistoryEntry
            {
                Fecha = DateTime.Now,
                Usuario = _usuarioActual.Username,
                Producto = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnidad = producto.PrecioUnidad,
                Total = Math.Abs(producto.PrecioUnidad * cantidad),
                Movimiento = ingreso ? "Ingreso" : "Venta"
            };

            HistoryManager.Insert(historial);

            LoadInventoryFromDb();
            MessageBox.Show($"{(ingreso ? "Ingreso" : "Venta")} registrado.");
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

        private void btnAgregarProducto_Click(object sender, EventArgs e)
        {
            var ingresoForm = new IngresoProductoForm(); // sin path
            ingresoForm.FormClosed += (s2, e2) => LoadInventoryFromDb();
            ingresoForm.ShowDialog();
        }

        private void btnGestionUsuarios_Click(object sender, EventArgs e)
        {
            var userMgmt = new UserManagementForm();
            userMgmt.ShowDialog();
        }

        private void btnResumenFinanciero_Click(object sender, EventArgs e)
        {
            new ResumenFinancieroForm().ShowDialog();
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "inventario.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportarInventarioACSV(sfd.FileName);
                }
            }
        }

        private void ExportarInventarioACSV(string rutaArchivo)
        {
            using (var sw = new StreamWriter(rutaArchivo))
            {
                sw.WriteLine("Id,Producto,Stock,PrecioUnidad");
                foreach (var p in _productos)
                {
                    sw.WriteLine($"{p.Id},{p.Nombre},{p.Stock},{p.PrecioUnidad}");
                }
            }

            MessageBox.Show("Inventario exportado a CSV con éxito.");
        }
    }
}
