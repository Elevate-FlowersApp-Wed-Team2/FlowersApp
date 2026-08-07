namespace FlowerApp.Auth.Domain.Interfaces
{
    public interface IPhotoStorageService
    {
        Task<string> UploadAsync(
         Stream fileStream,
         string fileName,
         string folder,
         CancellationToken cancellationToken);

        Task DeleteAsync(string fileUrl, CancellationToken cancellationToken);
    }
}
