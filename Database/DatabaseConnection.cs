using System;
using System.Configuration;
using System.Threading.Tasks;
using MySqlConnector;

namespace StudentAttendanceSysttem.Database
{
    /// <summary>
    /// Provides a centralized, production-ready MySQL database connection factory.
    /// Uses connection pooling and reads credentials exclusively from App.config.
    /// </summary>
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection()
        {
            var cs = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;

            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException(
                    "Connection string 'DefaultConnection' is missing in App.config. " +
                    "Please add it under <connectionStrings>.");

            _connectionString = cs;
        }

        /// <summary>Returns a new (pooled) synchronous MySQL connection. Caller must open and dispose it.</summary>
        public MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        /// <summary>Returns an already-opened asynchronous connection.</summary>
        public async Task<MySqlConnection> GetOpenConnectionAsync()
        {
            var conn = new MySqlConnection(_connectionString);
            await conn.OpenAsync().ConfigureAwait(false);
            return conn;
        }

        /// <summary>Tests connectivity and returns true on success, false on failure.</summary>
        public bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[DatabaseConnection] TestConnection failed: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Check whether the specified column exists for a table in the current database.
        /// </summary>
        public bool ColumnExists(string tableName, string columnName)
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                const string sql = @"SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
                                     WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = @table AND COLUMN_NAME = @column";
                using var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@table", tableName);
                cmd.Parameters.AddWithValue("@column", columnName);
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
