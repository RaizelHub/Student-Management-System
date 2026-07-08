StudentAttendanceSysttem
=========================

Simple student attendance manager (WinForms, .NET 10).

Requirements
- .NET 10 SDK
- Visual Studio 2026 (or VS Code with Windows Forms support)
- MySQL Server (or compatible) and MySqlConnector

Quick start
1. Clone the repository:
   git clone <repo-url>
2. Open the solution:
   Open StudentAttendanceSysttem.slnx in Visual Studio 2026
3. Restore and build:
   - Visual Studio: Build Solution
   - CLI: dotnet restore && dotnet build
4. Configure the database connection:
   - Update the connection string used by the application (App.config / appsettings or the Db helper in the project). Example connection string:
	 Server=localhost;Database=student_attendance;User Id=root;Password=yourpassword;SslMode=None;

Database schema (example)
-- Courses table
CREATE TABLE `courses` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `course_name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB;

-- Sections table (must include `id` column expected by the application)
CREATE TABLE `sections` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `section_name` VARCHAR(255) NOT NULL,
  `course_id` INT DEFAULT NULL,
  `year_level` VARCHAR(50),
  `school_year` VARCHAR(50),
  `is_active` TINYINT(1) DEFAULT 1,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`course_id`) REFERENCES `courses`(`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

If you receive an error like "Unknown column 's.id' in 'field list'":
- Confirm the `sections` table has an `id` column (or adjust the query to use the real primary key name).
- Verify the application is pointing to the correct database/schema (check the connection string and run SELECT DATABASE();).

Running the app
- From Visual Studio: Run (F5) the Windows Forms project.
- From CLI (project directory): dotnet run

Testing
- No automated tests included. Manual testing: open the Sections form from the dashboard and confirm the grid loads.

Contributing
- Fork, create a branch, make changes, and submit a pull request.

License
- Specify a license or add one to the repo (e.g., MIT) if you plan to publish.

Contact
- Open an issue with troubleshooting details or the SQL schema if you need help setting up the database.
