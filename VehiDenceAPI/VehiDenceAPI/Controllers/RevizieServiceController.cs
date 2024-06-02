using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using User.Management.Service.Services;
using VehiDenceAPI.Models;
using VehiDenceAPI.Services;

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
            return new RevizieServiceService().AddRevizieService(revizieService,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteRevizieService")]
        public Response DeleteRevizieService(RevizieService revizieService)
        {
            return new RevizieServiceService().DeleteRevizieService(revizieService,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("RevizieServiceList/{serieSasiu}")]
        public Response RevizieServiceList(string serieSasiu)
        {
            return new RevizieServiceService().RevizieServiceList(new RevizieService(serieSasiu),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }
    }
}
