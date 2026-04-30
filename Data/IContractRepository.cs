using LandRentManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public interface IContractRepository
    {
        List<Contract> GetAll();
        Contract? GetById(int id);
        void Add(Contract contract);
        void Update(Contract contract);
        void Delete(int id);
        List<Contract> GetByFarmer(int idFarmer);
        bool ExistsDuplicate(int idFarmer, int idLand, int excludeId = 0);
    }
}
