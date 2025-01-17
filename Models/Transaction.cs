using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; } = string.Empty;  // "TRANSFER", "WITHDRAWAL", "DEPOSIT", "BILL_PAYMENT"

        [Required]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "PENDING";  // "COMPLETED", "PENDING", "FAILED"

        // Navigation properties
        [ForeignKey("FromAccountId")]
        public virtual Account FromAccount { get; set; } = null!;

        [ForeignKey("ToAccountId")]
        public virtual Account ToAccount { get; set; } = null!;
    }
} 