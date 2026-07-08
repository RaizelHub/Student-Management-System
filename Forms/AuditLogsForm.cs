using System;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Utilities;

namespace StudentAttendanceSysttem.Forms
{
    public partial class AuditLogsForm : Form
    {
        private readonly DatabaseConnection _db;

        public AuditLogsForm()
        {
            InitializeComponent();
            _db = new DatabaseConnection();
        }

        private void AuditLogsForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            ThemeManager.StyleDataGridView(dgvLogs);
            ThemeManager.StylePrimaryButton(btnFilter);
            ThemeManager.StyleSecondaryButton(btnClear);
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value   = DateTime.Today;
            LoadLogs();
        }

        private void LoadLogs(string? action = null)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                string sql = @"
                    SELECT id, username, action, description, ip_address, created_at
                    FROM   logs
                    WHERE  created_at BETWEEN @from AND @to";
                if (!string.IsNullOrEmpty(action)) sql += " AND action = @action";
                sql += " ORDER BY created_at DESC LIMIT 1000";

                using var adapter = new MySqlDataAdapter(sql, conn);
                adapter.SelectCommand!.Parameters.AddWithValue("@from", dtpFrom.Value.Date);
                adapter.SelectCommand.Parameters.AddWithValue("@to",   dtpTo.Value.Date.AddDays(1));
                if (!string.IsNullOrEmpty(action))
                    adapter.SelectCommand.Parameters.AddWithValue("@action", action);

                var dt = new DataTable();
                adapter.Fill(dt);
                dgvLogs.DataSource = dt;
                lblCount.Text = $"Records: {dt.Rows.Count}";

                if (dgvLogs.Columns.Contains("id")) dgvLogs.Columns["id"]!.Visible = false;
            }
            catch (Exception ex) { NotificationHelper.ShowError(ex.Message); }
        }

        private void btnFilter_Click(object sender, EventArgs e) =>
            LoadLogs(cmbAction.SelectedItem?.ToString()?.Replace("All Actions",""));

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbAction.SelectedIndex = 0;
            LoadLogs();
        }
    }
}
