using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using User.Management.Service.Services;
using VehiDenceAPI.Models;
using VehiDenceAPI.Services;

namespace VehiDenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsigurareController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public AsigurareController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("AddAsigurare")]
        public Response AddAsigurare([FromForm] Asigurare asigurare, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    asigurare.ImageData = ms.ToArray();
                }
            }
            return new AsigurareService().AddAsigurare(asigurare,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("DeleteAsigurare")]
        public Response DeleteAsigurare([FromForm] Asigurare asigurare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            AsigurareService dal = new AsigurareService();
            response = dal.DeleteAsigurare(asigurare, connection);

            return response;
        }

        [HttpGet]
        [Route("AsigurareList/{nrinmatriculare}")]
        public Response AsigutareList(string nrinmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            AsigurareService dal = new AsigurareService();
            Asigurare asigurare = new Asigurare();
            asigurare.NrInmatriculare = nrinmatriculare;
            response = dal.AsigurareList(asigurare, connection);

            return response;

        }

        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder7()
        {
            Response response = new AsigurareService().VerificareExpirareAsigurareAvans(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => SendExpirationReminder7(), "0 0 * * *");
                if (response.StatusCode == 200)
                {
                    string subject = "Expirare Asigurare";
                    foreach (Users user in response.ListUsers)
                    {
                        int daysUntilExpiration = response.UserDaysUntilExpiration[user.Email];
                        string message = $"Hi {user.Name}! " +
                        $"Your insurance will expire in {daysUntilExpiration} days from now !" +
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
        [Route("ExpirareAsigurare")]
        public async Task<IActionResult> ExpirareAsigurare()
        {
            Response response = new AsigurareService().ExpirareAsigurare(
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareAsigurare(), "0 0 * * *");
            if (response.StatusCode == 200)
            {
                string subject = "Inssurance Expired";
                foreach (Users user in response.ListUsers)
                {
                    string message = $"Hi {user.Name}! " +
                        $"Your insurance has expired today !" +
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

