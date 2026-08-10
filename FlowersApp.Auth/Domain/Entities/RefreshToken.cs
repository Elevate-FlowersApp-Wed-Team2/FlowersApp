namespace FlowersApp.Auth.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Foreign Key AppUser
        public Guid UserId { get; set; }
        public AppUser User { get; set; } = null!;
    }
}
