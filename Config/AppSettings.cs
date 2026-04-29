using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Config
{
    public static class AppSettings
    {
        public static string ConnectionString { get; private set; } = "Server=DESKTOP-T924N09;Database=LandRentDB;Trusted_Connection=True;TrustServerCertificate=True";

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

    }
}
