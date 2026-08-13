using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.Auth.VerifyOtp;

public record VerifyOtpCommand(string Email, string Otp) : ICommand<VerifyOtpResponse>;

public record VerifyOtpResponse(string ResetToken);
