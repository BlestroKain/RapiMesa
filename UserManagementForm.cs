using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Rapimesa.Data;

namespace Rapimesa
{
    public partial class UserManagementForm : Form
    {
        private BindingList<User> _users;
        private readonly AccountManager _accountManager;

        public UserManagementForm()
        {
            InitializeComponent();
            _accountManager = new AccountManager();
            LoadUsers();

            // Eventos para guardar al editar celda
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dataGridView1.IsCurrentCellDirty)
                    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            // Evento para configurar columnas después de cargar
            this.Load += UserManagementForm_Load;
        }

        private void LoadUsers()
        {
            var lista = _accountManager.GetUsers();
            _users = new BindingList<User>(lista);
            dataGridView1.DataSource = _users;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            _accountManager.UpdateUser(_users[e.RowIndex]);
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            if (dataGridView1.Columns["Username"] != null)
            {
                dataGridView1.Columns["Username"].ReadOnly = true;
            }

            if (dataGridView1.Columns["Password"] != null)
            {
                dataGridView1.Columns["Password"].Visible = false;
            }

            // Reemplazar columna Rol por ComboBox
            if (dataGridView1.Columns["Rol"] != null)
            {
                int index = dataGridView1.Columns["Rol"].Index;
                dataGridView1.Columns.Remove("Rol");

                var comboCol = new DataGridViewComboBoxColumn
                {
                    Name = "Rol",
                    DataPropertyName = "Rol",
                    HeaderText = "Rol",
                    DataSource = new string[] { "Supervisor", "Mesero" },
                    FlatStyle = FlatStyle.Flat
                };

                dataGridView1.Columns.Insert(index, comboCol);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var username = dataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();
            var confirm = MessageBox.Show($"¿Eliminar al usuario {username}?", "Confirmar", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                var userToRemove = _users.FirstOrDefault(u => u.Username == username);
                if (userToRemove != null)
                {
                    _accountManager.DeleteUser(username);
                    _users.Remove(userToRemove);
                }
            }
        }

        private void btnRegistrarUsuario_Click(object sender, EventArgs e)
        {
            var formRegistro = new RegisterUserForm();
            formRegistro.ShowDialog();
            LoadUsers(); // refrescar después del registro
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            var username = dataGridView1.SelectedRows[0].Cells["Username"].Value.ToString();
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.IsLocked = false;
                user.FailedAttempts = 0;
                _accountManager.UpdateUser(user);
                dataGridView1.Refresh(); // actualizar visualmente
                MessageBox.Show("Usuario desbloqueado.");
            }
        }
    }
}
