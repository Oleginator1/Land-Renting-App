using LandRentManagementApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public class FarmerRepository : IFarmerRepository
    {
        private static Farmer Map(SqlDataReader r) => new()
        {
            FarmerId = r.GetInt32(0),
            Name = r.GetString(1),
            Surname = r.GetString(2),
            IDNP = r.GetString(3),
            Residence = r.GetString(4),
            Phone = r.IsDBNull(5) ? null : r.GetString(5),
            Email = r.IsDBNull(6) ? null : r.GetString(6)
        };

        public List<Farmer> GetAll()
        {
            const string sql = @"
            SELECT FarmerId, Name, Surname, IDNP, Residence, Phone, Email
            FROM dbo.Farmer
            ORDER BY Name, Surname";
            return DatabaseHelper.ExecuteReader(sql, Map);
        }

        public Farmer? GetById(int id)
        {
            const string sql = @"
            SELECT FarmerId, Name, Surname, IDNP, Residence, Phone, Email
            FROM dbo.Farmer WHERE FarmerId = @Id";
            var list = DatabaseHelper.ExecuteReader(sql, Map,
                p => p.AddWithValue("@Id", id));
            return list.FirstOrDefault();
        }

        public void Add(Farmer farmer)
        {
            const string sql = @"INSERT INTO dbo.Farmer (Name, Surname, IDNP, Residence, Phone, Email) 
                VALUES (@Name, @Surname, @IDNP, @Residence, @Phone, @Email)";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@Name", farmer.Name);
                p.AddWithValue("@Prenume", farmer.Surname);
                p.AddWithValue("@IDNP", farmer.IDNP);
                p.AddWithValue("@Residence", farmer.Residence);
                p.AddWithValue("@Phone", (object?)farmer.Phone ?? DBNull.Value);
                p.AddWithValue("@Email", (object?)farmer.Email ?? DBNull.Value);
            });
        }

        public void Update(Farmer farmer)
        {
            const string sql = @"UPDATE dbo.Farmer SET Name=@Name, Surname=@Surname, IDNP=@IDNP, Residence=@Residence, Phone=@Phone, Email=@Email";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@Name", farmer.Name);
                p.AddWithValue("@Surname", farmer.Surname);
                p.AddWithValue("@IDNP", farmer.IDNP);
                p.AddWithValue("@Residence", farmer.Residence);
                p.AddWithValue("@Phone", (object?)farmer.Phone ?? DBNull.Value);
                p.AddWithValue("@Email", (object?)farmer.Email ?? DBNull.Value);
                p.AddWithValue("@Id", farmer.FarmerId);
            });
        }

        public void Delete(int id)
        {
            const string sql = @"DELETE FROM dbo.Farmer WHERE FarmerId=@Id";
            DatabaseHelper.ExecuteNonQuery(sql,
            p => p.AddWithValue("@Id", id));
        }

        public List<Farmer> Search(string term)
        {
            const string sql = @"SELECT FarmerId, Name, Surname, IDNP, Residence, Phone, Email FROM dbo.Farmer 
                WHERE Name LIKE %@T OR Prenume LIKE %T OR Residence LIKE %T OR IDNP LIKE %T
                ORDER BY Name, Surname";
            return DatabaseHelper.ExecuteReader(sql, Map,
            p => p.AddWithValue("@T", $"%{term}%"));
        }

        public bool ExistsIdnp(string idnp, int excludeId = 0)
        {
            const string sql = @"SELECT COUNT(1) FROM dbo.Farmer WHERE IDNP = @IDNP AND FarmerId <> @ExcludeId";
            var result = DatabaseHelper.ExecuteScalar(sql, p =>
            {
                p.AddWithValue("@IDNP", idnp);
                p.AddWithValue("@ExcludeId", excludeId);
            });
            return Convert.ToInt32(result) > 0;
        }

    }
}
