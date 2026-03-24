

using System;

namespace MySecureBackend.WebApi.Models
{
    public class GameProgress
    {
        public Guid GameProgressID { get; set; }

        public Guid BehandelingID { get; set; }

        public float LevelProgress { get; set; }

        public int Points { get; set; }
    }
}
