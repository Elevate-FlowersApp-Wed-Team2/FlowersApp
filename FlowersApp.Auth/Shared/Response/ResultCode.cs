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
    NameCharactersMismatch = 309,
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

    // User (cusomer + driver)
    UserNotFound = 500,
    NewPasswordSameAsCurrent = 501,
    CurrentPasswordIncorrect = 502,
    PasswordChangeFailed = 503,
    PasswordChangedSuccessfully = 504,
    ProfileUpdatedSuccessfully = 505,
    InvalidProfilePhoto = 506,
    PhotoUploadFailed = 507,
    PhoneAlreadyInUse = 508,
    ProfileUpdateFailed = 509,
    NothingToUpdate = 510,
    LoggedOutSuccessfully = 511,

    // Password reset / OTP (SCRUM-14)
    PasswordResetOtpSent = 520,
    OtpRequired = 521,
    InvalidOrExpiredOtp = 522,
    OtpExpired = 523,
    OtpMaxAttemptsExceeded = 524,
    OtpResendTooSoon = 525,
    OtpVerifiedSuccessfully = 526,
    InvalidOrExpiredResetToken = 527,
    PasswordResetSuccessful = 528,

    // Sessions / refresh (SCRUM-19)
    TokenRefreshedSuccessfully = 530,
    InvalidRefreshToken = 531,
    RefreshTokenReuseDetected = 532,
    SessionsRetrievedSuccessfully = 533,
    SessionRevokedSuccessfully = 534,
    SessionNotFound = 535,

    // Registration
    RegistrationSuccessful = 600,
    EmailAlreadyRegistered = 601,
    PhoneAlreadyRegistered = 602,
    RegistrationFailed = 603,

    // Login & Auth (SCRUM-12)
    InvalidCredentials = 700,
    DriverAccountNotApproved = 701,
    DriverApplicationRejected = 702,
    TooManyFailedAttempts = 703,
    LoginSuccessful = 704,
}
