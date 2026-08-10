namespace FlowersApp.Auth.Shared.Response;

public enum ResultCode
{
    //Driver
    DriverIsAlreadyExist = 100,
    DriverNotFound =101,

    // Vehicle
    InvalidVehicleNumber = 200,
    VehicleExist = 201,
    VehicleNotFound =202,


    //Application
    FailedToSubmitApplication = 300,
    ApplicationSubmittedSuccessfully = 301,
    ApplicationCreatedSuccessfully = 302,
    InvalidGender = 303,
    UserAlreadyApplied = 304,
    UserNotApplied = 305,
    NameRequired = 306,
    InvalidEmail = 307,
    InvalidPhone = 308,
    NameChractersMismatch = 309,
    EmailRequired = 310,
    EmailTooLong = 311,
    PhoneRequired = 312,
    NidRequired = 313,
    NidTooLong = 314,
    LicenceNumberRequired = 315,
    LicenceNumberTooLong = 316,
    PasswordRequired = 317,
    PasswordTooShort = 318,
    PasswordMissingUppercase = 319,
    PasswordMissingDigit = 320,
    PasswordMismatch = 321,
    LicenceImageInvalid = 322,
    NidImageInvalid = 323,

    // Document
    DocumentCreated = 400,

    // User 
    UserNotFound = 500,
    NewPasswordSameAsCurrent = 501,
    CurrentPasswordIncorrect = 502,
    PasswordChangeFailed = 503,
    PasswordChangedSuccessfully = 504,
}
