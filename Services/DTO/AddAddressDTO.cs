using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddAddressDTO
    {
        public string AccountId { get; set; }
        public string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }

        public string PhoneNumber { get; set; }


        public string NameReceiver { get; set; }
        public string City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsDefault { get; set; }
      
        public DateTime CreatedAt { get; set; }

        
        public DateTime UpdatedAt { get; set; }
    }
}
