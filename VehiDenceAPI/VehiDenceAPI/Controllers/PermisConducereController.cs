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
    public class PermisConducereController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public PermisConducereController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("AddPermis")]
        public Response AddPermis([FromForm] PermisConducere pc, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    pc.ImageData = ms.ToArray();
                }
            }
            return new PermisConducereServices().AddPermisConducere(pc,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeletePermisConducere")]
        public Response DeletePermisConducere([FromForm] PermisConducere pc)
        {
            return new PermisConducereServices().DeletePermisConducere(pc,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("PermisList/{username}")]
        public Response PermisConducereList(string username)
        {
            return new PermisConducereServices().PermisConducereList(new PermisConducere(username),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {
            Response response = new PermisConducereServices().VerificareExpirarePermisConducere(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare ITP", () => SendExpirationReminder(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Expirare Permis Conducere";
                foreach (Users user in response.ListUsers)
                {
                    int daysUntilExpiration = response.UserDaysUntilExpiration[user.Email];
                    string message = $"Hi {user.Name}! " +
                        $"Your Permis Conducere will expire in {daysUntilExpiration} days from now !" +
                        $"Don't forget to make an appointment to renew it!";
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
        [Route("ExpirarePermisConducere")]
        public async Task<IActionResult> ExpirarePermisConducere()
        {
            Response response = new PermisConducereServices().ExpirarePermisConducere(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirarePermisConducere(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Inssurance Expired";
                foreach (Users user in response.ListUsers)
                {
                    string message = $"Hi {user.Name}! " +
                        $"Your Permis Conducere has expired today !" +
                        $"Don't forget to make an appointment to renew it!";
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