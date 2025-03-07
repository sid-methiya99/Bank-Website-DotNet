using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class FeedbacksModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public FeedbacksModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Feedback> Feedbacks { get; set; } = new();
        public string? SuccessMessage { get; set; }

        public async Task OnGetAsync()
        {
            Feedbacks = await _context.Feedbacks
                .Include(f => f.User)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(string id, string status)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback != null)
            {
                feedback.Status = status;
                await _context.SaveChangesAsync();
                SuccessMessage = "Feedback status updated successfully.";
            }
            return RedirectToPage();
        }
    }
} 