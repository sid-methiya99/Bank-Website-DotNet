using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin.KYC
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<KYCDocument> Documents { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalDocuments { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, string? status = null, string? type = null)
        {
            PageNumber = pageNumber;

            var query = _context.KYCDocuments
                .Include(d => d.User)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                bool isVerified = status.ToLower() == "verified";
                query = query.Where(d => d.IsVerified == isVerified);
            }

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(d => d.DocumentType == type);
            }

            // Get total count for pagination
            TotalDocuments = await query.CountAsync();

            // Get paginated results
            Documents = await query
                .OrderByDescending(d => d.UploadDate)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
} 