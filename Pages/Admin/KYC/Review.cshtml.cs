using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin.KYC
{
    [Authorize(Roles = "Admin")]
    public class ReviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ReviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public KYCDocument? Document { get; set; }
        public UserProfile? UserProfile { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Document = await _context.KYCDocuments
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (Document == null)
            {
                return NotFound();
            }

            UserProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == Document.UserId);

            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var document = await _context.KYCDocuments.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            document.IsVerified = true;
            await _context.SaveChangesAsync();

            // Check if all required documents are verified
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.Id == document.UserId);

            if (user != null)
            {
                var allDocumentsVerified = await _context.KYCDocuments
                    .Where(d => d.UserId == user.Id)
                    .AllAsync(d => d.IsVerified);

                if (allDocumentsVerified)
                {
                    // Update pending accounts to active
                    var pendingAccounts = await _context.Accounts
                        .Where(a => a.UserId == user.Id && a.Status == "PENDING")
                        .ToListAsync();

                    foreach (var account in pendingAccounts)
                    {
                        account.Status = "ACTIVE";
                    }

                    await _context.SaveChangesAsync();
                }
            }

            TempData["SuccessMessage"] = "Document has been approved successfully.";
            return RedirectToPage("/Admin/KYC/Index");
        }

        public async Task<IActionResult> OnPostRejectAsync(int id)
        {
            var document = await _context.KYCDocuments.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            _context.KYCDocuments.Remove(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document has been rejected and removed.";
            return RedirectToPage("/Admin/KYC/Index");
        }
    }
} 