

using System;
using UnityEditor;

namespace MySecureBackend.WebApi.Models
{
    public class Behandeling
    {
        public string Type { get; set; }

        public DateTime Datum { get; set; }

        public string Arts { get; set; }
    }
}
