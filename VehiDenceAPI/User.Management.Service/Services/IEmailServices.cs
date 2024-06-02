namespace User.Management.Service.Services
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string email,string subiect,string message);
    }
}
