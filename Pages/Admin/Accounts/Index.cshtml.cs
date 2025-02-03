using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin.Accounts
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<BankManagementSystem.Models.Account> Accounts { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalAccounts { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, string? status = null)
        {
            PageNumber = pageNumber;

            var query = _context.Accounts
                .Include(a => a.User)
                .ThenInclude(u => u.UserProfile)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(a => a.Status == status);
            }

            // Get total count for pagination
            TotalAccounts = await query.CountAsync();

            // Get paginated results
            Accounts = await query
                .OrderByDescending(a => a.OpenDate)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostActivateAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            account.Status = "ACTIVE";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account has been activated successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSuspendAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                return NotFound();
            }

            account.Status = "SUSPENDED";
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Account has been suspended successfully.";
            return RedirectToPage();
        }
    }
} 