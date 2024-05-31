using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using User.Management.Service.Services;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevizieServiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public RevizieServiceController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpPost]
        [Route("AddRevizieService")]

        public Response AddRevizieService(RevizieService revizieService)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.AddRevizieService(revizieService, connection);



            return response;
        }
        [HttpDelete]
        [Route("DeleteRevizieService")]

        public Response DeleteRevizieService(RevizieService revizieService)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeleteRevizieService(revizieService, connection);

            return response;
        }
        [HttpGet]
        [Route("RevizieServiceList/{serieSasiu}")]
        public Response RevizieServiceList(string serieSasiu)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            RevizieService rs = new RevizieService();
            rs.SerieSasiu = serieSasiu;
            response = dal.RevizieServiceList(rs, connection);

            return response;

        }
    }
}
