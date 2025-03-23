using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Contracts
{
   public class ReviewRequest
    {
        public Guid OrderDetailId { get; set; }
        public string AccountId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }

        public string? ImageURL { get; set; }

    }
}
