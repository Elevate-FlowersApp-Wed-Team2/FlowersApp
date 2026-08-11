using FlowersApp.Auth.Shared.Interfaces;
using MediatR;

namespace FlowersApp.Auth.Features.Customer.Logout
{
    public record LogoutCommand : ICommand<Unit>;
}
