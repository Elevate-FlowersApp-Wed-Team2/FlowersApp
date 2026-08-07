using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.DriverApplications.ApplyDriver;

public record ApplyDriverOrchestrator(
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
    : ICommand<ApplyDriverOrchestratorResponse>;

public record ApplyDriverOrchestratorResponse(string ApplicationId, string Status);


