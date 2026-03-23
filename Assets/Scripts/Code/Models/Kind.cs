
using System;

namespace MySecureBackend.WebApi.Models
{
    public class Kind
    {
        public Kind(string naam, int leeftijd)
        {
            Naam = naam;
            Leeftijd = leeftijd;
        }

        public Guid KindID { get; set; }

        public Guid BehandelingID { get; set; }

        public string Naam { get; set; }

        public int Leeftijd { get; set; }
    }
}
