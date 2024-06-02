using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VehiDenceAPI.Models;
using VehiDenceAPI.Services;
using IEmailServices = User.Management.Service.Services.IEmailServices;

namespace VehiDenceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public UserController(IConfiguration configuration, IEmailServices emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration(Users user)
        {
            Response response = new Response();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString());
            UserAuthentificationService userAuthentificationService = new UserAuthentificationService(); 
            user.Token= Guid.NewGuid().ToString();
            response = userAuthentificationService.Registration(user, connection);
            Console.WriteLine(user.Token);
            // Send validation email
            if (response.StatusCode == 200)
            {
                //string validationLink = $"http://localhost:5277/api/User/ValidateEmail?username={user.username}&token={user.Token}";
                string validationLink = $"http://localhost:3000/email_validation?username={user.username}&token={user.Token}";
                string subject = "Welcome to our platform!";
                string message = $"Thank you for registering with us, {user.Name}. Please click the following link to validate your email: {validationLink}";
                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, message);   
                    return StatusCode(200,"Registration successful. Please check your email for validation instructions.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Failed to send email: {ex.Message}");
                }
            }
             return StatusCode(500, "Failed to send email");
        }

        [HttpPost]
        [Route("Login")]
        public Response Login(Users user)
        {
            return new UserAuthentificationService().Login(user, 
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpPut]
        [Route("Update")]
        public Response UpdateUser(Users user)
        {
            return new UserServices().UpdateUser(user,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpDelete]
        [Route("Delete")]
        public Response DeleteUser(Users user)
        {
            return new UserServices().DeleteUser(user,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpGet]
        [Route("All/Users")]
        public List<Users> GetUsers()
        {
            return new UserServices().GetUsers(new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }
        
        [HttpGet]
        [Route("SendEmail")]
        public async Task<IActionResult> TestEmail()
        {
            var reciver = "catalinmaris@yahoo.com";
            var subiect = "ceva";
            var mesaj = "Du te ba de aici";
            try
            {
                await _emailService.SendEmailAsync(reciver, subiect, mesaj);
                return Ok("Email sent successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("ValidateEmail")]
        public Response ValidateEmail([FromQuery]string username,[FromQuery] string token)
        {
            return new UserValidations().UserValidation(new Users(username, token), 
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }
        
        [HttpGet]
        [Route("ValidareLinkParola")]
        public Response ValidateEmailForgotPassword([FromQuery] string token)
        {
            return new UserValidations().UserValidationEmail(new Users(token),
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }
        
        [HttpPost]
        [Route("ResetPassword")]
        public Response RessetPassword(Users user)
        {
            return new UserAuthentificationService().ResetPassword(user,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
        }

        [HttpPost]
        [Route("SendEmailPassword")]
        public async Task<IActionResult> SendEmailPassword(Users user)
        {
            user.Token = Guid.NewGuid().ToString();
            Response response = new UserAuthentificationService().ResetToken(user,
                new SqlConnection(_configuration.GetConnectionString("VehiDenceConnectionString").ToString()));
            Console.WriteLine(user.Token);
            if (response.StatusCode == 200)
            {
                //string validationLink = $"http://localhost:5277/api/User/ValidateEmail?username={user.username}&token={user.Token}";
                string resetLink = $"http://localhost:3000/reset_password?token={user.Token}";
                string subject = "Reset Password";
                string message = $"Hi {user.Name}! Please click the following link to reset your password: {resetLink}";
                try
                {
                    await _emailService.SendEmailAsync(user.Email, subject, message);
                    return StatusCode(200, "Email sent successful. Please check your email for resset instructions.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Failed to send email: {ex.Message}");
                }
            }
            return StatusCode(500, "Failed to send email");
        }
    }
}

