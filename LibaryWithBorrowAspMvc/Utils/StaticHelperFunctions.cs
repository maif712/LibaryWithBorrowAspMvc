using Domain.Common;
using Microsoft.Extensions.Hosting;

namespace LibaryWithBorrowAspMvc.Utils
{
    public static class StaticHelperFunctions
    {
        public static string SaveImage(IFormFile file, string existingPath = null)
        {
            if (file == null || file.Length == 0)
                return existingPath; // nothing uploaded, keep old path

            // ✅ Enforce max size (5 MB)
            const long maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
            if (file.Length > maxFileSize)
                throw new InvalidOperationException("File size cannot exceed 5 MB.");

            // ✅ Validate file extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
                throw new InvalidOperationException("Only image files (.jpg, .jpeg, .png, .gif, .webp) are allowed.");

            // ✅ Validate MIME type
            if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Invalid file type. Only image files are allowed.");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Delete old image if provided
            if (!string.IsNullOrEmpty(existingPath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPath.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            // Generate unique filename
            var uniqueFileName = Guid.NewGuid().ToString() + extension;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return relative path for DB
            return "/images/" + uniqueFileName;
        }

        public static void DeleteImage(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
