using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentAttendanceSysttem.Utilities
{
    /// <summary>
    /// Centralises the application's color palette, fonts, and control styling.
    /// Supports Dark Mode (default) and Light Mode.
    /// Call <see cref="ApplyTheme(Form)"/> on every form after InitializeComponent().
    /// </summary>
    public static class ThemeManager
    {
        // ─── Theme State ──────────────────────────────────────────────────────────
        public static bool IsDarkMode { get; private set; } = true;

        // ─── Dark Mode Palette ────────────────────────────────────────────────────
        public static class Dark
        {
            public static readonly Color Background      = Color.FromArgb(18, 18, 30);
            public static readonly Color Surface         = Color.FromArgb(30, 30, 46);
            public static readonly Color SurfaceVariant  = Color.FromArgb(40, 40, 60);
            public static readonly Color Sidebar         = Color.FromArgb(22, 22, 38);
            public static readonly Color Primary         = Color.FromArgb(99, 102, 241);   // Indigo-500
            public static readonly Color PrimaryHover    = Color.FromArgb(79,  70, 229);   // Indigo-600
            public static readonly Color PrimaryText     = Color.White;
            public static readonly Color Secondary       = Color.FromArgb(139, 92, 246);   // Violet-500
            public static readonly Color Success         = Color.FromArgb(34, 197, 94);    // Green-500
            public static readonly Color Warning         = Color.FromArgb(234, 179, 8);    // Yellow-500
            public static readonly Color Danger          = Color.FromArgb(239, 68, 68);    // Red-500
            public static readonly Color Info            = Color.FromArgb(14, 165, 233);   // Sky-500
            public static readonly Color TextPrimary     = Color.FromArgb(248, 248, 252);
            public static readonly Color TextSecondary   = Color.FromArgb(148, 163, 184);
            public static readonly Color TextMuted       = Color.FromArgb(100, 116, 139);
            public static readonly Color Border          = Color.FromArgb(55, 55, 80);
            public static readonly Color InputBackground = Color.FromArgb(35, 35, 55);
            public static readonly Color CardBackground  = Color.FromArgb(28, 28, 44);
            public static readonly Color GridHeader      = Color.FromArgb(40, 40, 62);
            public static readonly Color GridRow         = Color.FromArgb(30, 30, 46);
            public static readonly Color GridRowAlt      = Color.FromArgb(34, 34, 52);
            public static readonly Color GridSelection   = Color.FromArgb(99, 102, 241, 80);
        }

        // ─── Light Mode Palette ───────────────────────────────────────────────────
        public static class Light
        {
            public static readonly Color Background      = Color.FromArgb(245, 247, 250);
            public static readonly Color Surface         = Color.White;
            public static readonly Color SurfaceVariant  = Color.FromArgb(235, 237, 245);
            public static readonly Color Sidebar         = Color.FromArgb(30, 30, 46);
            public static readonly Color Primary         = Color.FromArgb(99, 102, 241);
            public static readonly Color PrimaryHover    = Color.FromArgb(79, 70, 229);
            public static readonly Color PrimaryText     = Color.White;
            public static readonly Color Secondary       = Color.FromArgb(139, 92, 246);
            public static readonly Color Success         = Color.FromArgb(22, 163, 74);
            public static readonly Color Warning         = Color.FromArgb(202, 138, 4);
            public static readonly Color Danger          = Color.FromArgb(220, 38, 38);
            public static readonly Color Info            = Color.FromArgb(2, 132, 199);
            public static readonly Color TextPrimary     = Color.FromArgb(15, 23, 42);
            public static readonly Color TextSecondary   = Color.FromArgb(71, 85, 105);
            public static readonly Color TextMuted       = Color.FromArgb(148, 163, 184);
            public static readonly Color Border          = Color.FromArgb(203, 213, 225);
            public static readonly Color InputBackground = Color.White;
            public static readonly Color CardBackground  = Color.White;
            public static readonly Color GridHeader      = Color.FromArgb(241, 245, 249);
            public static readonly Color GridRow         = Color.White;
            public static readonly Color GridRowAlt      = Color.FromArgb(248, 250, 252);
            public static readonly Color GridSelection   = Color.FromArgb(99, 102, 241, 60);
        }

        // ─── Current Palette Accessors ────────────────────────────────────────────
        public static Color Background      => IsDarkMode ? Dark.Background      : Light.Background;
        public static Color Surface         => IsDarkMode ? Dark.Surface         : Light.Surface;
        public static Color SurfaceVariant  => IsDarkMode ? Dark.SurfaceVariant  : Light.SurfaceVariant;
        public static Color Sidebar         => IsDarkMode ? Dark.Sidebar         : Light.Sidebar;
        public static Color Primary         => IsDarkMode ? Dark.Primary         : Light.Primary;
        public static Color PrimaryHover    => IsDarkMode ? Dark.PrimaryHover    : Light.PrimaryHover;
        public static Color Success         => IsDarkMode ? Dark.Success         : Light.Success;
        public static Color Warning         => IsDarkMode ? Dark.Warning         : Light.Warning;
        public static Color Danger          => IsDarkMode ? Dark.Danger          : Light.Danger;
        public static Color Info            => IsDarkMode ? Dark.Info            : Light.Info;
        public static Color TextPrimary     => IsDarkMode ? Dark.TextPrimary     : Light.TextPrimary;
        public static Color TextSecondary   => IsDarkMode ? Dark.TextSecondary   : Light.TextSecondary;
        public static Color TextMuted       => IsDarkMode ? Dark.TextMuted       : Light.TextMuted;
        public static Color Border          => IsDarkMode ? Dark.Border          : Light.Border;
        public static Color InputBackground => IsDarkMode ? Dark.InputBackground : Light.InputBackground;
        public static Color CardBackground  => IsDarkMode ? Dark.CardBackground  : Light.CardBackground;
        public static Color GridHeader      => IsDarkMode ? Dark.GridHeader      : Light.GridHeader;
        public static Color GridRow         => IsDarkMode ? Dark.GridRow         : Light.GridRow;
        public static Color GridRowAlt      => IsDarkMode ? Dark.GridRowAlt      : Light.GridRowAlt;

        // ─── Fonts ────────────────────────────────────────────────────────────────
        public static readonly Font FontTitle    = new Font("Segoe UI", 22f, FontStyle.Bold);
        public static readonly Font FontSubtitle = new Font("Segoe UI", 13f, FontStyle.Regular);
        public static readonly Font FontBody     = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font FontSmall    = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        public static readonly Font FontBold     = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static readonly Font FontButton   = new Font("Segoe UI", 10f, FontStyle.Bold);
        public static readonly Font FontSidebar  = new Font("Segoe UI", 11f, FontStyle.Regular);
        public static readonly Font FontCard     = new Font("Segoe UI", 28f, FontStyle.Bold);
        public static readonly Font FontCardSub  = new Font("Segoe UI", 10f, FontStyle.Regular);
        public static readonly Font FontGrid     = new Font("Segoe UI", 9.5f, FontStyle.Regular);

        // ─── Toggle ───────────────────────────────────────────────────────────────
        public static void ToggleTheme() => IsDarkMode = !IsDarkMode;
        public static void SetDark()     => IsDarkMode = true;
        public static void SetLight()    => IsDarkMode = false;

        // ─── Apply Theme ──────────────────────────────────────────────────────────
        /// <summary>Recursively applies the current theme to a form and all its controls.</summary>
        public static void ApplyTheme(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = TextPrimary;
            form.Font      = FontBody;
            ApplyToControls(form.Controls);
        }

        private static void ApplyToControls(Control.ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                switch (ctrl)
                {
                    case Panel p:
                        p.BackColor = Surface;
                        p.ForeColor = TextPrimary;
                        break;
                    case Label lbl:
                        lbl.ForeColor = TextPrimary;
                        lbl.BackColor = Color.Transparent;
                        break;
                    case TextBox tb:
                        tb.BackColor = InputBackground;
                        tb.ForeColor = TextPrimary;
                        tb.BorderStyle = BorderStyle.FixedSingle;
                        break;
                    case ComboBox cb:
                        cb.BackColor = InputBackground;
                        cb.ForeColor = TextPrimary;
                        break;
                    case DataGridView dgv:
                        StyleDataGridView(dgv);
                        break;
                    case Button btn when btn.Tag?.ToString() == "primary":
                        StylePrimaryButton(btn);
                        break;
                    case Button btn when btn.Tag?.ToString() == "danger":
                        StyleDangerButton(btn);
                        break;
                    case Button btn when btn.Tag?.ToString() == "secondary":
                        StyleSecondaryButton(btn);
                        break;
                    case CheckBox chk:
                        chk.ForeColor = TextSecondary;
                        chk.BackColor = Color.Transparent;
                        break;
                    case GroupBox gb:
                        gb.ForeColor = TextSecondary;
                        gb.BackColor = Color.Transparent;
                        break;
                    case DateTimePicker dtp:
                        dtp.CalendarForeColor  = TextPrimary;
                        dtp.CalendarMonthBackground = Surface;
                        break;
                }

                if (ctrl.HasChildren)
                    ApplyToControls(ctrl.Controls);
            }
        }

        // ─── Control Stylers ──────────────────────────────────────────────────────

        public static void StylePrimaryButton(Button btn)
        {
            btn.BackColor  = Primary;
            btn.ForeColor  = Color.White;
            btn.FlatStyle  = FlatStyle.Flat;
            btn.Font       = FontButton;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor     = Cursors.Hand;
            btn.MouseEnter += (s, e) => btn.BackColor = PrimaryHover;
            btn.MouseLeave += (s, e) => btn.BackColor = Primary;
        }

        public static void StyleDangerButton(Button btn)
        {
            btn.BackColor  = Danger;
            btn.ForeColor  = Color.White;
            btn.FlatStyle  = FlatStyle.Flat;
            btn.Font       = FontButton;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor     = Cursors.Hand;
        }

        public static void StyleSecondaryButton(Button btn)
        {
            btn.BackColor  = SurfaceVariant;
            btn.ForeColor  = TextPrimary;
            btn.FlatStyle  = FlatStyle.Flat;
            btn.Font       = FontButton;
            btn.FlatAppearance.BorderColor = Border;
            btn.FlatAppearance.BorderSize  = 1;
            btn.Cursor     = Cursors.Hand;
        }

        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor         = Surface;
            dgv.GridColor               = Border;
            dgv.BorderStyle             = BorderStyle.None;
            dgv.RowHeadersVisible       = false;
            dgv.SelectionMode           = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode     = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows      = false;
            dgv.AllowUserToDeleteRows   = false;
            dgv.ReadOnly                = true;
            dgv.CellBorderStyle         = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.Font                    = FontGrid;
            dgv.RowTemplate.Height      = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = GridRowAlt;
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;
            dgv.DefaultCellStyle.BackColor  = GridRow;
            dgv.DefaultCellStyle.ForeColor  = TextPrimary;
            dgv.DefaultCellStyle.SelectionBackColor = IsDarkMode ? Dark.Primary : Light.Primary;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding    = new Padding(4, 0, 4, 0);
            dgv.ColumnHeadersDefaultCellStyle.BackColor  = GridHeader;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = TextSecondary;
            dgv.ColumnHeadersDefaultCellStyle.Font       = FontBold;
            dgv.ColumnHeadersDefaultCellStyle.Padding    = new Padding(4, 8, 4, 8);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
        }
    }
}
