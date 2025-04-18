﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class CreateOrderDetailRequest
    {
        [Required]
        public int OrderId { get; set; }

        public Guid? BlindBoxId { get; set; }

        public Guid? PackageId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; } = decimal.Zero;
    }
}
