using LandRentManagementApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace LandRentManagementApp.Data
{
    public class LandRepository : ILandRepository
    {
        private static Land Map(SqlDataReader r) => new()
        {
            LandId = r.GetInt32(0),
            Area = r.GetDecimal(1),
            Category = r.GetString(2),
            LandLocation = r.GetString(3),
            AnnualRentPrice = r.GetDecimal(4),
            LandDescription = r.IsDBNull(5) ? null : r.GetString(5)
        };

        public List<Land> GetAll()
        {
            const string sql = @"SELECT * FROM dbo.Land ORDER BY LandLocation, Category";
            return DatabaseHelper.ExecuteReader(sql, Map);
        }

        public Land? GetById(int id)
        {
            const string sql = @"SELECT * FROM dbo.Land WHERE LandId = @Id";
            return DatabaseHelper.ExecuteReader(sql, Map,
            p => p.AddWithValue("@Id", id)).FirstOrDefault();
        }

        public void Add(Land land)
        {
            const string sql = @"INSERT INTO dbo.Land (Area, Category, LandLocation, AnnualRentPrice, Description) 
                VALUES (@Area, @Category, @LandLocation, @AnnualRentPrice, @Description)";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@Area", land.Area);
                p.AddWithValue("@Category", land.Category);
                p.AddWithValue("@LandLocation", land.LandLocation);
                p.AddWithValue("@AnnualRentPrice", land.AnnualRentPrice);
                p.AddWithValue("@Description", (object?)land.LandDescription ?? DBNull.Value);
            });
        }

        public void Update(Land land)
        {
            const string sql = @"UPDATE dbo.Land SET Area = @Area, Category = @Category, LandLocation = @LandLocation, AnnualRentPrice = @AnnualPrice, Description = @Description
                    WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@Area", land.Area);
                p.AddWithValue("@Category", land.Category);
                p.AddWithValue("@LandLocation", land.LandLocation);
                p.AddWithValue("@AnnualRentPrice", land.AnnualRentPrice);
                p.AddWithValue("@Description", (object?)land.LandDescription ?? DBNull.Value);
                p.AddWithValue("@Id", land.LandId);

            });
        }

        public void Delete(int id)
        {
            const string sql = @"DELETE FROM dbo.Land WHERE LandId = @Id";
            DatabaseHelper.ExecuteNonQuery(sql,
          p => p.AddWithValue("@Id", id));
        }

        public List<Land> Filter(string? location, string? category)
        {
            var where = new List<string>();
            if (!string.IsNullOrWhiteSpace(location)) where.Add("LandLocation = @LandLocation");
            if (!string.IsNullOrWhiteSpace(category)) where.Add("Category = @Category");

            string sql = "SELECT LandId, Area, Category, LandLocation, AnnualRentPrice, Description" +
                         "FROM dbo.Teren";
            if (where.Count > 0)
                sql += " WHERE " + string.Join(" AND ", where);
            sql += " ORDER BY LandLocation, Category";

            return DatabaseHelper.ExecuteReader(sql, Map, p =>
            {
                if (!string.IsNullOrWhiteSpace(location)) p.AddWithValue("@LandLocation", location);
                if (!string.IsNullOrWhiteSpace(category)) p.AddWithValue("@Category", category);
            });
        }

        public List<string> GetLocation()
        {
            const string sql = "SELECT DISTINCT LandLocation FROM dbo.Land ORDER BY LandLocation";
            return DatabaseHelper.ExecuteReader(sql, r => r.GetString(0));
        }

        public List<string> GetCategory()
        {
            const string sql = "SELECT DISTINCT Category FROM dbo.Land ORDER BY Category";
            return DatabaseHelper.ExecuteReader(sql, r => r.GetString(0));
        }
    }
}
