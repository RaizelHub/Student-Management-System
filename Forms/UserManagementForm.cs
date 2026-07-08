using System;
using System.Windows.Forms;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;
using StudentAttendanceSysttem.Repositories;
using StudentAttendanceSysttem.Services;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class UserManagementForm : Form
    {
        private readonly UserService _service;
        private readonly AuditService _audit;
        private readonly DatabaseConnection _db;
        private int _editingUserId;

        public UserManagementForm()
        {
            InitializeComponent();
            _db      = new DatabaseConnection();
            _audit   = new AuditService(_db);
            _service = new UserService(new UserRepository(_db));
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            if (!SessionManager.Instance.IsAdmin)
            { NotificationHelper.ShowError("Access denied. Administrator only."); this.Close(); return; }
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvUsers);
            ThemeManager.StylePrimaryButton(btnSave);
            ThemeManager.StyleDangerButton(btnDelete);
            ThemeManager.StyleSecondaryButton(btnReset);
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgvUsers.DataSource = null;
            var users = _service.GetAllUsers();
            var dt = new System.Data.DataTable();
            dt.Columns.Add("id", typeof(int));
            dt.Columns.Add("Username"); dt.Columns.Add("Full Name"); dt.Columns.Add("Email");
            dt.Columns.Add("Role"); dt.Columns.Add("Active"); dt.Columns.Add("Last Login");
            foreach (var u in users)
                dt.Rows.Add(u.Id, u.Username, u.FullName, u.Email, u.Role,
                    u.IsActive ? "Yes" : "No",
                    u.LastLogin?.ToString("MM/dd/yyyy hh:mm tt") ?? "Never");
            dgvUsers.DataSource = dt;
            if (dgvUsers.Columns.Contains("id")) dgvUsers.Columns["id"]!.Visible = false;
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = dgvUsers.Rows[e.RowIndex];
            _editingUserId     = Convert.ToInt32(row.Cells["id"].Value);
            txtUsername.Text   = row.Cells["Username"].Value?.ToString();
            txtFirstName.Text  = row.Cells["Full Name"].Value?.ToString()?.Split(' ')[0];
            txtEmail.Text      = row.Cells["Email"].Value?.ToString();
            cmbRole.Text       = row.Cells["Role"].Value?.ToString();
            chkActive.Checked  = row.Cells["Active"].Value?.ToString() == "Yes";
            txtPassword.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isNew = _editingUserId == 0;
            if (isNew)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                { NotificationHelper.ShowWarning("Password is required for new users."); return; }
                var user = new User
                {
                    Username  = txtUsername.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    Email     = txtEmail.Text.Trim(),
                    Role      = cmbRole.Text,
                    IsActive  = chkActive.Checked
                };
                if (_service.UsernameExists(user.Username))
                { NotificationHelper.ShowError("Username already exists."); return; }
                int newId = _service.CreateUser(user, txtPassword.Text);
                _audit.LogAction("ADD_USER", $"Created user: {user.Username}");
                NotificationHelper.ShowSuccess("User created.");
            }
            else
            {
                var user = _service.GetAllUsers().Find(u => u.Id == _editingUserId);
                if (user == null) return;
                user.FirstName = txtFirstName.Text.Trim();
                user.Email     = txtEmail.Text.Trim();
                user.Role      = cmbRole.Text;
                user.IsActive  = chkActive.Checked;
                _service.UpdateUser(user);
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                    _service.ChangePassword(_editingUserId, txtPassword.Text, txtPassword.Text);
                _audit.LogAction("UPDATE_USER", $"Updated user: {user.Username}");
                NotificationHelper.ShowSuccess("User updated.");
            }
            ClearInputs(); LoadUsers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_editingUserId == 0) { NotificationHelper.ShowWarning("Select a user."); return; }
            if (_editingUserId == SessionManager.Instance.UserId)
            { NotificationHelper.ShowError("Cannot delete yourself."); return; }
            if (!NotificationHelper.Confirm("Delete this user?")) return;
            _service.DeleteUser(_editingUserId);
            _audit.LogAction("DELETE_USER", $"Deleted user ID: {_editingUserId}");
            ClearInputs(); LoadUsers();
            NotificationHelper.ShowSuccess("User deleted.");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (_editingUserId == 0) { NotificationHelper.ShowWarning("Select a user."); return; }
            string temp = _service.ResetPassword(_editingUserId);
            _audit.LogAction("RESET_PASSWORD", $"Password reset for user ID: {_editingUserId}");
            NotificationHelper.ShowInfo($"Temporary password: {temp}\n\nPlease share this with the user securely.", "Password Reset");
        }

        private void btnNew_Click(object sender, EventArgs e) => ClearInputs();

        private void ClearInputs()
        {
            _editingUserId = 0;
            txtUsername.Clear(); txtFirstName.Clear(); txtEmail.Clear();
            txtPassword.Clear(); cmbRole.SelectedIndex = 0; chkActive.Checked = true;
        }
    }
}
