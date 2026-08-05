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
}
