using Azure.Core;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.ServiceModel.Channels;

namespace IMAL_FIN_Inquery
{
    public class DAL
    {
        public DataTable IMALChannelstatus(string ChannelName,string username ,string ChannelIPAddress,string ServiceName)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALChannelstatus";
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("IPAddress", ChannelIPAddress);
            cmd.Parameters.AddWithValue("ServiceName", ServiceName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable IMALGLStatus(string GLNo,string ServiceName)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALGLsCheck";
            cmd.Parameters.AddWithValue("GLNo", GLNo);
            cmd.Parameters.AddWithValue("ServiceName", ServiceName);
    
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable IMALResponseField(string ServiceName, string ChannelName)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALFieldCOnfig_Select";
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("ServiceName", ServiceName);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public async void InsertLog(string RequestName,DateTime MWRequestTime,String MWRequest,string status,string ChannelName,string RequestID)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "IMALLogInsert";
            cmd.Parameters.AddWithValue("RequestName", RequestName);
            cmd.Parameters.AddWithValue("MWRequestTime", MWRequestTime);
            cmd.Parameters.AddWithValue("MWRequest", MWRequest);
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("RequestID", RequestID);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            Database_Conection.Open();
            cmd.ExecuteNonQuery();
            Database_Conection.Close();

        }

        public async void UpdateLog(string MWResponseTime,string MWResponse, string Status,string ChannelName,string RequestID)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var DBConnection = MyConfig.GetValue<string>("AppSettings:DBConnection");
            SqlConnection Database_Conection = new SqlConnection(DBConnection);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UpdateLog";
            cmd.Parameters.AddWithValue("MWResponseTime", MWResponseTime);
            cmd.Parameters.AddWithValue("MWResponse", MWResponse);     
            cmd.Parameters.AddWithValue("Status", Status);
            cmd.Parameters.AddWithValue("ChannelName", ChannelName);
            cmd.Parameters.AddWithValue("RequestID", RequestID);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = Database_Conection;
            Database_Conection.Open();
            cmd.ExecuteNonQuery();
            Database_Conection.Close();
        }
    }
}
