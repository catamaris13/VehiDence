namespace User.Management.Service.Models
{
    public class EmailConfiguration
    {
        public string From {  get; private set; } = null!; 
        public string StmpServer { get; private set; } = null!;
        public int Port { get; private set; }
        public string UserName { get; private set; } = null!;
        public string Password { get; private set; } = null!;
    }
}
