using LandRentManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public interface IFarmerRepository
    {
        List<Farmer> GetAll();
        Farmer? GetById(int id);
        void Add(Farmer farmer);
        void Update(Farmer farmer);
        void Delete(int id);
        List<Farmer> Search(string term);
        bool ExistsIdnp(string idnp, int excludeId = 0);
    }
}
