namespace VehiDenceAPI.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string username { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public bool IsValid { get; set; } 
        public string Token { get; set; } = null!;

        public Users(int id, string name, string email, string username)
        {
            Id = id;
            Name = name;
            Email = email;
            this.username = username;
        }

        public Users(string username, string token)
        {
            this.username = username;
            Token = token;
        }

        public Users(string name, string password, string email, string username, string phoneNo)
        {
            Name = name;
            Password = password;
            Email = email;
            this.username = username;
            PhoneNo = phoneNo;
        }

        public Users(string token)
        {
            Token = token;
        }

        public Users() { }
    }
}
