using MediatR;
using Shared;

namespace FlowerApp.Auth.Features.Account.ChangePassword
{
    public record ChangePasswordCommand(
    Guid UserId,
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
    ) : IRequest<Result>;
}
