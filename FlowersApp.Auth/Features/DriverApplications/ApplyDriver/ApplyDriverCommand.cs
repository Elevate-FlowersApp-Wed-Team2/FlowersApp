using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.DriverApplications.SubmitApplication;

public record ApplyDriverCommand(
   string Name,
    string Email,
    string Password,
    string ConfirmPassword,
    string Phone,
    string Gender,
    Guid VehicleId,
    string VehicleNumber,
    string LicenceNumber,
    IFormFile LicenceImage,
    string Nid,
    IFormFile NidImage,
    string? FcmToken = null)
    : ICommand<ApplyDriverResponse>;

public record ApplyDriverResponse(string ApplicationId, string Status);


