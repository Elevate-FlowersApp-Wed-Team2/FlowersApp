using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Infrastructure.Photos
{
    public class LocalPhotoStorageService : IPhotoStorageService
    {
        private const string UploadsFolder = "uploads/profile-photos";

        private readonly IWebHostEnvironment _env;

        public LocalPhotoStorageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default)
        {
            var uploadsDirectory = Path.Combine(_env.WebRootPath, UploadsFolder);
            Directory.CreateDirectory(uploadsDirectory);


            var safeFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            var filePath = Path.Combine(uploadsDirectory, safeFileName);

            await using (var output = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(output, cancellationToken);
            }

            return $"/{UploadsFolder}/{safeFileName}";
        }
    }
}
