using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UpdateOrderDto
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public Guid OrderId { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal PriceTotal { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public bool PaymentConfirmed { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public string Note { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public decimal DiscountMoney { get; set; }

    }
}