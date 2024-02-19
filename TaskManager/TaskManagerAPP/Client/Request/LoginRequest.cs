namespace TaskManagerAPP.Client.Requests
{
    internal class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TwoFactorCode { get; set; }
        public string? TwoFactorRecoveryCode { get; set; }
    }
}
