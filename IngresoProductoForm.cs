using Rapimesa.Data;
using System;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class IngresoProductoForm : Form
    {
        public IngresoProductoForm()
        {
            InitializeComponent();

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var nombre = txtNombre.Text.Trim();
            var cantidad = (int)numCantidad.Value;
            var precio = (double)numPrecio.Value;

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del producto es obligatorio.");
                return;
            }

                     
            var nuevoProducto = new Product
            {
                Nombre = nombre,
                Stock = cantidad,
                PrecioUnidad = precio,
             
            };

            ProductManager.Add(nuevoProducto);

            MessageBox.Show("Producto agregado correctamente.");
            this.Close();
        }

        private void Cancelbttn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

  
}
