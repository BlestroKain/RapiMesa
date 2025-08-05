using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rapimesa
{
    public partial class LoginForm : Form
    {
        private List<User> _users;
        private int intentosFallidos = 0;
        private const int maxIntentos = 3;

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
        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            var recoverForm = new RecoverPasswordForm();
            recoverForm.ShowDialog();
        }

        private void LoadUsers()
        {
            _users = UserFileManager.LoadUsers();
            if (_users == null)
                _users = new List<User>();
            SaveUsers();
        }

        private void SaveUsers()
        {
            UserFileManager.SaveUsers(_users);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var user = _users.Find(u => u.Username == usernameTextBox.Text);
            if (string.IsNullOrWhiteSpace(usernameTextBox.Text) || string.IsNullOrWhiteSpace(passwordTextBox.Text))
            {
                MessageBox.Show("Por favor ingresa usuario y contraseña.");
                return;
            }

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
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }
}
