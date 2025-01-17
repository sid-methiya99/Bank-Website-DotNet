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
        public string AccountHolderName { get; set; } = string.Empty;
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        [Required]
        public AccountType AccountType { get; set; }

        // New fields
        public string? Branch { get; set; }
        public string? IFSC { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
    }

    public enum AccountType
    {
        Savings,
        Checking,
        Fixed
    }
} 