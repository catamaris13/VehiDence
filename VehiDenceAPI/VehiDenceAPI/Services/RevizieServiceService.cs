using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class RevizieServiceService
    {
        public Response AddRevizieService(RevizieService revizieService, SqlConnection connection)
        {
            SqlCommand updateCmd = new SqlCommand("UPDATE RevizieService SET IsValid = 0 WHERE SerieSasiu = @SerieSasiu", connection);
            updateCmd.Parameters.AddWithValue("@SerieSasiu", revizieService.SerieSasiu);
            connection.Open();
            updateCmd.ExecuteNonQuery();
            connection.Close();
            SqlCommand cmd = new SqlCommand("Insert into RevizieService (SerieSasiu,KmUltim,KmExpirare,ServiceName,IsValid) Values ('" + revizieService.SerieSasiu + "','" + revizieService.KmUltim + "','" + revizieService.KmExpirare + "','" + revizieService.ServiceName + "',1)", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Revizie successful");
            else return new Response(100, "Revizie failed");
        }

        public Response DeleteRevizieService(RevizieService revizieService, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from RevizieService where SerieSasiu='" + revizieService.SerieSasiu + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion failed");
        }

        public Response RevizieServiceList(RevizieService revizieService, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from RevizieService where SerieSasiu='" + revizieService.SerieSasiu + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<RevizieService> list = new List<RevizieService>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new RevizieService(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["SerieSasiu"]), Convert.ToInt32(dt.Rows[i]["KmUltim"]),
                        Convert.ToInt32(dt.Rows[i]["KmExpirare"]), Convert.ToString(dt.Rows[i]["ServiceName"])));
                }
                if (list.Count > 0)
                   return new Response(200, "Revizii gasite", list);
                else return new Response(100, "Nu au fost gasite revizii");
            }
            else return new Response(100, "Nu au fost gasite revizii");
        }
    }
}
