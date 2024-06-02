using Microsoft.Data.SqlClient;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class UserValidations
    {
        public Response UserValidation(Users user, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Update Users set IsValid=1 where username='" + user.username + "' and Token='" + user.Token + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Validation successful");
            else
                return new Response(100, "Validation failed");
        }
        public Response UserValidationEmail(Users user, SqlConnection connection)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("SELECT * FROM Users WHERE Token = @Token",connection);
            command.Parameters.AddWithValue("@Token", user.Token);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
                return new Response(200, "Validation successful");
            else return new Response(100, "Validation failed"); 
            reader.Close();
            connection.Close();
        }
    }
}
