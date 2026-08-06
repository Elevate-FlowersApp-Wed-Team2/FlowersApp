using MediatR;

namespace FlowerApp.Auth.Features.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<LogoutDto>;
}
