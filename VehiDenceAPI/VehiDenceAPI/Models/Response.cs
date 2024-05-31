using Microsoft.EntityFrameworkCore;

namespace VehiDenceAPI.Models
{
    public class Response
    {
        public int StatusCode {  get; set; }    
        public string StatusMessage { get; set; }
        public Users User { get; set; }
        public Dictionary<string, int> UserDaysUntilExpiration { get; set; }
        public Asigurare Asigurare { get; set; }  
        public List<Masina> listMasina { get; set; }
        public List<Asigurare> listAsigurare { get; set;}
        public List<Users> listUsers { get; set; } 
        public List<Casco> listCasco { get; set; }
        public List<ITP> listITP { get; set; }  
        public List<PermisConducere> listPermisConducere { get; set; }
        public List<RevizieService> listRevizieService { get; set; }
        public List<Vigneta> listVigneta { get; set; }


    }
}
