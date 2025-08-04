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
            var user = _users.Find(u => u.Username == usernameTextBox.Text && u.Password == passwordTextBox.Text);
            if (user != null)
            {
                var main = new MainForm(user);
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Usuario o contraseña incorrectos.");
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string NombreCompleto { get; set; }
    }
}
