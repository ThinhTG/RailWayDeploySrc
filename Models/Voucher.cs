﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid VoucherId { get; set; } = Guid.NewGuid();

        
        public int? OrderId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal DiscountMoney { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string VoucherCode { get; set; }

        // So tien toi thieu de ap dung Voucher
        [Required]
        public decimal Money { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; }

        public virtual Order Order { get; set; }
    }
}
