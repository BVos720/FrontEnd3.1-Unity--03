using System;

namespace MySecureBackend.WebApi.Models
{
    public class SettingsData
    {
        public SettingsData(int character, int colorTheme)
        {
            Character = character;
            ColorTheme = colorTheme;
        }

        public SettingsData(Guid settingsID, int character, int colorTheme)
        {
            this.SettingsID = settingsID;
            Character = character;
            ColorTheme = colorTheme;
        }

        public Guid SettingsID { get; set; }

        public int Character { get; set; }

        public int ColorTheme { get; set; }

        public Guid KindID { get; set; }
    }
}
