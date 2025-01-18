using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagementSystem.Models;
using BankManagementSystem.Data;
using BankManagementSystem.Services;

namespace BankManagementSystem.Pages.Account
{
    [Authorize]
    public class KYCUploadModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly FileUploadService _fileUploadService;

        public KYCUploadModel(ApplicationDbContext context, FileUploadService fileUploadService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
        }

        public KYCDocument? AadhaarDocument { get; set; }
        public KYCDocument? PanDocument { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            AadhaarDocument = await _context.KYCDocuments
                .FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == "AADHAAR");
            
            PanDocument = await _context.KYCDocuments
                .FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == "PAN");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string documentType, string documentNumber, IFormFile file)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file to upload.");
                return Page();
            }

            var (success, filePath, error) = await _fileUploadService.UploadKYCDocumentAsync(file, userId, documentType);
            if (!success)
            {
                ModelState.AddModelError("", $"Failed to upload file: {error}");
                return Page();
            }

            var document = new KYCDocument
            {
                UserId = userId,
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                FilePath = filePath,
                UploadDate = DateTime.Now,
                IsVerified = false
            };

            _context.KYCDocuments.Add(document);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{documentType} document uploaded successfully. It will be verified shortly.";
            return RedirectToPage();
        }
    }
} 