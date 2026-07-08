using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using StudentAttendanceSysttem.Database;
using StudentAttendanceSysttem.Models;

namespace StudentAttendanceSysttem.Repositories
{
    /// <summary>
    /// Data access layer for the <c>students</c> table.
    /// ASSUMPTION: columns — id, student_number, first_name, middle_name, last_name,
    ///   gender, birth_date, address, contact_number, email, course_id, section_id,
    ///   year_level, rfid_number, fingerprint_id, qr_code, photo_path, is_active,
    ///   created_at, updated_at
    /// </summary>
    public class StudentRepository
    {
        private readonly DatabaseConnection _db;
        public StudentRepository(DatabaseConnection db) => _db = db;

        // ─── Read ─────────────────────────────────────────────────────────────────

        public List<Student> GetAll(bool activeOnly = true)
        {
            var students = new List<Student>();
            using var conn = _db.GetConnection();
            conn.Open();
            string sql = @"
                SELECT s.*, c.course_name, sec.section_name
                FROM   students s
                LEFT JOIN courses  c   ON s.course_id   = c.id
                LEFT JOIN sections sec ON s.section_id  = sec.id"
                + (activeOnly ? " WHERE s.is_active = 1" : "")
                + " ORDER BY s.last_name, s.first_name";

            using var cmd    = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) students.Add(MapStudent(reader));
            return students;
        }

        public Student? GetById(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT s.*, c.course_name, sec.section_name
                FROM   students s
                LEFT JOIN courses  c   ON s.course_id  = c.id
                LEFT JOIN sections sec ON s.section_id = sec.id
                WHERE s.id = @id LIMIT 1";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? MapStudent(reader) : null;
        }

        public DataTable Search(string query)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT s.id, s.student_number,
                       CONCAT(s.first_name,' ',IFNULL(s.middle_name,''),' ',s.last_name) AS full_name,
                       s.gender, s.year_level, c.course_name, sec.section_name,
                       s.email, s.contact_number, s.is_active
                FROM   students s
                LEFT JOIN courses  c   ON s.course_id  = c.id
                LEFT JOIN sections sec ON s.section_id = sec.id
                WHERE  s.student_number LIKE @q
                    OR s.first_name     LIKE @q
                    OR s.last_name      LIKE @q
                    OR s.email          LIKE @q
                    OR c.course_name    LIKE @q
                ORDER BY s.last_name, s.first_name
                LIMIT 200";

            using var adapter = new MySqlDataAdapter(sql, conn);
            adapter.SelectCommand!.Parameters.AddWithValue("@q", $"%{query}%");
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public DataTable GetAllAsTable()
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                SELECT s.id, s.student_number,
                       CONCAT(s.first_name,' ',IFNULL(s.middle_name,''),' ',s.last_name) AS full_name,
                       s.gender, s.year_level, c.course_name, sec.section_name,
                       s.email, s.contact_number, s.is_active
                FROM   students s
                LEFT JOIN courses  c   ON s.course_id  = c.id
                LEFT JOIN sections sec ON s.section_id = sec.id
                WHERE s.is_active = 1
                ORDER BY s.last_name, s.first_name";

            using var adapter = new MySqlDataAdapter(sql, conn);
            var dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        // ─── Write ────────────────────────────────────────────────────────────────

