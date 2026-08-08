namespace FlowerApp.Auth.Common.Enums
{
    public enum ErrorCode
    {
        None,

        // Auth
        InvalidCredentials = 100,
        AccountInactive = 101,

        // Customer Registration
        EmailAlreadyRegistered = 200,
        PhoneAlreadyRegistered = 201,
        InvalidEmail = 202,
        InvalidPhoneNumber = 203,
        InvalidPassword = 204,
        PasswordMismatch = 205,
        InvalidFullName = 206,
        InvalidGender = 207,
        RegistrationFailed = 208
    }
}
