using Rapimesa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace Rapimesa
{
    public partial class RegisterUserForm : Form
    {
        private List<User> _users;
  

        public RegisterUserForm()
        {
            InitializeComponent();
            _users = UserFileManager.LoadUsers();
            SetupRolOptions();
        }
        private void SetupRolOptions()
        {
            if (_users.Count == 0)
            {
                // Primer usuario, lo forzamos a Supervisor
                cmbRol.Items.Add("Supervisor");
                cmbRol.SelectedIndex = 0;
                cmbRol.Enabled = false;
            }
            else
            {
                cmbRol.Items.Add("Mesero");
                cmbRol.Items.Add("Supervisor");
                cmbRol.SelectedIndex = 0;
            }
        }

        private void SaveUsers()
        {
            UserFileManager.SaveUsers(_users);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var password = txtPassword.Text.Trim();
            var nombre = txtNombreCompleto.Text.Trim();
            var rol = cmbRol.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(rol))
            {
                MessageBox.Show("Completa todos los campos.");
                return;
            }

            if (_users.Any(u => u.Username == username))
            {
                MessageBox.Show("El usuario ya existe.");
                return;
            }
            var pregunta = txtPregunta.Text.Trim();
            var respuesta = txtRespuesta.Text.Trim();

            if (string.IsNullOrWhiteSpace(pregunta) || string.IsNullOrWhiteSpace(respuesta))
            {
                MessageBox.Show("Debes ingresar la pregunta y la respuesta secreta.");
                return;
            }

            _users.Add(new User
            {
                Username = username,
                Password = password,
                Rol = rol,
                NombreCompleto = nombre,
                FailedAttempts = 0,
                IsLocked = false,
                SecurityQuestion = pregunta,
                SecurityAnswer = respuesta
            });


            SaveUsers();
            MessageBox.Show("Usuario registrado con éxito.");
            this.Close();
        }


    }
}