namespace FlowersApp.Auth.Shared.Interfaces
{
    
    public interface IPhotoStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName,
            string contentType, CancellationToken cancellationToken = default);

        // NEW — needed for cleanup on DB failure and old-photo deletion
        Task DeleteAsync(string relativeUrl, CancellationToken cancellationToken = default);
    }
}
