using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class VignetaService
    {
        public Response AddVigneta(Vigneta vigneta, SqlConnection connection)
        {
            try
            {
                if (vigneta.DataExpirare < vigneta.DataCreare)
                    return new Response(100, "Data expirarii trebuie sa fie dupa data crearii");
                SqlCommand updateCmd = new SqlCommand("UPDATE Vigneta SET IsValid = 0 WHERE NrInmatriculare = @NrInmatriculare", connection);
                updateCmd.Parameters.AddWithValue("@NrInmatriculare", vigneta.NrInmatriculare);
                connection.Open();
                updateCmd.ExecuteNonQuery();
                connection.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO Vigneta (NrInmatriculare, DataCreare, DataExpirare, Tara, ImageData,IsValid) VALUES (@NrInmatriculare, @DataCreare, @DataExpirare, @Tara, @ImageData,1)", connection);
                cmd.Parameters.AddWithValue("@DataCreare", vigneta.DataCreare);
                cmd.Parameters.AddWithValue("@NrInmatriculare", vigneta.NrInmatriculare);
                cmd.Parameters.AddWithValue("@DataExpirare", vigneta.DataExpirare);
                cmd.Parameters.AddWithValue("@Tara", vigneta.Tara);
                if (vigneta.ImageData != null)
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = vigneta.ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                    return new Response(200, "Vigneta successful");
                else
                    return new Response(100, "Vigneta failed");
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

        public Response DeleteVigneta(Vigneta vigneta, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from Vigneta where NrInmatriculare='" + vigneta.NrInmatriculare + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion failed");
        }
        public Response VignetaList(Vigneta vigneta, SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from Vigneta where NrInmatriculare='" + vigneta.NrInmatriculare + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Vigneta> list = new List<Vigneta>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Vigneta(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]), Convert.ToDateTime(dt.Rows[i]["DataCreare"]),
                        Convert.ToDateTime(dt.Rows[i]["DataExpirare"]), Convert.ToString(dt.Rows[i]["Tara"]), dt.Rows[i]["ImageData"] as byte[], Convert.ToInt32(dt.Rows[i]["IsValid"])));
                }
                if (list.Count > 0)
                    return new Response(200, "Vignete gasite", list);
                else
                    return new Response(100, "Nu au fost gasite vignete");
            }
            else
                return new Response(100, "Nu au fost gasite vignete");
        }

        public Response VerificareExpirareVigneta(SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT distinct Users.Email, Users.Name  " +
            "FROM Users  " +
            "JOIN Masina  ON Users.username = Masina.Username " +
            "JOIN Vigneta ON Masina.NrInmatriculare = Vigneta.NrInmatriculare " +
            "WHERE DATEDIFF(day, GETDATE(), Vigneta.DataExpirare) = 7" +
            "OR DATEDIFF(day, GETDATE(), Vigneta.DataExpirare) = 2;", connection);
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
                    return new Response(200, "Exista vignete care vor expira", list, userDaysUntilExpiration);
                else
                    return new Response(100, "Nu au fost gasite vignete care vor expira");
            }
            else
                return new Response(100, "Nu au fost gasite vignete care vor expira");
        }

        public Response ExpirareVigneta(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE Vigneta SET IsValid = 0 WHERE DataExpirare < @CurrentDate", connection);
            updateCmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT DISTINCT Users.Email, Users.Name " +
            "FROM Users " +
            "JOIN Masina ON Users.Username = Masina.Username " +
            "JOIN Vigneta ON Masina.NrInmatriculare = Vigneta.NrInmatriculare " +
            "WHERE Asigurare.DataExpirare < GETDATE()",
            connection);
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
                    return new Response(200, "Vignete gasite expirate", list);
                else return new Response(100, "Nu s-au gasit vignete expirate");
            }
            else
                return new Response(100, "Nu s-au gasit vignete expirate");
        }
    }
}
