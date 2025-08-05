using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Rapimesa.Data;

namespace Rapimesa
{
    public partial class LoginForm : Form
    {
        private readonly AccountManager _accountManager;
        private List<User> _users;

        public LoginForm()
        {
            InitializeComponent();
            _accountManager = new Data.AccountManager();
            LoadUsers();

            if (_users.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios. Registra al primer Supervisor.");
                var registro = new RegisterUserForm();
                registro.ShowDialog();
                LoadUsers();
            }
        }
        private void btnRecuperar_Click(object sender, EventArgs e)
        {
            var recoverForm = new RecoverPasswordForm();
            recoverForm.ShowDialog();
        }

        private void LoadUsers()
        {
            _users = _accountManager.GetUsers();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var user = _accountManager.GetUser(usernameTextBox.Text);
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
                _accountManager.UpdateUser(user);
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
                _accountManager.UpdateUser(user);
            }
        }
    }

}
