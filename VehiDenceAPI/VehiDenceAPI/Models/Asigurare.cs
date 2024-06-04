using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VehiDenceAPI.Models
{
    public class Asigurare
    {
        public int Id { get; set; }
        public string NrInmatriculare { get; set; } = null!;
        public DateTime DataCreare { get; set; }
        public DateTime DataExpirare { get; set; }
        public string Asigurator { get; set; } = null!;
        public int IsValid { get; set; }
        [BindNever]
        public byte[]? ImageData { get; set; }

        public Asigurare(int id, string nrInmatriculare, DateTime dataCreare, DateTime dataExpirare, string asigurator, byte[]? imageData,int isvalid)
        {
            Id = id;
            NrInmatriculare = nrInmatriculare;
            DataCreare = dataCreare;
            DataExpirare = dataExpirare;
            Asigurator = asigurator;
            ImageData = imageData;
            IsValid = isvalid;
        }

        public Asigurare(string nrInmatriculare)
        {
          this.NrInmatriculare = nrInmatriculare;
        }

        public Asigurare()
        { }
    }
}
