using System.Configuration;

namespace StudentAttendanceSysttem.Properties
{
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal sealed partial class Settings : ApplicationSettingsBase
    {
        private static readonly Settings defaultInstance =
            (Settings)Synchronized(new Settings());

        public static Settings Default => defaultInstance;

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool RememberMe
        {
            get => (bool)this["RememberMe"];
            set => this["RememberMe"] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("")]
        public string SavedUsername
        {
            get => (string)this["SavedUsername"];
            set => this["SavedUsername"] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("Dark")]
        public string Theme
        {
            get => (string)this["Theme"];
            set => this["Theme"] = value;
        }
    }
}
