using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class UserAuthentificationService
    {
        public Response Registration(Users user, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Insert into Users(Name,Email,Password,PhoneNo,username,Token,IsValid) Values('" + user.Name + "','" + user.Email + "','" + BCrypt.Net.BCrypt.HashPassword(user.Password) + "','" + user.PhoneNo + "','" + user.username + "','" + user.Token + "',0)", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Registration successful");
            else return new Response(100, "Registration failed");
        }
        public Response ResetToken(Users user, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Update Users set Token='" + user.Token + "' where Email='" + user.Email + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Token changed successful");
            else return new Response(100, "Token change failed");
        }
        public Response ResetPassword(Users user, SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("UPDATE Users SET  Password = @Password WHERE Token=@Token", connection);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Token", user.Token);
            int queryResult = command.ExecuteNonQuery();
            connection.Close();
            if (queryResult > 0)
                return new Response(200, "Reset password successful");
            else return new Response(100, "Reset password failed");
        }
        public Response Login(Users user, SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from Users where Email= '" + user.Email + "'and IsValid = 1", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                string storedHashedPassword = Convert.ToString(dt.Rows[0]["Password"]);
                if (BCrypt.Net.BCrypt.Verify(user.Password, storedHashedPassword))
                    return new Response(200, "Login Successful", new Users(Convert.ToInt32(dt.Rows[0]["Id"]), Convert.ToString(dt.Rows[0]["Name"]), Convert.ToString(dt.Rows[0]["Email"]), Convert.ToString(dt.Rows[0]["username"])));
                else
                    return new Response(100, "Login Failed. Incorrect password.");
            }
            else
                return new Response(100, "Login Failed. Please Sign up or check your Email");
        }
    }
}