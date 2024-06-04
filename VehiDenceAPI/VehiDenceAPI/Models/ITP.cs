namespace VehiDenceAPI.Models
{
    public class ITP
    {
        public int Id { get; set; }
        public string NrInmatriculare { get; set; } = null!;
        public DateTime DataCreare { get; set; }
        public DateTime DataExpirare { get; set; }
        public int IsValid { get; set; }

        public ITP(int id, string nrInmatriculare, DateTime dataCreare, DateTime dataExpirare,int isvalid)
        {
            Id = id;
            NrInmatriculare = nrInmatriculare;
            DataCreare = dataCreare;
            DataExpirare = dataExpirare;
            IsValid=isvalid;
        }

        public ITP(string nrInmatriculare)
        {
            NrInmatriculare = nrInmatriculare;
        }

        public ITP() { }
    }
}
