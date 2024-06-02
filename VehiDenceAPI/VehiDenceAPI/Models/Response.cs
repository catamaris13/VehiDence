namespace VehiDenceAPI.Models
{
    public class Response
    {
        public int StatusCode {  get; set; }
        public string StatusMessage { get; set; } = null!;
        public Users User { get; set; } = null!;
        public Dictionary<string, int> UserDaysUntilExpiration { get; set; } = null!;
        public Asigurare Asigurare { get; set; } = null!;
        public List<Masina> ListMasina { get; set; } = null!;
        public List<Asigurare> ListAsigurare { get; set;} = null!;
        public List<Users> ListUsers { get; set; } = null!;
        public List<Casco> ListCasco { get; set; } = null!;
        public List<ITP> ListITP { get; set; } = null!;
        public List<PermisConducere> ListPermisConducere { get; set; } = null!;
        public List<RevizieService> ListRevizieService { get; set; } = null!;
        public List<Vigneta> ListVigneta { get; set; } = null!;

        public Response(int statusCode, string statusMessage)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }

        public Response(int statusCode, string statusMessage, List<Users> listUsers, Dictionary<string, int> userDaysUntilExpiration)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListUsers = listUsers;
            UserDaysUntilExpiration = userDaysUntilExpiration;
        }

        public Response(int statusCode, string statusMessage, Users user)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            User = user;
        }

        public Response(int statusCode, string statusMessage, List<Users> listUsers)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListUsers = listUsers;
        }

        public Response(int statusCode, string statusMessage, List<Masina> listMasina)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListMasina = listMasina;
        }

        public Response(int statusCode, string statusMessage, List<Asigurare> listAsigurare)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListAsigurare = listAsigurare;
        }

        public Response(int statusCode, string statusMessage, List<Casco> listCasco)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListCasco = listCasco;
        }

        public Response(int statusCode, string statusMessage, List<ITP> listITP)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListITP = listITP;
        }

        public Response(int statusCode, string statusMessage, List<PermisConducere> listPermisConducere)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListPermisConducere = listPermisConducere;
        }

        public Response(int statusCode, string statusMessage, List<RevizieService> listRevizieService)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListRevizieService = listRevizieService;
        }

        public Response(int statusCode, string statusMessage, List<Vigneta> listVigneta)
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            ListVigneta = listVigneta;
        }

        public Response() { }
    }
}
