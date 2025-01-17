using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Account
{
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Models.Account> Accounts { get; set; } = new List<Models.Account>();
        public decimal TotalBalance { get; set; }
        public int TotalAccounts { get; set; }
        public int RecentTransactions { get; set; } = 0; // TODO: Implement transactions

        public async Task OnGetAsync()
        {
            Accounts = await _context.Accounts
                .Include(a => a.Customer)
                .Where(a => a.IsActive)
                .ToListAsync();

            TotalBalance = Accounts.Sum(a => a.Balance);
            TotalAccounts = Accounts.Count;
        }
    }
} 