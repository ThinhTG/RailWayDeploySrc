using DAO.Contracts.ReviewResponses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Contracts
{
    public class BlindBoxReviewResponse
    {
        public Guid ReviewId { get; set; }
        public Guid OrderDetailId { get; set; }
        public AccountReviewResponse Account { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
        public string ReviewStatus { get; set; }
    }
}
