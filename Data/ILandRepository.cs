using LandRentManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public interface ILandRepository
    {
        List<Land> GetAll();
        Land? GetById(int id);
        void Add(Land land);
        void Update(Land land);
        void Delete(int id);
        List<Land> Filter(string? location, string? category);
        List<string> GetLocation();
        List<string> GetCategory();
    }
}
