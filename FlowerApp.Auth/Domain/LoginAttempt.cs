namespace FlowerApp.Auth.Domain
{
    public sealed class LoginAttempt
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Guest";
        public string? Email { get; set; }= default!;
        public DateTime AttemptedAt { get; set; }
        public bool IsSuccess { get; set; }
        public string IpAddress { get; set; } = default!;
    }
}
