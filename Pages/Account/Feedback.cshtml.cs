using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using BankManagementSystem.Data;
using BankManagementSystem.Models;

namespace BankManagementSystem.Pages.Account
{
    [Authorize]
    public class FeedbackModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FeedbackModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public FeedbackInput FeedbackInput { get; set; } = new();

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ErrorMessage = "User not found.";
                    return Page();
                }

                var feedback = new Feedback
                {
                    UserId = user.Id,
                    Subject = FeedbackInput.Subject,
                    Message = FeedbackInput.Message,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending"
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                SuccessMessage = "Thank you for your feedback! We will review it shortly.";
                FeedbackInput = new FeedbackInput(); // Clear the form
                return Page();
            }
            catch (Exception ex)
            {
                ErrorMessage = "An error occurred while submitting your feedback. Please try again.";
                return Page();
            }
        }
    }

    public class FeedbackInput
    {
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(200, ErrorMessage = "Subject cannot be longer than 200 characters")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, ErrorMessage = "Message cannot be longer than 1000 characters")]
        public string Message { get; set; } = string.Empty;
    }
} 