using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace FlowersApp.Notification.Extensions;

public static class FirebaseExtensions
{
    public static IServiceCollection AddFirebase(this IServiceCollection services, IConfiguration configuration)
    {
        var credentialsPath = configuration["Firebase:CredentialsPath"];

        if (string.IsNullOrWhiteSpace(credentialsPath))
        {
            throw new InvalidOperationException("Firebase credentials path is not configured.");
        }

        var resolvedPath = Path.IsPathRooted(credentialsPath)
            ? credentialsPath
            : Path.Combine(Directory.GetCurrentDirectory(), credentialsPath);

        if (!File.Exists(resolvedPath))
        {
            throw new FileNotFoundException($"Firebase credentials file was not found at path: '{resolvedPath}'");
        }

        if (FirebaseApp.DefaultInstance == null)
        {
            lock (typeof(FirebaseExtensions))
            {
                if (FirebaseApp.DefaultInstance == null)
                {
                    try
                    {
                        var jsonContent = File.ReadAllText(resolvedPath);
                        if (jsonContent.Contains("\\n"))
                        {
                            jsonContent = jsonContent.Replace("\\n", "\n");
                        }

                        var credential = GoogleCredential.FromJson(jsonContent);

                        FirebaseApp.Create(new AppOptions
                        {
                            Credential = credential
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[FirebaseExtensions] Warning: Could not initialize FirebaseApp from '{resolvedPath}': {ex.Message}");
                    }
                }
            }
        }

        return services;
    }
}
