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
    public class VignetaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public VignetaController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("AddVigneta")]
        public Response AddVigneta([FromForm] Vigneta vigneta, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    vigneta.ImageData = ms.ToArray();
                }
            }
            return new VignetaService().AddVigneta(vigneta,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteVigneta")]
        public Response DeleteVigneta(Vigneta vigneta)
        {
            return new VignetaService().DeleteVigneta(vigneta,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("VignetaList/{nrInmatriculare}")]
        public Response RevizieServiceList(string nrInmatriculare)
        {
            return new VignetaService().VignetaList(new Vigneta(nrInmatriculare),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {
            Response response = new VignetaService().VerificareExpirareVigneta(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare Vigneta", () => SendExpirationReminder(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Expirare Vigneta";
                foreach (Users user in response.ListUsers)
                {
                    int daysUntilExpiration = response.UserDaysUntilExpiration[user.Email];
                    string message = $"Hi {user.Name}! " +
                        $"Your Vigneta will expire in {daysUntilExpiration} days from now !" +
                        $"Don't forget to renew it!";
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
        [Route("ExpirareVigneta")]
        public async Task<IActionResult> ExpirareVigneta()
        {
            Response response = new VignetaService().ExpirareVigneta(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareVigneta(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Vigneta Expired";
                foreach (Users user in response.ListUsers)
                {
                    string message = $"Hi {user.Name}! " +
                        $"Your Vigneta has expired today !" +
                        $"Don't forget to renew it!";
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