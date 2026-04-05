using System;
using Newtonsoft.Json;

namespace MySecureBackend.WebApi.Models
{
    public class SettingsData
    {
        public SettingsData() { }

        public SettingsData(int character, int colorTheme, int taal = 0)
        {
            Character = character;
            ColorTheme = colorTheme;
            Taal = taal;
        }

        public SettingsData(Guid settingsID, int character, int colorTheme, int taal = 0)
        {
            this.SettingsID = settingsID;
            Character = character;
            ColorTheme = colorTheme;
            Taal = taal;
        }

        public Guid SettingsID { get; set; }

        public int Character { get; set; }

        public int ColorTheme { get; set; }

        public int Taal { get; set; }

        public Guid KindID { get; set; }
    }
}
