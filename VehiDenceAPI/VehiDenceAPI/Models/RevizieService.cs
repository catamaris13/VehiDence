namespace VehiDenceAPI.Models
{
    public class RevizieService
    {
        public int Id { get; set; }
        public string SerieSasiu { get; set; }

        public int KmUltim {  get; set; }
        public int KmExpirare { get; set; }
        public string ServiceName { get; set; }
        public int IsValid { get; set; }

    }
}
