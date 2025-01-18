using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Account
{
    [Authorize]
    public class TransactionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Transaction> Transactions { get; set; } = new();
        public List<Models.Account> UserAccounts { get; set; } = new();
        public int CurrentAccountId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Get user's accounts
            UserAccounts = await _context.Accounts
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            if (UserAccounts.Any())
            {
                CurrentAccountId = UserAccounts.First().Id;

                // Get transactions for all user accounts
                var accountIds = UserAccounts.Select(a => a.Id).ToList();
                Transactions = await _context.Transactions
                    .Where(t => accountIds.Contains(t.FromAccountId) || accountIds.Contains(t.ToAccountId))
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(10) // Show last 10 transactions
                    .ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostTransferAsync(int fromAccount, string toAccount, decimal amount, string description)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var sourceAccount = await _context.Accounts.FindAsync(fromAccount);
            var destinationAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == toAccount);

            if (sourceAccount == null || destinationAccount == null)
            {
                TempData["ErrorMessage"] = "Invalid account details.";
                return RedirectToPage();
            }

            if (sourceAccount.Balance < amount)
            {
                TempData["ErrorMessage"] = "Insufficient balance.";
                return RedirectToPage();
            }

            // Create transaction
            var transaction = new Transaction
            {
                FromAccountId = sourceAccount.Id,
                ToAccountId = destinationAccount.Id,
                Amount = amount,
                Description = description,
                TransactionType = "TRANSFER",
                Status = "COMPLETED",
                TransactionDate = DateTime.Now
            };

            // Update account balances
            sourceAccount.Balance -= amount;
            destinationAccount.Balance += amount;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Transfer completed successfully.";
            return RedirectToPage();
        }
    }
} 