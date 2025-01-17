using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(10)]
        public string MobileNumber { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(6)]
        public string PinCode { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        public string AadhaarNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string PanNumber { get; set; } = string.Empty;

        // KYC Document paths
        public string? AadhaarDocPath { get; set; }
        public string? PanDocPath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
} 