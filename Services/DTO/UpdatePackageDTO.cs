using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdatePackageDTO
    {

       

       
        public string PackageName { get; set; }

       
        public string TypeSell { get; set; }

       
        public decimal PackagePrice { get; set; }

        public string? Description { get; set; } // Cho phép null

      
        public int Stock { get; set; }

      
        public int Amount { get; set; } = 0;

      
        public string PackageStatus { get; set; }
    }
}
