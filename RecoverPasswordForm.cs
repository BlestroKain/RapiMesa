using System;
using System.Windows.Forms;
using Rapimesa.Data;

namespace Rapimesa
{
    public partial class RecoverPasswordForm : Form
    {
        private readonly AccountManager _accountManager;

        public RecoverPasswordForm()
        {
            InitializeComponent();
            _accountManager = new AccountManager();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var username = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Ingresa el nombre de usuario.");
                return;
            }

            var user = _accountManager.GetUser(username);
            if (user == null)
            {
                MessageBox.Show("Usuario no encontrado.");
                return;
            }
          

            lblPregunta.Text = user.SecurityQuestion;
            pnlPregunta.Visible = true;
        }
        private void btnVerificar_Click(object sender, EventArgs e)
        {
            var username = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(txtRespuesta.Text))
            {
                MessageBox.Show("Ingresa una respuesta.");
                return;
            }

            var user = _accountManager.GetUser(username);
            if (user == null) return;

            if (txtRespuesta.Text.Trim().Equals(user.SecurityAnswer, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"Tu contraseña es: {user.Password}", "Recuperación Exitosa");
                this.Close();
            }
            else
            {
                MessageBox.Show("Respuesta incorrecta.");
            }
        }

    }
}
