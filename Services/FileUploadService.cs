using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BankManagementSystem.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private const string UPLOAD_DIRECTORY = "uploads/kyc";
        private readonly string[] ALLOWED_EXTENSIONS = { ".jpg", ".jpeg", ".png", ".pdf" };
        private const long MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<(bool success, string filePath, string errorMessage)> UploadKYCDocumentAsync(IFormFile file, string userId, string documentType)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return (false, string.Empty, "No file uploaded");

                if (file.Length > MAX_FILE_SIZE)
                    return (false, string.Empty, "File size exceeds 5MB limit");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!ALLOWED_EXTENSIONS.Contains(extension))
                    return (false, string.Empty, "Invalid file type. Allowed types: jpg, jpeg, png, pdf");

                // Create upload directory if it doesn't exist
                var uploadPath = Path.Combine(_environment.WebRootPath, UPLOAD_DIRECTORY, userId);
                Directory.CreateDirectory(uploadPath);

                // Generate unique filename
                var fileName = $"{documentType}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadPath, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return relative path for database storage
                var relativePath = Path.Combine(UPLOAD_DIRECTORY, userId, fileName);
                return (true, relativePath.Replace("\\", "/"), string.Empty);
            }
            catch (Exception ex)
            {
                return (false, string.Empty, $"Error uploading file: {ex.Message}");
            }
        }
    }
} 