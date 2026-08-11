namespace FlowersApp.Auth.Shared.Constants;

public static class Endpoints
{
    private const string BasePath = "api/v1";
    public static class DriverApplications
    {
        private const string DriverApplicationBase = $"{BasePath}/driver-applications";
        public const string GetById = $"{DriverApplicationBase}/{{id}}";
    }

    public static class Drivers
    {
        public const string BaseDrivers = $"{BasePath}/drivers";
        public const string ApplyDriver = $"{BaseDrivers}/apply";
    }
    public static class Users
    {
        private const string UsersMeBase = $"{BasePath}/users/me";
        public const string ChangePassword = $"{UsersMeBase}/change-password";
        public const string UpdateProfile = $"{UsersMeBase}/profile"; // PUT /api/users/me/profile
        public const string Logout = $"{UsersMeBase}/logout"; // POST /api/users/me/logout
    }
    public static class Auth
    {
        private const string AuthBase = $"{BasePath}";
        public const string Register = $"{AuthBase}/register";
        public const string UserLogin = "auth/user/login";
        public const string DriverLogin = "auth/driver/login";
    }
}
