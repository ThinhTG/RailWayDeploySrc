using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateBlindBoxDTOV2
    {

       public string BlindBoxName { get; set; }


        public string TypeSell { get; set; }


        public string Size { get; set; }


        public decimal Price { get; set; }


        public string? Description { get; set; } // Cho phép null


        public int Stock { get; set; }




        public DateTime UpdatedAt { get; set; }

        public float? Percent { get; set; } // Cho phép null


        public string BlindBoxStatus { get; set; }
    }
}
