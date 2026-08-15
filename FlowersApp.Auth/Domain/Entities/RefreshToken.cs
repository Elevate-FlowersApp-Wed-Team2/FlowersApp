namespace FlowersApp.Auth.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        /// <summary>SHA-256 hash of the raw refresh token presented to clients.</summary>
        public string Token { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public Guid FamilyId { get; set; }
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }
        public Guid? ReplacedByTokenId { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;

        public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;
    }
}
