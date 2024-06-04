using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VehiDenceAPI.Models
{
    public class Vigneta
    {
        public int Id { get; set; }
        public string NrInmatriculare { get; set; } = null!;
        public DateTime DataCreare { get; set; }
        public DateTime DataExpirare { get; set; }
        public int IsValid { get; set; }

        public string Tara { get; set; } = null!;
        [BindNever]
        public byte[]? ImageData { get; set; }

        public Vigneta(int id, string nrInmatriculare, DateTime dataCreare, DateTime dataExpirare,  string tara, byte[]? imageData,int isvalid)
        {
            Id = id;
            NrInmatriculare = nrInmatriculare;
            DataCreare = dataCreare;
            DataExpirare = dataExpirare;
            Tara = tara;
            ImageData = imageData;
            IsValid = isvalid;
        }

        public Vigneta(string nrInmatriculare)
        {
            NrInmatriculare = nrInmatriculare;
        }

        public Vigneta() { }
    }
}
