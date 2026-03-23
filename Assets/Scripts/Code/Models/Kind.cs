
using System;

namespace MySecureBackend.WebApi.Models
{
    public class Kind
    {
        public string Naam { get; set; }

        public int Leeftijd { get; set; }

        public Guid OuderID { get; set; }
    }
}
