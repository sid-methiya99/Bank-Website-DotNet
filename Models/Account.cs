using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BankManagementSystem.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string AccountNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; } = string.Empty;

        [Required]
        public AccountType AccountType { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? Branch { get; set; }
        public string? IFSC { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation property
        public virtual ApplicationUser User { get; set; } = null!;
    }

    public enum AccountType
    {
        Savings,
        Checking,
        Fixed
    }
} 