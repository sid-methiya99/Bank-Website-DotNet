using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalUsers { get; set; }
        public int TotalAccounts { get; set; }
        public int PendingKYC { get; set; }
        public int TotalTransactions { get; set; }
        public List<ApplicationUser> RecentUsers { get; set; } = new();
        public List<PendingApproval> PendingApprovals { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get statistics
            TotalUsers = await _context.Users.CountAsync();
            TotalAccounts = await _context.Accounts.CountAsync();
            PendingKYC = await _context.KYCDocuments.CountAsync(k => !k.IsVerified);
            TotalTransactions = await _context.Transactions.CountAsync();

            // Get recent users with their profiles
            RecentUsers = await _context.Users
                .Include(u => u.UserProfile)
                .OrderByDescending(u => u.UserProfile.LastLoginDate)
                .Take(5)
                .ToListAsync();

            // Get pending approvals
            var pendingKycDocs = await _context.KYCDocuments
                .Include(k => k.User)
                .Where(k => !k.IsVerified)
                .Take(5)
                .ToListAsync();

            var pendingAccounts = await _context.Accounts
                .Include(a => a.User)
                .Where(a => a.Status == "PENDING")
                .Take(5)
                .ToListAsync();

            PendingApprovals = new List<PendingApproval>();

            foreach (var doc in pendingKycDocs)
            {
                PendingApprovals.Add(new PendingApproval
                {
                    Type = "KYC Verification",
                    Details = $"{doc.DocumentType} - {doc.User?.Email}",
                    ActionUrl = $"/Admin/KYC/Review/{doc.Id}"
                });
            }

            foreach (var account in pendingAccounts)
            {
                PendingApprovals.Add(new PendingApproval
                {
                    Type = "Account Approval",
                    Details = $"Account #{account.AccountNumber} - {account.User?.Email}",
                    ActionUrl = $"/Admin/Accounts/Review/{account.Id}"
                });
            }

            return Page();
        }
    }

    public class PendingApproval
    {
        public string Type { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string ActionUrl { get; set; } = string.Empty;
    }
} 