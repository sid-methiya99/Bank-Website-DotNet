using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankManagementSystem.Pages
{
    public class ContactModel : PageModel
    {
        [TempData]
        public string? SuccessMessage { get; set; }
        
        [TempData]
        public string? ErrorMessage { get; set; }

        [BindProperty]
        public ContactFormModel Contact { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Please fill in all required fields correctly.";
                return Page();
            }

            // Here you would typically send an email or save to database
            // For now, we'll just show a success message
            SuccessMessage = "Thank you for your message. We'll get back to you soon!";
            return RedirectToPage();
        }
    }

    public class ContactFormModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
} 