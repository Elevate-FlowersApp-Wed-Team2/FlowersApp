using FlowersApp.Auth.Domain.Entities;
using FlowersApp.Auth.Shared.Interfaces;

namespace FlowersApp.Auth.Features.RefreshTokens;

public class SaveRefreshTokenCommand : ICommand<Guid>
{
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
