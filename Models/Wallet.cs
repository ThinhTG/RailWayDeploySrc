using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Wallet")]
    public class Wallet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WalletId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AccountId { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<ApplicationUser>? ApplicationUser { get; set; }

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
