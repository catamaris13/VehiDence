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
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.AddItp(itp, connection);
            


            return response;
        }
        [HttpDelete]
        [Route("DeleteITP")]

        public Response DeleteITP(ITP itp)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.DeleteITP(itp, connection);

            return response;
        }
        [HttpGet]
        [Route("ITPList/{nrinmatriculare}")]
        public Response ITPList(string nrinmatriculare)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            ITP itp = new ITP();
            itp.NrInmatriculare = nrinmatriculare;
            response = dal.ITPList(itp, connection);

            return response;

        }
        [HttpPost]
        [Route("SendExpirationReminder")]
        public async Task<IActionResult> SendExpirationReminder()
        {

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.VerificareExpirareITP(connection);
            RecurringJob.AddOrUpdate("Verificare ITP", () => SendExpirationReminder(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {
                string subject = "Expirare ITP";

                foreach (Users user in response.listUsers)
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

            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            Dal dal = new Dal();
            response = dal.ExpirareAsigurare(connection);
            RecurringJob.AddOrUpdate("Verificare asigurare", () => ExpirareITP(), "0 0 * * *");
            //Console.WriteLine(response.ToString());

            if (response.StatusCode == 200)
            {



                string subject = "Inssurance Expired";

                foreach (Users user in response.listUsers)
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
