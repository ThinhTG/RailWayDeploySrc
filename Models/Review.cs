using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public string ProductId { get; set; }
        public string OrderDetailId { get; set; }
        public string AccountId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; }
        public string ReviewStatus { get; set; }

        // Quan hệ navigation
        public virtual OrderDetail OrderDetail { get; set; }
        public virtual ApplicationUser Account { get; set; }
        //public virtual BlindBox? BlindBox { get; set; }

        //public virtual Package? Package { get; set; }
    }

}
