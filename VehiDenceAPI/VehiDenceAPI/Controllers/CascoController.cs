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
    public class CascoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public CascoController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("AddCasco")]
        public Response AddCasco([FromForm] Casco casco, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    casco.ImageData = ms.ToArray();
                }
            }
            return new CascoService().AddCasco(casco,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteCasco")]
        public Response DeleteCaco([FromForm]Casco casco)
        {
            return new CascoService().DeleteCasco(casco,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("CascoList/{nrinmatriculare}")]
        public Response CascoList(string nrInmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            CascoService dal = new CascoService();
            Casco casco = new Casco();
            casco.NrInmatriculare = nrInmatriculare;
            response = dal.CascoList(casco, connection);

            return response;
        }

        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {
            Response response = new CascoService().VerificareExpirareCasco(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare Casco", () => SendExpirationReminder(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Expirare Casco";
                foreach (Users user in response.ListUsers)
                {
                    int daysUntilExpiration = response.UserDaysUntilExpiration[user.Email];
                    string message = $"Hi {user.Name}! " +
                        $"Your Casco will expire in {daysUntilExpiration} days from now !" +
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

        [HttpPost]
        [Route("ExpirareCoasco")]
        public async Task<IActionResult> ExpirareCasco()
        {
            Response response = new CascoService().ExpirareCasco(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareCasco(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Inssurance Expired";
                foreach (Users user in response.ListUsers)
                {
                    string message = $"Hi {user.Name}! " +
                        $"Your casco has expired today !" +
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
