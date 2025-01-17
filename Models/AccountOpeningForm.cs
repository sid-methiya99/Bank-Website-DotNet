using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BankManagementSystem.Models
{
    public class AccountOpeningForm
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter valid 10-digit mobile number")]
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
        [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Please enter valid 6-digit PIN code")]
        public string PinCode { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Please enter valid 12-digit Aadhaar number")]
        public string AadhaarNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$", ErrorMessage = "Please enter valid PAN number")]
        public string PanNumber { get; set; } = string.Empty;

        [Required]
        public IFormFile AadhaarFile { get; set; } = null!;

        [Required]
        public IFormFile PanFile { get; set; } = null!;

        [Required]
        public string AccountType { get; set; } = string.Empty;

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Initial deposit must be at least ₹1,000")]
        public decimal InitialDeposit { get; set; }
    }
} 