namespace TaskManagerShared.Client.Requests
{
    internal class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? TwoFactorCode { get; set; } // Future improvement. Not used;
        public string? TwoFactorRecoveryCode { get; set; } // Future improvement. Not used;
    }
}
