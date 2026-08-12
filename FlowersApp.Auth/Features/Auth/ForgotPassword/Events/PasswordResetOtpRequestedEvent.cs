using MediatR;

namespace FlowersApp.Auth.Features.Auth.ForgotPassword.Events;

public record PasswordResetOtpRequestedEvent(Guid UserId, string Email, string Otp) : INotification;
