using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Contracts
{
    public class BlindBoxMobileResponse
    {
        public Guid BlindBoxId { get; set; }
        public string BlindBoxName { get; set; }

        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; } 

        public int Stock { get; set; }


    }
}
