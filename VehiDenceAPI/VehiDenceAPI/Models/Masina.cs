using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VehiDenceAPI.Models
{
    public class Masina
    {
        public int Id { get; set; }
        public string SerieSasiu { get; set; } = null!;
        public string NrInmatriculare { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Username { get; set; } = null!;
        [BindNever]
        public byte[]? ImageData { get; set; }  

        public Masina(int id, string serieSasiu, string nrInmatriculare, string marca, string model, string username, byte[]? imageData)
        {
            Id = id;
            SerieSasiu = serieSasiu;
            NrInmatriculare = nrInmatriculare;
            Marca = marca;
            Model = model;
            Username = username;
            ImageData = imageData;
        }

        public Masina(string username)
        {
            Username = username;
        }

        public Masina(int id)
        {
            Id = id;
        }

        public Masina() { }
    }
}
