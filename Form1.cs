using System;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using StudentAttendanceSysttem.Database;

namespace StudentAttendanceSysttem
{
    public partial class Form1 : Form
    {
        DatabaseConnection db = new DatabaseConnection();

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadAttendance()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();

                    string query = "SELECT * FROM attendance";

                    using (var adapter = new MySqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAttendance();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadAttendance();
        }
    }
}