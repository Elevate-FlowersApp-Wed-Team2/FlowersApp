using MediatR;

namespace FlowersApp.Auth.Features.Customer.ChangePassword.Events
{
    public class PasswordChangedEvent : INotification
    {
        public Guid UserId { get; }
        public string Email { get; }

        public PasswordChangedEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
