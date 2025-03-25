using DAO.Contracts.ReviewResponses;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Contracts
{
    public class ReviewResponse
    {
        public Guid ReviewId { get; set; }
        public Guid OrderDetailId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
        public string ReviewStatus { get; set; }
        public string ImageUrl { get; set; }

        public AccountReviewResponse Account { get; set; }
    }
}
