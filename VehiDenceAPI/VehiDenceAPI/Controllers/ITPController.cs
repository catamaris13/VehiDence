
using Hangfire;
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
    public class ITPController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public ITPController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("AddItp")]
        public Response AddItp([FromForm] ITP itp)
        {
            return new ITPServices().AddITP(itp,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteITP")]
        public Response DeleteITP(ITP itp)
        {
            return new ITPServices().DeleteITP(itp,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("ITPList/{nrinmatriculare}")]
        public Response ITPList(string nrInmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            ITPServices dal = new ITPServices();
            ITP itp = new ITP();
            itp.NrInmatriculare = nrInmatriculare;
            response = dal.ITPList(itp, connection);

            return response;
        }

        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {
            Response response = new ITPServices().VerificareExpirareITP(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare ITP", () => SendExpirationReminder(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Expirare ITP";
                foreach (Users user in response.ListUsers)
                {
                    int daysUntilExpiration = response.UserDaysUntilExpiration[user.Email];
                    string message = $"Hi {user.Name}! " +
                        $"Your ITP will expire in {daysUntilExpiration} days from now !" +
                        $"Don't forget to get in touch with your service!";
                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, message);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Failed to send email: {ex.Message}");
                    }
                }
                return StatusCode(200, "Email sent successful. Please check your email for resset instructions.");
            }
            return StatusCode(500, "Failed to send email");
        }

        [HttpPost]
        [Route("ExpirareITP")]
        public async Task<IActionResult> ExpirareITP()
        {
            Response response = new ITPServices().ExpirareITP(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareITP(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "ITP Expired";
                foreach (Users user in response.ListUsers)
                {
                    string message = $"Hi {user.Name}! " +
                        $"Your ITP has expired today !" +
                        $"Don't forget to get in touch with your inssurance company!";
                    try
                    {
                        await _emailService.SendEmailAsync(user.Email, subject, message);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, $"Failed to send email: {ex.Message}");
                    }
                }
                return StatusCode(200, "Email sent successful. Please check your email for resset instructions.");
            }
            return StatusCode(500, "Failed to send email");
        }
    }
}
