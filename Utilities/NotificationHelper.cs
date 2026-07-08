using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudentAttendanceSysttem.Utilities
{
    /// <summary>
    /// Provides styled, reusable notification dialogs and toast-style banners.
    /// All methods are static — call from any form without instantiation.
    /// </summary>
    public static class NotificationHelper
    {
        // ─── MessageBox Wrappers ──────────────────────────────────────────────────

        public static void ShowSuccess(string message, string title = "Success") =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static void ShowError(string message, string title = "Error") =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ShowWarning(string message, string title = "Warning") =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);

        public static void ShowInfo(string message, string title = "Information") =>
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static bool Confirm(string message, string title = "Confirm") =>
            MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes;

        // ─── Inline Status Banner ─────────────────────────────────────────────────

        /// <summary>
        /// Shows a temporary colored status label on a form for 3 seconds, then hides it.
        /// The label must already exist on the form — just pass its reference.
        /// </summary>
        public static void ShowBanner(Label banner, string message, NotificationType type = NotificationType.Info)
        {
            banner.Text      = message;
            banner.BackColor = GetBannerColor(type);
            banner.ForeColor = Color.White;
            banner.Visible   = true;

            var timer = new System.Windows.Forms.Timer { Interval = 3000 };
            timer.Tick += (s, e) =>
            {
                banner.Visible = false;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        private static Color GetBannerColor(NotificationType type) => type switch
        {
            NotificationType.Success => ThemeManager.Success,
            NotificationType.Warning => ThemeManager.Warning,
            NotificationType.Error   => ThemeManager.Danger,
            _                        => ThemeManager.Info
        };
    }

    public enum NotificationType { Info, Success, Warning, Error }
}
