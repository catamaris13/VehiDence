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
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    vigneta.ImageData = ms.ToArray();
                }
            }

            response = dal.AddVigneta(vigneta, connection);
            return response;
        }
        [HttpDelete]
        [Route("DeleteVigneta")]

        public Response DeleteVigneta(Vigneta vigneta)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeleteVigneta(vigneta, connection);

            return response;
        }
        [HttpGet]
        [Route("VignetaList/{nrInmatriculare}")]
        public Response RevizieServiceList(string nrInmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            Vigneta vg = new Vigneta();
            vg.NrInmatriculare = nrInmatriculare;
            response = dal.VignetaList(vg, connection);

            return response;

        }
        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.VerificareExpirareVigneta(connection);
            RecurringJob.AddOrUpdate("Verificare Vigneta", () => SendExpirationReminder(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {
                string subject = "Expirare Vigneta";

                foreach (Users user in response.listUsers)
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

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.ExpirareAsigurare(connection);
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareVigneta(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {



                string subject = "Inssurance Expired";

                foreach (Users user in response.listUsers)
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
