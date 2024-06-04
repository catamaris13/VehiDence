using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class AsigurareService
    {
        public Response AddAsigurare(Asigurare asigurare, SqlConnection connection)
        {
            try
            {
                if (asigurare.DataExpirare < asigurare.DataCreare)
                    return new Response(100, "Data expirarii trebuie sa fie dupa data crearii");
                // Update existing asigurare to IsValid = 0
                SqlCommand updateCmd = new SqlCommand("UPDATE Asigurare SET IsValid = 0 WHERE NrInmatriculare = @NrInmatriculare", connection);
                updateCmd.Parameters.AddWithValue("@NrInmatriculare", asigurare.NrInmatriculare);
                connection.Open();
                updateCmd.ExecuteNonQuery();
                connection.Close();
                // Insert new asigurare with IsValid = 1
                SqlCommand insertCmd = new SqlCommand("INSERT INTO Asigurare (NrInmatriculare, DataCreare, DataExpirare, Asigurator, ImageData, IsValid) VALUES (@NrInmatriculare, @DataCreare, @DataExpirare, @Asigurator, @ImageData, 1)", connection);
                insertCmd.Parameters.AddWithValue("@DataCreare", asigurare.DataCreare);
                insertCmd.Parameters.AddWithValue("@NrInmatriculare", asigurare.NrInmatriculare);
                insertCmd.Parameters.AddWithValue("@DataExpirare", asigurare.DataExpirare);
                insertCmd.Parameters.AddWithValue("@Asigurator", asigurare.Asigurator);
                if (asigurare.ImageData != null)
                {
                    insertCmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = asigurare.ImageData;
                }
                else
                {
                    insertCmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                connection.Open();
                int i = insertCmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                    return new Response(200, "Asigurare successful");
                else
                    return new Response(100, "Asigurare failed");
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

        public Response DeleteAsigurare(Asigurare asigurare, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from Asigurare where id='" + asigurare.Id + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion failed");
        }
        public Response AsigurareList(Asigurare asigurare, SqlConnection connection)
        {
            
            SqlDataAdapter da = new SqlDataAdapter("select * from Asigurare where NrInmatriculare='" + asigurare.NrInmatriculare + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Asigurare> list = new List<Asigurare>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Asigurare(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]), Convert.ToDateTime(dt.Rows[i]["DataCreare"]),
                        Convert.ToDateTime(dt.Rows[i]["DataExpirare"]), Convert.ToString(dt.Rows[i]["Asigurator"]), dt.Rows[i]["ImageData"] as byte[], Convert.ToInt32(dt.Rows[i]["IsValid"])));
                }
                if (list.Count > 0)
                    return new Response(200, "Asigurari gasite", list);
                else return new Response(100, "Nu au fost gasite asigurari");
            }
            else
                return new Response(100, "Nu au fost gasite asigurari");
        }
        public Response VerificareExpirareAsigurareAvans(SqlConnection connection)
        {
            
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT DISTINCT Users.Email, Users.Name, DATEDIFF(day, GETDATE(), Asigurare.DataExpirare) AS DaysUntilExpiration " +
                "FROM Users " +
                "JOIN Masina ON Users.username = Masina.Username " +
                "JOIN Asigurare ON Masina.NrInmatriculare = Asigurare.NrInmatriculare " +
                "WHERE DATEDIFF(day, GETDATE(), Asigurare.DataExpirare) = 7 " +
                "OR DATEDIFF(day, GETDATE(), Asigurare.DataExpirare) = 2;",
                connection);
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
                    return new Response(200, "Asigurari care vor expira gasite", list, userDaysUntilExpiration);
                else
                    return new Response(100, "Nu au fost gasite asigurari care vor expira");
            }
            else
                return new Response(100, "Nu au fost gasite asigurari care vor expira");
        }
        public Response ExpirareAsigurare(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE Asigurare SET IsValid = 0 WHERE DataExpirare < @CurrentDate", connection);
            updateCmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT DISTINCT Users.Email, Users.Name " +
            "FROM Users " +
            "JOIN Masina ON Users.Username = Masina.Username " +
            "JOIN Asigurare ON Masina.NrInmatriculare = Asigurare.NrInmatriculare " +
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
                    return new Response(200, "Asigurari expirate gasite", list);
                else
                    return new Response(100, "Nu au fost gasite asigurari expirate");
            }
            return new Response(100, "Nu au fost gasite asigurari expirate");
        }
    }
}
