using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RaxhetMvc.Models;
using System.Data;

namespace RaxhetMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }

    public class DBAccess
    {
        SqlConnection con = new SqlConnection("Data Source=wpl22.hosting.reg.ru;User ID=u0790449_Raxhet;Password=********;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        public string AddRecord(ErrorViewModel errorViewModel)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spMyProcedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", errorViewModel.Name);
                cmd.Parameters.AddWithValue("@Surname", errorViewModel.Surname);
                cmd.Parameters.AddWithValue("@City", errorViewModel.City);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Data save succsess");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return (ex.Message.ToString());
            }
        }
    }
}

