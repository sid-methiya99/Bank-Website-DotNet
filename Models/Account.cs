using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(12)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AccountType { get; set; } = "SAVINGS";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public DateTime OpenDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "PENDING";

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
    }
} 