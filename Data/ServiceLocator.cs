using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Data
{
    public static class ServiceLocator
    {
        private static IFarmerRepository? _farmerRepo;
        private static ILandRepository? _landRepo;
        private static IContractRepository? _contractRepo;

        public static IFarmerRepository FarmerRepo =>
            _farmerRepo ??= new FarmerRepository();

        public static ILandRepository LandRepo =>
            _landRepo ??= new LandRepository();

        public static IContractRepository ContractRepo =>
            _contractRepo ??= new ContractRepository();

        
        public static void Register(IFarmerRepository repo) => _farmerRepo = repo;
        public static void Register(ILandRepository repo) => _landRepo = repo;
        public static void Register(IContractRepository repo) => _contractRepo = repo;
    }
}
