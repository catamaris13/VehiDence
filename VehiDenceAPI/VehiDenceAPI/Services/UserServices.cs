using Microsoft.Data.SqlClient;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class UserServices
    {
        public Response UpdateUser(Users user, SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE Users SET Name = @Name, Password = @Password, username = @Username, PhoneNo = @PhoneNo, IsValid = @IsValid WHERE Email=@Email",
                connection);
            command.Parameters.AddWithValue("@Name", user.Name);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Username", user.username);
            command.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@isValid", true);
            int queryResult = command.ExecuteNonQuery();
            connection.Close();
            if (queryResult > 0)
                return new Response(200, "User update successful");
            else
                return new Response(100, "User update failed");
        }
        public Response DeleteUser(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand command = new SqlCommand("Delete from Users where email='" + users.Email + "'", connection);
            connection.Open();
            int i = command.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deletion successful");
            else
                return new Response(100, "Deletion failed");
        }
        public List<Users> GetUsers(SqlConnection connection)
        {
            Response response = new Response();
            List<Users> users = new List<Users>();
            connection.Open();
            SqlCommand command = new SqlCommand("Select * from Users", connection);
            SqlDataReader read = command.ExecuteReader();
            while (read.Read())
            {
                Users user = new Users(read["Name"].ToString(), read["Password"].ToString(), read["Email"].ToString(), read["username"].ToString(), read["PhoneNo"].ToString());
                users.Add(user);
            }
            connection.Close();
            return users;
        }
    }
}