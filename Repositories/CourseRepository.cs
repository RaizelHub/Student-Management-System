using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Data access for the <c>courses</c> table.
    /// ASSUMPTION: id, course_code, course_name, description, is_active, created_at, updated_at
    /// </summary>
    public class CourseRepository
    {
        private readonly DatabaseConnection _db;
        public CourseRepository(DatabaseConnection db) => _db = db;

        public List<Course> GetAll(bool activeOnly = true)
        {
            var list = new List<Course>();
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = "SELECT * FROM courses"
                + (activeOnly ? " WHERE is_active = 1" : "")
                + " ORDER BY course_name";
            using var cmd    = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) list.Add(Map(reader));
            return list;
        }

        public DataTable GetAllAsTable()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "SELECT id, course_code, course_name, description, is_active FROM courses WHERE is_active = 1 ORDER BY course_name";
            using var adapter = new MySqlDataAdapter(sql, conn);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public Course? GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("SELECT * FROM courses WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? Map(reader) : null;
        }

        public int Add(Course c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"INSERT INTO courses (course_code, course_name, description, is_active, created_at, updated_at)
                                 VALUES (@code, @name, @desc, 1, NOW(), NOW()); SELECT LAST_INSERT_ID();";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", c.CourseCode);
            cmd.Parameters.AddWithValue("@name", c.CourseName);
            cmd.Parameters.AddWithValue("@desc", c.Description);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(Course c)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE courses SET course_code=@code, course_name=@name, description=@desc, updated_at=NOW() WHERE id=@id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@code", c.CourseCode);
            cmd.Parameters.AddWithValue("@name", c.CourseName);
            cmd.Parameters.AddWithValue("@desc", c.Description);
            cmd.Parameters.AddWithValue("@id",   c.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE courses SET is_active = 0, updated_at = NOW() WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool CourseNameExists(string name, int excludeId = 0)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM courses WHERE course_name = @name AND id != @eid", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@eid",  excludeId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private static Course Map(MySqlDataReader r) => new()
        {
            Id          = r.GetInt32("id"),
            CourseCode  = r.IsDBNull(r.GetOrdinal("course_code"))  ? "" : r.GetString("course_code"),
            CourseName  = r.GetString("course_name"),
            Description = r.IsDBNull(r.GetOrdinal("description"))  ? "" : r.GetString("description"),
            IsActive    = r.GetBoolean("is_active"),
            CreatedAt   = r.GetDateTime("created_at"),
            UpdatedAt   = r.GetDateTime("updated_at"),
        };
    }
}