        public int Add(Student s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                INSERT INTO students
                    (student_number, first_name, middle_name, last_name, gender,
                     birth_date, address, contact_number, email,
                     course_id, section_id, year_level, rfid_number,
                     fingerprint_id, qr_code, photo_path, is_active, created_at, updated_at)
                VALUES
                    (@sn, @fn, @mn, @ln, @gender,
                     @bd, @addr, @phone, @email,
                     @cid, @sid, @yl, @rfid,
                     @fp, @qr, @photo, 1, NOW(), NOW());
                SELECT LAST_INSERT_ID();";

            using var cmd = BuildStudentCommand(sql, s, conn);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Update(Student s)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = @"
                UPDATE students SET
                    student_number = @sn, first_name = @fn, middle_name = @mn,
                    last_name = @ln, gender = @gender, birth_date = @bd,
                    address = @addr, contact_number = @phone, email = @email,
                    course_id = @cid, section_id = @sid, year_level = @yl,
                    rfid_number = @rfid, fingerprint_id = @fp, qr_code = @qr,
                    photo_path = @photo, updated_at = NOW()
                WHERE id = @id";

            using var cmd = BuildStudentCommand(sql, s, conn);
            cmd.Parameters.AddWithValue("@id", s.Id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Delete(int id)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "UPDATE students SET is_active = 0, updated_at = NOW() WHERE id = @id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool StudentNumberExists(string studentNumber, int excludeId = 0)
        {
            using var conn = _db.GetConnection();
            conn.Open();
            const string sql = "SELECT COUNT(*) FROM students WHERE student_number = @sn AND id != @excludeId";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sn",        studentNumber);
            cmd.Parameters.AddWithValue("@excludeId", excludeId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static MySqlCommand BuildStudentCommand(string sql, Student s, MySqlConnection conn)
        {
            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sn",     s.StudentNumber);
            cmd.Parameters.AddWithValue("@fn",     s.FirstName);
            cmd.Parameters.AddWithValue("@mn",     string.IsNullOrWhiteSpace(s.MiddleName) ? DBNull.Value : s.MiddleName);
            cmd.Parameters.AddWithValue("@ln",     s.LastName);
            cmd.Parameters.AddWithValue("@gender", s.Gender);
            cmd.Parameters.AddWithValue("@bd",     s.BirthDate.HasValue ? s.BirthDate.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@addr",   s.Address);
            cmd.Parameters.AddWithValue("@phone",  s.ContactNumber);
            cmd.Parameters.AddWithValue("@email",  s.Email);
            cmd.Parameters.AddWithValue("@cid",    s.CourseId > 0 ? s.CourseId : DBNull.Value);
            cmd.Parameters.AddWithValue("@sid",    s.SectionId > 0 ? s.SectionId : DBNull.Value);
            cmd.Parameters.AddWithValue("@yl",     s.YearLevel);
            cmd.Parameters.AddWithValue("@rfid",   s.RfidNumber);
            cmd.Parameters.AddWithValue("@fp",     s.FingerprintId);
            cmd.Parameters.AddWithValue("@qr",     s.QrCode);
            cmd.Parameters.AddWithValue("@photo",  string.IsNullOrWhiteSpace(s.PhotoPath) ? DBNull.Value : s.PhotoPath);
            return cmd;
        }

        private static Student MapStudent(MySqlDataReader r)
        {
            bool hasCourse  = HasColumn(r, "course_name");
            bool hasSection = HasColumn(r, "section_name");
            return new Student
            {
                Id            = r.GetInt32("id"),
                StudentNumber = r.GetString("student_number"),
                FirstName     = SafeString(r, "first_name"),
                MiddleName    = SafeString(r, "middle_name"),
                LastName      = SafeString(r, "last_name"),
                Gender        = SafeString(r, "gender"),
                BirthDate     = r.IsDBNull(r.GetOrdinal("birth_date")) ? null : r.GetDateTime("birth_date"),
                Address       = SafeString(r, "address"),
                ContactNumber = SafeString(r, "contact_number"),
                Email         = SafeString(r, "email"),
                CourseId      = r.IsDBNull(r.GetOrdinal("course_id"))   ? 0 : r.GetInt32("course_id"),
                SectionId     = r.IsDBNull(r.GetOrdinal("section_id"))  ? 0 : r.GetInt32("section_id"),
                CourseName    = hasCourse  && !r.IsDBNull(r.GetOrdinal("course_name"))   ? r.GetString("course_name")   : "",
                SectionName   = hasSection && !r.IsDBNull(r.GetOrdinal("section_name"))  ? r.GetString("section_name")  : "",
                YearLevel     = SafeString(r, "year_level"),
                RfidNumber    = SafeString(r, "rfid_number"),
                FingerprintId = SafeString(r, "fingerprint_id"),
                QrCode        = SafeString(r, "qr_code"),
                PhotoPath     = r.IsDBNull(r.GetOrdinal("photo_path")) ? null : r.GetString("photo_path"),
                IsActive      = r.GetBoolean("is_active"),
                CreatedAt     = r.GetDateTime("created_at"),
                UpdatedAt     = r.GetDateTime("updated_at"),
            };
        }

        private static string SafeString(MySqlDataReader r, string col)
        {
            int ord = r.GetOrdinal(col);
            return r.IsDBNull(ord) ? "" : r.GetString(ord);
        }

        private static bool HasColumn(MySqlDataReader r, string col)
        {
            for (int i = 0; i < r.FieldCount; i++)
                if (r.GetName(i).Equals(col, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }
}
