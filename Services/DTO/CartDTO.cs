using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CartDTO
    {
            public string UserId { get; set; }
            public Guid? BlindBoxId { get; set; }
            public Guid? PackageId { get; set; }
            public int Quantity { get; set; }
    }
}
