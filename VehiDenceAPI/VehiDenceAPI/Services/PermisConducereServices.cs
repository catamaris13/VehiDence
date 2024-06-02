using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class PermisConducereServices
    {
        public Response AddPermisConducere(PermisConducere permisConducere, SqlConnection connection)
        {
            try
            {
                if (permisConducere.DataExpirare < permisConducere.DataCreare)
                    return new Response(100, "Data expirarii trebuie sa fie dupa data crearii");
                SqlCommand updateCmd = new SqlCommand("UPDATE PermisConducere SET IsValid = 0 WHERE Username = @Username", connection);
                updateCmd.Parameters.AddWithValue("@Username", permisConducere.username);
                connection.Open();
                updateCmd.ExecuteNonQuery();
                connection.Close();
                SqlCommand cmd = new SqlCommand("INSERT INTO PermisConducere (Nume, Username, DataCreare, DataExpirare, Categorie, ImageData,IsValid) VALUES (@Nume, @Username, @DataCreare, @DataExpirare, @Categorie, @ImageData,1)", connection);
                cmd.Parameters.AddWithValue("@DataCreare", permisConducere.DataCreare);
                cmd.Parameters.AddWithValue("@Nume", permisConducere.Nume);
                cmd.Parameters.AddWithValue("@Username", permisConducere.username);
                cmd.Parameters.AddWithValue("@DataExpirare", permisConducere.DataExpirare);
                cmd.Parameters.AddWithValue("@Categorie", permisConducere.Categorie);
                if (permisConducere.ImageData != null)
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = permisConducere.ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                    return new Response(200, "Permis de conducere successful");
                else return new Response(100, "Permis de conducere failed");
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

        public Response DeletePermisConducere(PermisConducere permisConducere, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("Delete from PermisConducere where username='" + permisConducere.username + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deteled successful");
            else return new Response(100, "Deletion failed");
        }

        public Response PermisConducereList(PermisConducere permisConducere, SqlConnection connetion)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from PermisConducere where NrInmatriculare='" + permisConducere.username + "'", connetion);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<PermisConducere> list = new List<PermisConducere>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new PermisConducere(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["Nume"]), Convert.ToString(dt.Rows[i]["username"]),
                        Convert.ToDateTime(dt.Rows[i]["DataCreare"]), Convert.ToDateTime(dt.Rows[i]["DataExpirare"]),
                        Convert.ToString(dt.Rows[i]["Categorie"]), dt.Rows[i]["ImageData"] as byte[]));
                }
                if (list.Count > 0)
                    return new Response(200, "Permise de conducere gasite", list);
                else return new Response(100, "Nu au fost gasite permise de conducere");
            }
            else return new Response(100, "Nu au fost gasite permise de conducere");
        }

        public Response VerificareExpirarePermisConducere(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("SELECT distinct Users.Email, Users.Name  " +
            "FROM Users  " +
            "JOIN PermisConducere  ON Users.username = PermisConducere.Username " +
            "WHERE DATEDIFF(day, GETDATE(), PermisConducere.DataExpirare) = 7+" +
            "OR DATEDIFF(day, GETDATE(), PermisConducere.DataExpirare) = 2;", connection);
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
                    return new Response(200, "Au fost gasite permise de conducere care vor expira", list, userDaysUntilExpiration);
                else return new Response(100, "Nu au fost gasite permise care vor expira");
            }
            else return new Response(100, "Nu au fost gasite permise care vor expira");
        }
        public Response ExpirarePermisConducere(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE PermisConducere SET IsValid = 0 WHERE DataExpirare < @CurrentDate", connection);
            updateCmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT DISTINCT Users.Email, Users.Name " +
            "FROM Users " +
            "JOIN PermisConducere  ON Users.username = PermisConducere.Username " +
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
                    return new Response(200, "Permise de conducere expirate gasite", list);
                else return new Response(100, "Nu au fost gasite permise de conducere expirate");
            }
            else return new Response(100, "Nu au fost gasite permise de conducere expirate");
        }
    }
}