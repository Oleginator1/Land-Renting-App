using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public int FarmerId { get; set; }
        public int LandId { get; set; }
        public DateTime ContractSignDate { get; set; }
        public int YearsPayed { get; set; }

        public string FarmerName { get; set; } = string.Empty;
        public string FarmerSurname { get; set; } = string.Empty;
        public string LandCategory { get; set; } = string.Empty;
        public string LandLocation { get; set; } = string.Empty;
        public decimal AnnualRentPrice { get; set; }

        public decimal TotalSum => YearsPayed * AnnualRentPrice;
        public string NumeCompletFermier => $"{FarmerName} {FarmerSurname}";

    }
}
