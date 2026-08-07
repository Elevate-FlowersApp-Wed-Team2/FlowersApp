namespace FlowersApp.Auth.Shared.Services;

public class DocumentService
{
    private readonly string _basePath;
    private readonly ILogger<DocumentService> _logger;

    public DocumentService(ILogger<DocumentService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _basePath = configuration["LocalStoragePath"] ?? "docs";
    }

    public async Task<string> UploadDocumentAsync(
        string storagePath,
        Stream fileStream,
        CancellationToken cancellationToken)
    {
        try
        {
            // Sanitize the path to prevent directory traversal
            var sanitizedPath = SanitizePath(storagePath);
            var fullPath = Path.Combine(_basePath, sanitizedPath);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Upload the file - renamed variable to avoid conflict with parameter
            using var fileStreamOutput = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await fileStream.CopyToAsync(fileStreamOutput, cancellationToken);

            _logger.LogInformation($"File uploaded successfully: {storagePath}");

            // Return the storage path for reference
            return storagePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error uploading file: {storagePath}");
            throw new Exception($"Failed to upload document: {ex.Message}");
        }
    }

    public async Task<Stream> GetDocumentAsync(string storagePath, CancellationToken cancellationToken)
    {
        try
        {
            var sanitizedPath = SanitizePath(storagePath);
            var fullPath = Path.Combine(_basePath, sanitizedPath);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Document not found: {storagePath}");
            }

            var memoryStream = new MemoryStream();
            using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            return memoryStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving file: {storagePath}");
            throw;
        }
    }

    public async Task DeleteDocumentAsync(string storagePath, CancellationToken cancellationToken)
    {
        try
        {
            var sanitizedPath = SanitizePath(storagePath);
            var fullPath = Path.Combine(_basePath, sanitizedPath);

            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath), cancellationToken);
                _logger.LogInformation($"File deleted: {storagePath}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting file: {storagePath}");
            throw;
        }
    }

    public async Task<bool> DocumentExistsAsync(string storagePath, CancellationToken cancellationToken)
    {
        try
        {
            var sanitizedPath = SanitizePath(storagePath);
            var fullPath = Path.Combine(_basePath, sanitizedPath);

            return await Task.FromResult(File.Exists(fullPath));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error checking file existence: {storagePath}");
            return false;
        }
    }

    private string SanitizePath(string path)
    {
        // Remove any directory traversal attempts
        var invalidChars = Path.GetInvalidPathChars();
        foreach (var c in invalidChars)
        {
            path = path.Replace(c.ToString(), "");
        }

        // Normalize path separators
        path = path.Replace('/', Path.DirectorySeparatorChar);
        path = path.Replace('\\', Path.DirectorySeparatorChar);

        // Remove any relative path components
        var parts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var sanitizedParts = new List<string>();

        foreach (var part in parts)
        {
            if (part == ".." || part == ".")
                continue;
            sanitizedParts.Add(part);
        }

        return string.Join(Path.DirectorySeparatorChar, sanitizedParts);
    }
}