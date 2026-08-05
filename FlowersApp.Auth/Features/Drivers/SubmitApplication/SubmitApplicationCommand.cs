using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Drivers.SubmitApplication;

public record SubmitApplicationCommand(
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
    : ICommand<SubmitApplicationResponse>;

public record SubmitApplicationResponse(string ApplicationId, string Status);


