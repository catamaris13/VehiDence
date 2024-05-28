﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VehiDenceAPI.Models
{
    public class Casco
    {
        public int Id { get; set; }
        public string NrInmatriculare { get; set; }
        public DateTime DataCreare { get; set; }
        public DateTime DataExpirare { get; set; }
        public string Asigurator { get; set; }
        public int IsValid { get; set; }

        [BindNever]
        public byte[]? ImageData { get; set; }
    }
}
