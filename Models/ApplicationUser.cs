using Microsoft.AspNetCore.Identity;

namespace BankManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsFirstLogin { get; set; } = true;
        public bool HasAcceptedTerms { get; set; } = false;
        public string PreferredTheme { get; set; } = "light";
        public virtual UserProfile? UserProfile { get; set; }
    }
} 