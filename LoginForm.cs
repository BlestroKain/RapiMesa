using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class LoginForm : Form
    {
        private List<User> _users;
        private string userFile = "usuarios.json";

        public LoginForm()
        {
            InitializeComponent();
            LoadUsers();

            // 👉 Si no hay usuarios, abrir el formulario de registro automáticamente
            if (_users.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios. Registra al primer Supervisor.");
                var registro = new RegisterUserForm();
                registro.ShowDialog();
                LoadUsers(); // Cargar de nuevo después de registrar
            }
        }

        private void LoadUsers()
        {
            if (!File.Exists(userFile))
            {
                _users = new List<User>();
                SaveUsers();
            }
            else
            {
                try
                {
                    var json = File.ReadAllText(userFile);
                    var serializer = new JavaScriptSerializer();
                    _users = serializer.Deserialize<List<User>>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error cargando usuarios: " + ex.Message);
                    _users = new List<User>();
                }
            }
        }

        private void SaveUsers()
        {
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(_users);
            File.WriteAllText(userFile, json);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var user = _users.Find(u => u.Username == usernameTextBox.Text);
            if (user == null)
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
                return;
            }

            if (user.IsLocked)
            {
                MessageBox.Show("Cuenta bloqueada. Contacta a un supervisor.");
                return;
            }

            if (user.Password == passwordTextBox.Text)
            {
                user.FailedAttempts = 0;
                SaveUsers();
                var main = new MainForm(user);
                main.Show();
                this.Hide();
            }
            else
            {
                user.FailedAttempts++;
                if (user.FailedAttempts >= 3)
                {
                    user.IsLocked = true;
                    MessageBox.Show("Cuenta bloqueada por múltiples intentos fallidos.");
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.");
                }
                SaveUsers();
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string NombreCompleto { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
    }
}
