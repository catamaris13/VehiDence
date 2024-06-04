using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VehiDenceAPI.Models;
using VehiDenceAPI.Services;

namespace VehiDenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class  MasinaController : ControllerBase
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
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    masina.ImageData = ms.ToArray();
                }
            }
            return new MasinaService().AddMasina(masina, 
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("MasinaList/{username}")]
        public Response MasinaListUsername(string username)
        {
            return new MasinaService().MasinaListUsername(new Masina(username),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("MasinaList/{id:int}")]
        public Response MasinaListId(int id)
        {
            return new MasinaService().MasinaListId(new Masina(id),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteMasinaa")]
        public Response DeleteMasina([FromForm] Masina masina)
        {
            return new MasinaService().DeleteMasina(masina, 
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }
    }
}
