using Rapimesa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;
namespace Rapimesa
{
    public partial class RegisterUserForm : Form
    {
        private string userFile = "usuarios.json";
        private List<User> _users;

        public RegisterUserForm()
        {
            InitializeComponent();
            LoadUsers();
            SetupRolOptions();
        }
        private void LoadUsers()
        {
            if (!File.Exists(userFile))
                _users = new List<User>();
            else
            {
                var json = File.ReadAllText(userFile);
                var serializer = new JavaScriptSerializer();
                _users = serializer.Deserialize<List<User>>(json);
            }
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
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(_users);
            File.WriteAllText(userFile, json);
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

            _users.Add(new User
            {
                Username = username,
                Password = password,
                Rol = rol,
                NombreCompleto = nombre,
                FailedAttempts = 0,
                IsLocked = false
            });

            SaveUsers();
            MessageBox.Show("Usuario registrado con éxito.");
            this.Close();
        }


    }
}