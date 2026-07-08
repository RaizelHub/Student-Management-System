namespace StudentAttendanceSysttem.Forms
{
    partial class StudentDetailsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label          lblTitle;
        private System.Windows.Forms.TextBox        txtStudentNumber, txtFirstName, txtMiddleName, txtLastName;
        private System.Windows.Forms.ComboBox       cmbGender, cmbCourse, cmbSection;
        private System.Windows.Forms.DateTimePicker dtpBirthDate;
        private System.Windows.Forms.TextBox        txtAddress, txtContact, txtEmail;
        private System.Windows.Forms.TextBox        txtYearLevel, txtRfid, txtFingerprint;
        private System.Windows.Forms.PictureBox     picPhoto;
        private System.Windows.Forms.Button         btnPhoto, btnSave, btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.Text          = "Student Details";
            this.Size          = new System.Drawing.Size(780, 680);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor     = System.Drawing.Color.FromArgb(28, 28, 44);
            this.Load         += StudentDetailsForm_Load;

            lblTitle = MakeLabel("Add New Student", 20, 16, 16, true);
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);

            // Column 1 (x=20)
            int x1 = 20, y = 60;
            var lblSN   = MakeLabel("Student Number *", x1, y);
            txtStudentNumber = MakeTextBox(x1, y + 22, 200);

            y += 68;
            var lblFN = MakeLabel("First Name *", x1, y);
            txtFirstName = MakeTextBox(x1, y + 22, 200);

            y += 68;
            var lblMN = MakeLabel("Middle Name", x1, y);
            txtMiddleName = MakeTextBox(x1, y + 22, 200);

            y += 68;
            var lblLN = MakeLabel("Last Name *", x1, y);
            txtLastName = MakeTextBox(x1, y + 22, 200);

            y += 68;
            var lblGen = MakeLabel("Gender *", x1, y);
            cmbGender = new System.Windows.Forms.ComboBox
            {
                Location    = new System.Drawing.Point(x1, y + 22),
                Size        = new System.Drawing.Size(200, 30),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            };
            cmbGender.Items.AddRange(new object[] { "Male", "Female", "Other" });

            y += 68;
            var lblBD = MakeLabel("Birth Date", x1, y);
            dtpBirthDate = new System.Windows.Forms.DateTimePicker
            {
                Location = new System.Drawing.Point(x1, y + 22),
                Size     = new System.Drawing.Size(200, 30),
                Checked  = false,
                ShowCheckBox = true,
                Format   = System.Windows.Forms.DateTimePickerFormat.Short
            };

            // Column 2 (x=240)
            int x2 = 250, y2 = 60;
            var lblCourse = MakeLabel("Course", x2, y2);
            cmbCourse = new System.Windows.Forms.ComboBox
            {
                Location      = new System.Drawing.Point(x2, y2 + 22),
                Size          = new System.Drawing.Size(220, 30),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            };
            cmbCourse.SelectedIndexChanged += cmbCourse_SelectedIndexChanged;

            y2 += 68;
            var lblSect = MakeLabel("Section", x2, y2);
            cmbSection = new System.Windows.Forms.ComboBox
            {
                Location      = new System.Drawing.Point(x2, y2 + 22),
                Size          = new System.Drawing.Size(220, 30),
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            };

            y2 += 68;
            var lblYL = MakeLabel("Year Level", x2, y2);
            txtYearLevel = MakeTextBox(x2, y2 + 22, 220);

            y2 += 68;
            var lblAddr = MakeLabel("Address", x2, y2);
            txtAddress = MakeTextBox(x2, y2 + 22, 220, 60);

            y2 += 92;
            var lblContact = MakeLabel("Contact Number", x2, y2);
            txtContact = MakeTextBox(x2, y2 + 22, 220);

            y2 += 68;
            var lblEmail = MakeLabel("Email", x2, y2);
            txtEmail = MakeTextBox(x2, y2 + 22, 220);

            y2 += 68;
            var lblRfid = MakeLabel("RFID Number", x2, y2);
            txtRfid = MakeTextBox(x2, y2 + 22, 220);

            y2 += 68;
            var lblFP = MakeLabel("Fingerprint ID", x2, y2);
            txtFingerprint = MakeTextBox(x2, y2 + 22, 220);

            // Photo (column 3)
            int x3 = 500;
            var lblPhoto = MakeLabel("Photo", x3, 60);
            picPhoto = new System.Windows.Forms.PictureBox
            {
                Location  = new System.Drawing.Point(x3, 82),
                Size      = new System.Drawing.Size(200, 200),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                SizeMode  = System.Windows.Forms.PictureBoxSizeMode.Zoom
            };
            btnPhoto = new System.Windows.Forms.Button
            {
                Text     = "📷 Choose Photo",
                Location = new System.Drawing.Point(x3, 290),
                Size     = new System.Drawing.Size(200, 32)
            };
            btnPhoto.Click += btnPhoto_Click;

            // Save / Cancel
            btnSave = new System.Windows.Forms.Button
            {
                Text     = "💾 Save",
                Location = new System.Drawing.Point(490, 590),
                Size     = new System.Drawing.Size(120, 36)
            };
            btnSave.Click += btnSave_Click;

            btnCancel = new System.Windows.Forms.Button
            {
                Text     = "Cancel",
                Location = new System.Drawing.Point(626, 590),
                Size     = new System.Drawing.Size(100, 36)
            };
            btnCancel.Click += btnCancel_Click;

            this.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                lblTitle,
                lblSN, txtStudentNumber, lblFN, txtFirstName, lblMN, txtMiddleName,
                lblLN, txtLastName, lblGen, cmbGender, lblBD, dtpBirthDate,
                lblCourse, cmbCourse, lblSect, cmbSection, lblYL, txtYearLevel,
                lblAddr, txtAddress, lblContact, txtContact, lblEmail, txtEmail,
                lblRfid, txtRfid, lblFP, txtFingerprint,
                lblPhoto, picPhoto, btnPhoto, btnSave, btnCancel
            });
        }

        private static System.Windows.Forms.Label MakeLabel(string text, int x, int y, int fontSize = 9, bool bold = false)
        {
            return new System.Windows.Forms.Label
            {
                Text      = text,
                AutoSize  = true,
                Location  = new System.Drawing.Point(x, y),
                ForeColor = System.Drawing.Color.FromArgb(148, 163, 184),
                Font      = new System.Drawing.Font("Segoe UI", fontSize, bold
                    ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular)
            };
        }

        private static System.Windows.Forms.TextBox MakeTextBox(int x, int y, int width, int height = 28)
        {
            return new System.Windows.Forms.TextBox
            {
                Location  = new System.Drawing.Point(x, y),
                Size      = new System.Drawing.Size(width, height),
                BackColor = System.Drawing.Color.FromArgb(35, 35, 55),
                ForeColor = System.Drawing.Color.White,
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Multiline = height > 30
            };
        }
    }
}
