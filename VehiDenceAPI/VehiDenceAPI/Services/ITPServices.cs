using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class ITPServices
    {
        public Response AddITP(ITP itp, SqlConnection connection)
        {
            if (itp.DataExpirare < itp.DataCreare)
                return new Response(100, "Data expirarii trebuie sa fie dupa data crearii");
            SqlCommand updateCmd = new SqlCommand("UPDATE ITP SET IsValid = 0 WHERE NrInmatriculare = @NrInmatriculare", connection);
            updateCmd.Parameters.AddWithValue("@NrInmatriculare", itp.NrInmatriculare);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlCommand cmd = new SqlCommand("INSERT INTO ITP (NrInmatriculare, DataCreare, DataExpirare,IsValid) VALUES (@NrInmatriculare, @DataCreare, @DataExpirare,1)", connection);
            cmd.Parameters.AddWithValue("@NrInmatriculare", itp.NrInmatriculare);
            cmd.Parameters.AddWithValue("@DataCreare", itp.DataCreare);
            cmd.Parameters.AddWithValue("@DataExpirare", itp.DataExpirare);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "ITP successful");
            else return new Response(100, "ITP failed");
        }

        public Response DeleteITP(ITP itp, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from ITP where NrInmatriculare='" + itp.NrInmatriculare + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion failed");
        }

        public Response ITPList(ITP itp, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from ITP where NrInmatriculare='" + itp.NrInmatriculare + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<ITP> list = new List<ITP>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new ITP(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]),
                        Convert.ToDateTime(dt.Rows[i]["DataCreare"]), Convert.ToDateTime(dt.Rows[i]["DataExpirare"]), Convert.ToInt32(dt.Rows[i]["IsValid"])));
                }
                if (list.Count > 0)
                    return new Response(200, "ITP gasite", list);
                else return new Response(100, "Nu au fost gasite ITP-uri");
            }
            else return new Response(100, "Nu au fost gasite ITP-uri");
        }

        public Response VerificareExpirareITP(SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT distinct Users.Email, Users.Name  " +
            "FROM Users  " +
            "JOIN Masina  ON Users.username = Masina.Username " +
            "JOIN ITP ON Masina.NrInmatriculare = ITP.NrInmatriculare " +
            "WHERE DATEDIFF(day, GETDATE(), ITP.DataExpirare) = 7" +
            "OR DATEDIFF(day, GETDATE(), ITP.DataExpirare) = 2;", connection);
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
                    return new Response(200, "Au fost gasite ITP-uri care vor expira", list, userDaysUntilExpiration);
                else return new Response(100, "Nu au fost gasite ITP-uri care vor expira");
            }
            else return new Response(100, "Nu au fost gasite ITP-uri care vor expira");
        }

        public Response ExpirareITP(SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE ITP SET IsValid = 0 WHERE DataExpirare < @CurrentDate", connection);
            updateCmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT DISTINCT Users.Email, Users.Name " +
            "FROM Users " +
            "JOIN Masina ON Users.Username = Masina.Username " +
            "JOIN ITP ON Masina.NrInmatriculare = ITP.NrInmatriculare " +
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
                    return new Response(200, "ITP-uri expirate gasite", list);
                else return new Response(100, "Nu au fost gasite ITP-uri expirate");
            }
            else return new Response(100, "Nu au fost gasite ITP-uri expirate");
        }
    }
}
