using LandRentManagementApp.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public class ContractRepository : IContractRepository
    {
        private const string SelectJoin = @"
        SELECT c.ContractId, c.FarmerId, c.LandId,
               c.ContractSignDate, c.YearsPayed,
               f.Name, f.Surname,
               t.Category, t.Location, t.AnnualRentPrice
        FROM dbo.Contract c
        INNER JOIN dbo.Farmer f ON c.FarmerId = f.Farmer
        INNER JOIN dbo.Land   t ON c.LandId   = t.LandId";

        private static Contract Map(SqlDataReader r) => new()
        {
            ContractId = r.GetInt32(0),
            FarmerId = r.GetInt32(1),
            LandId = r.GetInt32(2),
            ContractSignDate = r.GetDateTime(3),
            YearsPayed = r.GetInt32(4),
            FarmerName = r.GetString(5),
            FarmerSurname = r.GetString(6),
            LandCategory = r.GetString(7),
            LandLocation = r.GetString(8),
            AnnualRentPrice = r.GetDecimal(9)
        };

        public List<Contract> GetAll()
        {
            string sql = SelectJoin + " ORDER BY f.Name, f.Surname";
            return DatabaseHelper.ExecuteReader(sql, Map);
        }

        public Contract? GetById(int id)
        {
            string sql = SelectJoin + " WHERE c.ContractId = @Id";
            return DatabaseHelper.ExecuteReader(sql, Map,
                p => p.AddWithValue("@Id", id)).FirstOrDefault();
        }

        public void Add(Contract c)
        {
            const string sql = @"
            INSERT INTO dbo.Contract (FarmerId, LandId, ContractSignDate, YearsPayed)
            VALUES (@FarmerId, @LandId, @ContractSignDate, @YearsPayed)";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@FarmerId", c.FarmerId);
                p.AddWithValue("@LandId", c.LandId);
                p.AddWithValue("@ContractSignDate", c.ContractSignDate);
                p.AddWithValue("@YearsPayed", c.YearsPayed);
            });
        }

        public void Update(Contract c)
        {
            const string sql = @"
            UPDATE dbo.Contract
            SET FarmerId=@FarmerId, LandId=@LandId,
                ContractSignDate=@ContractSignDate, YearsPayed=@YearsPayed
            WHERE IdContract=@Id";
            DatabaseHelper.ExecuteNonQuery(sql, p =>
            {
                p.AddWithValue("@FarmerId", c.FarmerId);
                p.AddWithValue("@LandId", c.LandId);
                p.AddWithValue("@ContractSignDate", c.ContractSignDate);
                p.AddWithValue("@YearsPayed", c.YearsPayed);
                p.AddWithValue("@Id", c.ContractId);
            });
        }

        public void Delete(int id)
        {
            const string sql = "DELETE FROM dbo.Contract WHERE ContractId = @Id";
            DatabaseHelper.ExecuteNonQuery(sql,
                p => p.AddWithValue("@Id", id));
        }

        public List<Contract> GetByFarmer(int farmerId)
        {
            string sql = SelectJoin + " WHERE c.FarmerId = @Id ORDER BY c.ContractSignDate DESC";
            return DatabaseHelper.ExecuteReader(sql, Map,
                p => p.AddWithValue("@Id", farmerId));
        }

        public bool ExistsDuplicate(int farmerId, int landId, int excludeId = 0)
        {
            const string sql = @"
            SELECT COUNT(1) FROM dbo.Contract
            WHERE FarmerId=@FarmerId AND LandId=@LandId
              AND ContractId <> @ExcludeId";
            var result = DatabaseHelper.ExecuteScalar(sql, p =>
            {
                p.AddWithValue("@FarmerId", farmerId);
                p.AddWithValue("@LandId", landId);
                p.AddWithValue("@ExcludeId", excludeId);
            });
            return Convert.ToInt32(result) > 0;
        }
    }
}
