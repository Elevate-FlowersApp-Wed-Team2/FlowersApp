using FlowerApp.Auth.Domain.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.Customer.ChangePassword.Events
{
    public class PasswordChangedEventHandler : INotificationHandler<PasswordChangedEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<PasswordChangedEventHandler> _logger;

        public PasswordChangedEventHandler(
            IEmailSender emailSender,
            ILogger<PasswordChangedEventHandler> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _emailSender.SendAsync(
                    toEmail: notification.Email,
                    subject: "Your password was changed",
                    body: "Your FlowersApp password was just changed. If this wasn't you, please contact support immediately.",
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Password changed for user {UserId} but confirmation email failed to send.",
                    notification.UserId);
            }
        }
    }
}
