using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Models
{
    public class Land
    {
        public int LandId { get; set; }
        public decimal Area { get; set; }
        public string Category { get; set; } = string.Empty;
        public string LandLocation { get; set; } = string.Empty;
        public decimal AnnualRentPrice { get; set; }
        public string? LandDescription { get; set; }

        public string LandName => $"{Category} - {LandLocation} ({Area}ha)";

        public override string ToString()
        {
            return LandName;
        }
    }
}
