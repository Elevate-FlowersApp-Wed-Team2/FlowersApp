using FlowerApp.Auth.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace FlowerApp.Auth.Domain
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public Gender? Gender { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
