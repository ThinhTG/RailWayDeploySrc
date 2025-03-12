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
        public string AccountId { get; set; }

        public virtual ApplicationUser? Account { get; set; }

        [Required]
        public int Balance { get; set; }

        public virtual ApplicationUser? ApplicationUser { get; set; }

        public virtual ICollection<WalletTransaction> WalletTransactions { get; set; }
    }
}
