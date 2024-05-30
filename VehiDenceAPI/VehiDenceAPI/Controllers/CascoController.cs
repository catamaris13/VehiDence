using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using User.Management.Service.Services;
using VehiDenceAPI.Models;

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

        public Response AddCasco([FromForm] Casco casco,IFormFile? imageFile)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    casco.ImageData = ms.ToArray();
                }
            }

            response = dal.AddCasco(casco, connection);
            return response;

            
        }
        [HttpDelete]
        [Route("DeleteCasco")]

        public Response DeleteCaco(Casco casco)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeleteCasco(casco, connection);

            return response;
        }
        [HttpGet]
        [Route("CascoList/{nrinmatriculare}")]
        public Response CascoList(string nrinmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            Casco casco = new Casco();
            casco.NrInmatriculare = nrinmatriculare;
            response = dal.CascoList(casco, connection);

            return response;

        }
        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.VerificareExpirareCasco(connection);
            RecurringJob.AddOrUpdate("Verificare Casco", () => SendExpirationReminder(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {
                string subject = "Expirare Casco";

                foreach (Users user in response.listUsers)
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
        public async Task<IActionResult> ExpirareCoasco()
        {

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.ExpirareAsigurare(connection);
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareCoasco(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {



                string subject = "Inssurance Expired";

                foreach (Users user in response.listUsers)
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
