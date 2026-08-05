using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Driver.SubmitApplication;

public record SubmitApplicationCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string NationalIdNumber,
    string LicenseNumber,
    string VehicleType,
    string VehiclePlateNumber,
    string Password,
    string ConfirmPassword,
    List<IFormFile> Documents)
    : ICommand<SubmitApplicationResponse>;

public record SubmitApplicationResponse(string ApplicationId, string Status);


