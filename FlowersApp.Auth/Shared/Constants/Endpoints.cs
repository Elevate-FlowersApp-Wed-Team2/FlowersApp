namespace FlowersApp.Auth.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";
    public static class DriverApplications
    {
        private const string DriverApplicationBase = $"{BasePath}/driver-applications";
        public const string Apply = $"{DriverApplicationBase}/apply";
        public const string GetById = $"{DriverApplicationBase}/{{id}}";
    }
}
