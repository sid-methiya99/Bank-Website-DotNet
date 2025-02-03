using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Admin.Users
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public class UserViewModel
        {
            public string Id { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public bool IsActive { get; set; }
            public UserProfile? UserProfile { get; set; }
        }

        public List<UserViewModel> Users { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalUsers { get; set; }

        public async Task OnGetAsync(int pageNumber = 1, string? role = null, string? status = null)
        {
            PageNumber = pageNumber;

            var query = _context.Users
                .Include(u => u.UserProfile)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(status))
            {
                bool isActive = status.ToLower() == "active";
                query = query.Where(u => u.LockoutEnd == null == isActive);
            }

            // Get total count for pagination
            TotalUsers = await query.CountAsync();

            // Get paginated results
            var users = await query
                .OrderBy(u => u.Email)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            // Convert to view model
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? string.Empty,
                    Role = roles.FirstOrDefault() ?? "User",
                    IsActive = user.LockoutEnd == null,
                    UserProfile = await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == user.Id)
                });
            }

            // Filter by role if specified
            if (!string.IsNullOrEmpty(role))
            {
                Users = Users.Where(u => u.Role == role).ToList();
            }
        }

        public async Task<IActionResult> OnPostDeactivateAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Lock the user account
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            TempData["SuccessMessage"] = "User has been deactivated successfully.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostActivateAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Remove the lockout
            await _userManager.SetLockoutEndDateAsync(user, null);
            TempData["SuccessMessage"] = "User has been activated successfully.";
            return RedirectToPage();
        }
    }
} 