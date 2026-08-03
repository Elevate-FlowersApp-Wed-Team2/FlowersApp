namespace FlowerApp.Auth.Domain
{
    public class PasswordResetOtp
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = default!;
    }

}
