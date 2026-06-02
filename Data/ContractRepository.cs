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
               t.Category, t.LandLocation, t.AnnualRentPrice
        FROM dbo.RentContract c
        INNER JOIN dbo.Farmer f ON c.FarmerId = f.FarmerId
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
            INSERT INTO dbo.RentContract (FarmerId, LandId, ContractSignDate, YearsPayed)
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
            UPDATE dbo.RentContract
            SET FarmerId=@FarmerId, LandId=@LandId,
                ContractSignDate=@ContractSignDate, YearsPayed=@YearsPayed
            WHERE ContractId=@Id";
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
            const string sql = "DELETE FROM dbo.RentContract WHERE ContractId = @Id";
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
            SELECT COUNT(1) FROM dbo.RentContract
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

        public bool TerenOcupat(int idTeren, int excludeId = 0)
        {
            const string sql = @"
        SELECT COUNT(1) FROM dbo.RentContract
        WHERE LandId = @LandId AND ContractId <> @ExcludeId";
            var r = DatabaseHelper.ExecuteScalar(sql, p =>
            {
                p.AddWithValue("@LandId", idTeren);
                p.AddWithValue("@ExcludeId", excludeId);
            });
            return Convert.ToInt32(r) > 0;
        }

        public Dictionary<string, decimal> GetSumePerFermier()
        {
            const string sql = @"
        SELECT f.Name + ' ' + f.Surname,
               SUM(c.YearsPayed * t.AnnualRentPrice) AS Suma
        FROM dbo.Contract c
        INNER JOIN dbo.Farmer f ON c.FarmerId = f.FarmerId
        INNER JOIN dbo.Land   t ON c.LandId   = t.LandId
        GROUP BY f.Name, f.Surname, f.FarmerId
        ORDER BY Suma DESC";

            var result = new Dictionary<string, decimal>();
            DatabaseHelper.ExecuteReader(sql, r =>
            {
                result[r.GetString(0)] = r.GetDecimal(1);
                return true;
            });
            return result;
        }

        public (string Teren, int NrContracte) GetTerenCeleMaiMulteContracte()
        {
            const string sql = @"
        SELECT TOP 1
            t.Category + ' — ' + t.LandLocation,
            COUNT(c.ContractId)
        FROM dbo.Contract c
        INNER JOIN dbo.Land t ON c.LandId = t.LandId
        GROUP BY t.LandId, t.Category, t.LandLocation
        ORDER BY COUNT(c.ContractId) DESC";

            var r = DatabaseHelper.ExecuteReader(sql,
                reader => (reader.GetString(0), reader.GetInt32(1)));
            return r.FirstOrDefault();
        }


        public List<(string Fermier, string Localitate, int NrContracte, decimal SumaTotala, DateTime? PrimulContract)> GetRaportFermieri()
        {
            const string sql = @"
        SELECT f.Name + ' ' + f.Surname,
               f.Residence,
               COUNT(c.ContractId),
               SUM(c.YearsPayed * t.AnnualRentPrice),
               MIN(c.ContractSignDate)
        FROM dbo.Farmer f
        INNER JOIN dbo.Contract c ON f.FarmerId = c.FarmerId
        INNER JOIN dbo.Land    t ON c.LandId   = t.LandId
        GROUP BY f.FarmerId, f.Name, f.Surname, f.Residence
        ORDER BY SUM(c.YearsPayed * t.AnnualRentPrice) DESC";

            return DatabaseHelper.ExecuteReader(sql, r =>
                (r.GetString(0), r.GetString(1), r.GetInt32(2),
                 r.GetDecimal(3), r.IsDBNull(4) ? (DateTime?)null
                     : r.GetDateTime(4)));
        }
    }


}
