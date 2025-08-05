using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class RecoverPasswordForm : Form
    {
        private List<User> _users;
        private string userFile = "usuarios.json";

        public RecoverPasswordForm()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (!File.Exists(userFile))
            {
                _users = new List<User>();
                return;
            }

            var json = File.ReadAllText(userFile);
            var serializer = new JavaScriptSerializer();
            _users = serializer.Deserialize<List<User>>(json);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var username = txtUsuario.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Ingresa el nombre de usuario.");
                return;
            }

            var user = _users.Find(u => u.Username == username);
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

            var user = _users.Find(u => u.Username == username);
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
