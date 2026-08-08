namespace FlowerApp.Auth.Domain
{
    public sealed class GuestSession
    {
        public Guid Id { get; set; }
        public string Name { get; set; }= "Guest";
        public DateTime CreatedAt { get;  set; }
        public DateTime ExpiresAt { get;  set; }
        public string? UserId { get;  set; }
        public GuestSession() { }

        public GuestSession(DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            ExpiresAt = expiresAt;
        }
        public void LinkToUser(string userId)
        {
            UserId = userId;
        }
    }
}
