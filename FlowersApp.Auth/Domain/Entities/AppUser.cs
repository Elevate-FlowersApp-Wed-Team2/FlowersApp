using FlowersApp.Auth.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FlowersApp.Auth.Domain.Entities;

public class AppUser :IdentityUser<Guid>
{
    public string FullName { get; set; }
    public Gender Gender { get; set; }
    public DateTime CreatedAt { get ; set ; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public string? ProfilePhotoUrl { get; set; }
}
