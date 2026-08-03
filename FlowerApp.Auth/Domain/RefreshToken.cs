namespace FlowerApp.Auth.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = default!;
    }

}
