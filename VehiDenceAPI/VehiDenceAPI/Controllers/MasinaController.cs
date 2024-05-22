using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasinaController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MasinaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        [Route("AddMasina")]
        public Response AddMasina([FromForm] Masina masina, IFormFile? imageFile)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();

            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    masina.ImageData = ms.ToArray();
                }
            }

            response = dal.AddMasina(masina, connection);
            return response;
        }

        [HttpGet]
        [Route("MasinaList/{username}")]
        public Response MasinaList(string username)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            Masina masina = new Masina();
            masina.Username = username;
            response = dal.MasinaList(masina, connection);

            return response;

        }
        [HttpDelete]
        [Route("DeleteMasinaa")]
        public Response DeleteMasina(Masina masina)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeleteMasina(masina, connection);

            return response;
        }
    }
}
