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
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            if (imageFile != null && imageFile.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    pc.ImageData = ms.ToArray();
                }
            }

            response = dal.AddPermisConducere(pc, connection);
            return response;
        }
        [HttpDelete]
        [Route("DeletePermisConducere")]

        public Response DeletePermisConducere(PermisConducere pc)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeletePermisConducere(pc, connection);

            return response;
        }
        [HttpGet]
        [Route("PermisList/{username}")]
        public Response PermisConducereList(string username)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            PermisConducere pc = new PermisConducere();
            pc.username = username;
            response = dal.PermisConducereList(pc, connection);

            return response;

        }
        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.VerificareExpirarePermisConducere(connection);
            RecurringJob.AddOrUpdate("Verificare ITP", () => SendExpirationReminder(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {
                string subject = "Expirare Permis Conducere";

                foreach (Users user in response.listUsers)
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

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.ExpirareAsigurare(connection);
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirarePermisConducere(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {



                string subject = "Inssurance Expired";

                foreach (Users user in response.listUsers)
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
