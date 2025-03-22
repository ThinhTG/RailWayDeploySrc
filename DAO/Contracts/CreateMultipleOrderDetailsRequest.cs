using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Contracts
{
    public class CreateMultipleOrderDetailsRequest
    {

        public int OrderId { get; set; }
        public List<OrderDetailRq> OrderDetails { get; set; }
    }
}
