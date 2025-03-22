using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public Guid? ReviewId { get; set; }

        public virtual ICollection<Review>? Reviews { get; set; }



        //public string Gender { get; set; }
        //public string PhoneNumber { get; set; } 
        //public string Address { get; set; } 

        public string? Gender { get; set; } // Made nullable
        public new string? PhoneNumber { get; set; } // Used new keyword and made nullable
        //public string? FullName { get; set; } // Made nullable
        public string? Address { get; set; } // Made nullable
        public string? AvatarURL { get; set; } // Made nullable

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public int? orderCode { get; set; }

        public virtual ICollection<Address>? Addresses { get; set; }



    }
}
