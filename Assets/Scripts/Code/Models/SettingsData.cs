using System;
using Newtonsoft.Json;

namespace MySecureBackend.WebApi.Models
{
    [Serializable]
    public class SettingsData
    {
        public SettingsData() { }

        public SettingsData(int character, int colorTheme, int taal = 0)
        {
            Character = character;
            ColorTheme = colorTheme;
            Taal = taal;
        }

        public Guid SettingsID { get; set; }
        public int Character { get; set; }
        public int ColorTheme { get; set; }
        public Guid KindID { get; set; }

      
        [JsonProperty("taal")]
        [JsonConverter(typeof(SafeIntConverter))]
        public int Taal { get; set; }
    }

   
    public class SafeIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(int);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            if (reader.TokenType == JsonToken.Integer)
                return Convert.ToInt32(reader.Value);

            return 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}