using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class CascoService
    {
        public Response AddCasco(Casco casco, SqlConnection connection)
        {
            try
            {
                if (casco.DataExpirare < casco.DataCreare)
                    return new Response(100, "Data expirarii trebuie sa fie dupa data crearii");
                SqlCommand updateCmd = new SqlCommand("UPDATE Casco SET IsValid = 0 WHERE NrInmatriculare = @NrInmatriculare", connection);
                updateCmd.Parameters.AddWithValue("@NrInmatriculare", casco.NrInmatriculare);
                connection.Open();
                updateCmd.ExecuteNonQuery();
                connection.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO Casco ( NrInmatriculare, DataCreare, DataExpirare, Asigurator, ImageData,IsValid) VALUES ( @NrInmatriculare, @DataCreare, @DataExpirare, @Asigurator, @ImageData,1)", connection);
                cmd.Parameters.AddWithValue("@DataCreare", casco.DataCreare);
                cmd.Parameters.AddWithValue("@NrInmatriculare", casco.NrInmatriculare);
                cmd.Parameters.AddWithValue("@DataExpirare", casco.DataExpirare);
                cmd.Parameters.AddWithValue("@Asigurator", casco.Asigurator);
                if (casco.ImageData != null)
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = casco.ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                    return new Response(200, "Casco successful");
                else
                    return new Response(100, "Casco failed");
            }
            catch (Exception ex)
            {
                return new Response(500, "An error occurred: " + ex.Message);
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public Response DeleteCasco(Casco casco, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from Casco where NrInmatriculare='" + casco.NrInmatriculare + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion failed");
        }

        public Response CascoList(Casco casco, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Casco where NrInmatriculare='" + casco.NrInmatriculare + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Casco> list = new List<Casco>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Casco(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]), Convert.ToDateTime(dt.Rows[i]["DataCreare"]),
                        Convert.ToDateTime(dt.Rows[i]["DataExpirare"]), Convert.ToString(dt.Rows[i]["Asigurator"]), dt.Rows[i]["ImageData"] as byte[], Convert.ToInt32(dt.Rows[i]["IsValid"])));
                }
                if (list.Count > 0)
                    return new Response(200, "Casco gasite", list);
                else return new Response(100, "Nu au fost gasite casco");
            }
            else return new Response(100, "Nu au fost gasite casco");
        }

        public Response VerificareExpirareCasco(SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT distinct Users.Email, Users.Name  " +
            "FROM Users  " +
            "JOIN Masina  ON Users.username = Masina.Username " +
            "JOIN Casco ON Masina.NrInmatriculare = Casco.NrInmatriculare " +
            "WHERE DATEDIFF(day, GETDATE(), Casco.DataExpirare) = 7" +
            "OR DATEDIFF(day, GETDATE(), Casco.DataExpirare) = 2;", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Users> list = new List<Users>();
            Dictionary<string, int> userDaysUntilExpiration = new Dictionary<string, int>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users us = new Users();
                    us.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    us.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    list.Add(us);
                    userDaysUntilExpiration[us.Email] = Convert.ToInt32(dt.Rows[i]["DaysUntilExpiration"]);
                }
                if (list.Count > 0)
                    return new Response(200, "Casco care urmeaza sa expire gasite", list, userDaysUntilExpiration);
                else return new Response(100, "Nu au fost gasite casco care urmeaza sa expire");
            }
            else return new Response(100, "Nu au fost gasite casco care urmeaza sa expire");
        }

        public Response ExpirareCasco(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE Casco SET IsValid = 0 WHERE DataExpirare < @CurrentDate", connection);
            updateCmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT DISTINCT Users.Email, Users.Name " +
            "FROM Users " +
            "JOIN Masina ON Users.Username = Masina.Username " +
            "JOIN Casco ON Masina.NrInmatriculare = Casco.NrInmatriculare " +
            "WHERE Asigurare.DataExpirare < GETDATE()", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Users> list = new List<Users>();
            if (dt.Rows.Count > 1)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users us = new Users();
                    us.Email = Convert.ToString(dt.Rows[i]["Email"]);
                    us.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    list.Add(us);
                }
                if (list.Count > 0)
                    return new Response(200, "Casco expirate", list);
                else return new Response(100, "Nu au fost gasite casco expirate");
            }
            else
                return new Response(100, "Nu au fost gasite casco expirate");
        }
    }
}
