using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        public String AccountId { get; set; }

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

        public int? OrderCode { get; set; }

        [Required]
        public decimal DiscountMoney { get; set; }

        public virtual ApplicationUser? Account { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
