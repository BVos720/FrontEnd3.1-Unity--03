

using System;

namespace MySecureBackend.WebApi.Models
{
    public class Behandeling
    {
        public Behandeling(string type, DateTime datum, string arts)
        {
            Type = type;
            Datum = datum;
            Arts = arts;
        }

        public Guid BehandelingID { get; set; }

        public Guid KindID { get; set; }

        public string Type { get; set; }

        public DateTime Datum { get; set; }

        public string Arts { get; set; }
    }
}
