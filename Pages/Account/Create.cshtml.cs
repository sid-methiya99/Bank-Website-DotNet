using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BankManagementSystem.Models;
using BankManagementSystem.Data;

namespace BankManagementSystem.Pages.Account
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public AccountOpeningForm AccountForm { get; set; } = new();

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Create new account
            var account = new Models.Account
            {
                AccountHolderName = AccountForm.FullName,
                AccountNumber = GenerateAccountNumber(),
                AccountType = Enum.Parse<AccountType>(AccountForm.AccountType),
                Balance = AccountForm.InitialDeposit,
                CreatedDate = DateTime.Now
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // TODO: Save KYC documents to storage
            // TODO: Send email notification

            return RedirectToPage("/Index");
        }

        private string GenerateAccountNumber()
        {
            // Generate a 12-digit account number
            // Format: YYMM + 8 random digits
            var prefix = DateTime.Now.ToString("yyMM");
            var random = new Random();
            var suffix = random.Next(10000000, 99999999).ToString();
            return prefix + suffix;
        }
    }
} 