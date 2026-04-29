using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LandRentManagementApp.Models
{
    public class Farmer
    {
        public int FarmerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set;} = string.Empty;
        public string IDNP { get; set; } = string.Empty;
        public string Residence {  get; set; } = string.Empty;
        public string? Phone {  get; set; }
        public string? Email { get; set; }

        public string FullName => $"{Name} {Surname}";

        public override string ToString()
        {
            return FullName;
        }


    }
}
