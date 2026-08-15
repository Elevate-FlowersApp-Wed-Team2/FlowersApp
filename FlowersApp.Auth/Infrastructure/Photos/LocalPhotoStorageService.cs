using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Infrastructure.Photos
{
    public class LocalPhotoStorageService : IPhotoStorageService
    {
        private const string UploadsFolder = "uploads/profile-photos";
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        private readonly IWebHostEnvironment _env;
        private readonly ILogger<LocalPhotoStorageService> _logger; // NEW

        public LocalPhotoStorageService(IWebHostEnvironment env,
            ILogger<LocalPhotoStorageService> logger) // NEW param
        {
            _env = env;
            _logger = logger;
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName,
            string contentType, CancellationToken cancellationToken = default)
        {
            if (fileStream.Length > MaxFileSizeBytes)
                throw new InvalidOperationException($"File exceeds the {MaxFileSizeBytes / 1024 / 1024} MB limit.");

            var uploadsDirectory = Path.Combine(_env.WebRootPath, UploadsFolder);
            Directory.CreateDirectory(uploadsDirectory);

            var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(uploadsDirectory, safeFileName);

            await using var output = new FileStream(filePath, FileMode.Create);
            await fileStream.CopyToAsync(output, cancellationToken);

            return $"/{UploadsFolder}/{safeFileName}";
        }

        public Task DeleteAsync(string relativeUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                var filePath = Path.Combine(
                    _env.WebRootPath,
                    relativeUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete photo at {Url}", relativeUrl);
            }
            return Task.CompletedTask;
        }
    }
}
