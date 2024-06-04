namespace VehiDenceAPI.Models
{
    public class RevizieService
    {
        public int Id { get; set; }
        public string SerieSasiu { get; set; } = null!;
        public int KmUltim {  get; set; }
        public int KmExpirare { get; set; }
        public string ServiceName { get; set; } = null!;
        public int IsValid { get; set; }

        public RevizieService(int id, string serieSasiu, int kmUltim, int kmExpirare, string serviceName,int isvalid)
        {
            Id = id;
            SerieSasiu = serieSasiu;
            KmUltim = kmUltim;
            KmExpirare = kmExpirare;
            ServiceName = serviceName;
            IsValid = isvalid;  
        }

        public RevizieService(string serieSasiu)
        {
            SerieSasiu = serieSasiu;
        }

        public RevizieService() { }
    }
}
