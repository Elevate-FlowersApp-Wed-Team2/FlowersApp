using FlowersApp.Auth.Domain.Enums;

namespace FlowersApp.Auth.Domain.Entities;

public class AppUser :BaseEntity
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public string HashedPassword { get; set; }
    public Gender Gender { get; set; }
    public DateTime CreatedAt { get ; set ; }
}
