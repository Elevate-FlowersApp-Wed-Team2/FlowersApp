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

    // Document
    DocumentCreated = 400,
}
