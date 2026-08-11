namespace FlowersApp.Auth.Shared.Interfaces
{
    public interface IPhotoStorageService
    {
        Task<string> UploadAsync(
            Stream fileStream,
            string fileName,
            string contentType,
            CancellationToken cancellationToken = default);
    }
}
