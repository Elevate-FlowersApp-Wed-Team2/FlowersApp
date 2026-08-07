using FlowerApp.Auth.Domain.Interfaces;

namespace FlowerApp.Auth.Infrastructure.Services
{
    public class LocalPhotoStorageService : IPhotoStorageService
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5MB

        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<LocalPhotoStorageService> _logger;

        public LocalPhotoStorageService(
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            ILogger<LocalPhotoStorageService> logger)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string folder,
            CancellationToken cancellationToken)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(ext))
                throw new InvalidOperationException("Only jpg, jpeg, png, and webp files are allowed.");

            if (fileStream.Length > MaxFileSizeBytes)
                throw new InvalidOperationException("File size must not exceed 5MB.");

            var uploadsRoot = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsRoot);

            var uniqueFileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsRoot, uniqueFileName);

            fileStream.Position = 0;
            await using (var output = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output, cancellationToken);
            }

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request is not null
                ? $"{request.Scheme}://{request.Host}"
                : string.Empty;

            var publicUrl = $"{baseUrl}/uploads/{folder}/{uniqueFileName}";

            _logger.LogInformation("Photo uploaded: {Path}", publicUrl);
            return publicUrl;
        }

        public Task DeleteAsync(string fileUrl, CancellationToken cancellationToken)
        {
            try
            {
                var relativePath = new Uri(fileUrl).AbsolutePath.TrimStart('/');
                var fullPath = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, relativePath);

                if (File.Exists(fullPath))
                    File.Delete(fullPath);
            }
            catch (Exception ex)
            {
                // Deletion failures shouldn't block the calling operation — log and move on.
                _logger.LogWarning(ex, "Failed to delete photo at {Url}", fileUrl);
            }

            return Task.CompletedTask;
        }
    }
}
