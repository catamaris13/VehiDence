using Microsoft.Data.SqlClient;
using System.Data;
using VehiDenceAPI.Models;

namespace VehiDenceAPI.Services
{
    public class MasinaService
    {
        public Response AddMasina(Masina masina, SqlConnection connection)
        {
            try
            {
                string query = "INSERT INTO Masina (SerieSasiu, NrInmatriculare, Marca, Model, Username, ImageData) " +
                               "VALUES (@SerieSasiu, @NrInmatriculare, @Marca, @Model, @Username, @ImageData)";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@SerieSasiu", masina.SerieSasiu);
                cmd.Parameters.AddWithValue("@NrInmatriculare", masina.NrInmatriculare);
                cmd.Parameters.AddWithValue("@Marca", masina.Marca);
                cmd.Parameters.AddWithValue("@Model", masina.Model);
                cmd.Parameters.AddWithValue("@Username", masina.Username);
                if (masina.ImageData != null)
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = masina.ImageData;
                }
                else
                {
                    cmd.Parameters.Add("@ImageData", SqlDbType.VarBinary).Value = DBNull.Value;
                }
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                    return new Response(200, "Masina adaugata cu succes");
                else return new Response(100, "Masina nu a putut fi adaugata");
            }
            catch (Exception ex)
            {
                connection.Close();
                return new Response(500, "Eroare: " + ex.Message);
            }
        }
        public Response MasinaListId(Masina masina, SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from Masina where Id='" + masina.Id + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Masina> list = new List<Masina>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Masina(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["SerieSasiu"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]),
                        Convert.ToString(dt.Rows[i]["Marca"]), Convert.ToString(dt.Rows[i]["Model"]), Convert.ToString(dt.Rows[i]["Username"]), dt.Rows[i]["ImageData"] as byte[]));
                }
                if (list.Count > 0)
                    return new Response(200, "Masini gasite", list);
                else
                    return new Response(100, "Nu au fost gasite masini");
            }
            return new Response(100, "Nu au fost gasite masini");
        }
        public Response MasinaListUsername(Masina masina, SqlConnection connetion)
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from Masina where username='" + masina.Username + "'", connetion);
            DataTable dt = new DataTable();
            da.Fill(dt);
            List<Masina> list = new List<Masina>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Masina(Convert.ToInt32(dt.Rows[i]["Id"]), Convert.ToString(dt.Rows[i]["SerieSasiu"]), Convert.ToString(dt.Rows[i]["NrInmatriculare"]),
                        Convert.ToString(dt.Rows[i]["Marca"]), Convert.ToString(dt.Rows[i]["Model"]), Convert.ToString(dt.Rows[i]["Username"]), dt.Rows[i]["ImageData"] as byte[]));
                }
                if (list.Count > 0)
                    return new Response(200, "Masini gasite", list);
                else
                    return new Response(100, "Nu au fost gasite masini");
            }
            return new Response(100, "Nu au fost gasite masini");
        } 
        public Response DeleteMasina(Masina masina, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("Delete from Masina where id='" + masina.Id + "'", connection);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
                return new Response(200, "Deleted successful");
            else return new Response(100, "Deletion successful");
        }
    }
}