using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Data;
using BankManagementSystem.Services;
using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Pages.Account
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FileUploadService _fileUploadService;

        public CreateModel(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            FileUploadService fileUploadService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
        }

        [BindProperty]
        public AccountOpeningForm AccountForm { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found. Please log in again.");
                return Page();
            }

            // Debug: Print user details
            Console.WriteLine($"User ID: {user.Id}");
            Console.WriteLine($"User Email: {user.Email}");

            try
            {
                // First verify if user exists in database
                var dbUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
                
                if (dbUser == null)
                {
                    ModelState.AddModelError("", "User not found in database. Please log in again.");
                    return Page();
                }

                Console.WriteLine($"Found user in database. ID: {dbUser.Id}");

                // Save KYC documents first
                var kycDocuments = new List<KYCDocument>();

                // Upload KYC documents
                var (aadhaarSuccess, aadhaarPath, aadhaarError) = await _fileUploadService.UploadKYCDocumentAsync(
                    AccountForm.AadhaarFile, user.Id, "AADHAAR");
                
                if (!aadhaarSuccess)
                {
                    ModelState.AddModelError("", $"Failed to upload Aadhaar card: {aadhaarError}");
                    return Page();
                }

                var (panSuccess, panPath, panError) = await _fileUploadService.UploadKYCDocumentAsync(
                    AccountForm.PanFile, user.Id, "PAN");
                
                if (!panSuccess)
                {
                    ModelState.AddModelError("", $"Failed to upload PAN card: {panError}");
                    return Page();
                }

                // Create KYC documents
                kycDocuments.Add(new KYCDocument
                {
                    UserId = user.Id,
                    DocumentType = "AADHAAR",
                    DocumentNumber = AccountForm.AadhaarNumber,
                    FilePath = aadhaarPath,
                    UploadDate = DateTime.Now
                });

                kycDocuments.Add(new KYCDocument
                {
                    UserId = user.Id,
                    DocumentType = "PAN",
                    DocumentNumber = AccountForm.PanNumber,
                    FilePath = panPath,
                    UploadDate = DateTime.Now
                });

                // Add KYC documents to context
                _context.KYCDocuments.AddRange(kycDocuments);
                
                // Save KYC documents first
                await _context.SaveChangesAsync();
                Console.WriteLine("KYC documents saved successfully");

                // Create new account
                var account = new Models.Account
                {
                    UserId = user.Id,
                    AccountNumber = GenerateAccountNumber(),
                    AccountType = AccountForm.AccountType,
                    Balance = AccountForm.InitialDeposit,
                    OpenDate = DateTime.Now,
                    Status = "PENDING"
                };

                Console.WriteLine($"Creating account with UserId: {account.UserId}");
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Account application submitted successfully. It will be reviewed shortly.";
                return RedirectToPage("/Account/Dashboard");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                ModelState.AddModelError("", $"An error occurred while processing your request: {ex.Message}");
                return Page();
            }
        }

        private string GenerateAccountNumber()
        {
            // Generate a 12-digit account number
            // Format: YYYYMM + 6 random digits
            Random random = new Random();
            return DateTime.Now.ToString("yyyyMM") + random.Next(100000, 999999).ToString();
        }
    }

    public class AccountOpeningForm
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Phone]
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
        public string PinCode { get; set; } = string.Empty;

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string AadhaarNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string PanNumber { get; set; } = string.Empty;

        [Required]
        public IFormFile AadhaarFile { get; set; } = null!;

        [Required]
        public IFormFile PanFile { get; set; } = null!;

        [Required]
        public string AccountType { get; set; } = "SAVINGS";

        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Initial deposit must be at least ₹1,000")]
        public decimal InitialDeposit { get; set; }
    }
} 