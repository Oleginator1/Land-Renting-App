using LandRentManagementApp.Config;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public static class DatabaseHelper
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(AppSettings.ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void ExecuteNonQuery(string sql, Action<SqlParameterCollection>? addParams = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            addParams?.Invoke(cmd.Parameters);
            cmd.ExecuteNonQuery();
        }

        public static List<T> ExecuteReader<T>(string sql, Func<SqlDataReader, T> map, Action<SqlParameterCollection>? addParams = null)
        {
            var results = new List<T>();
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            addParams?.Invoke(cmd.Parameters);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                results.Add(map(reader));
            return results;
        }

        public static object? ExecuteScalar(string sql, Action<SqlParameterCollection>? addParams = null)
        {
            using var conn = GetConnection();
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            addParams?.Invoke(cmd.Parameters);
            return cmd.ExecuteScalar();
        }

    }
}
