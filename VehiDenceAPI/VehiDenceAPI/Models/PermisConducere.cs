using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.CompilerServices;

namespace VehiDenceAPI.Models
{
    public class PermisConducere
    {
        public int Id { get; set; }
        public string Nume { get; set; } = null!;
        public string username { get; set; } = null!;
        public DateTime DataCreare { get; set; }
        public DateTime DataExpirare { get; set; }
        public int IsValid { get; set; }
        public string Categorie { get; set; } = null!;
        [BindNever]
        public byte[]? ImageData { get; set; }

        public PermisConducere(int id, string nume, string username, DateTime dataCreare, DateTime dataExpirare, string categorie, byte[]? imageData,int isvalid)
        {
            Id = id;
            Nume = nume;
            this.username = username;
            DataCreare = dataCreare;
            DataExpirare = dataExpirare;
            Categorie = categorie;
            ImageData = imageData;
            IsValid = isvalid;
        }

        public PermisConducere(string username)
        {
            this.username = username;
        }

        public PermisConducere() { }
    }
}
