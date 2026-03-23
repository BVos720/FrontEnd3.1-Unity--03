

using System;
using UnityEditor;

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

        public string Type { get; set; }

        public DateTime Datum { get; set; }

        public string Arts { get; set; }
    }
}
