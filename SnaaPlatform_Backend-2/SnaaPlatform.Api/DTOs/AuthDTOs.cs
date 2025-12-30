namespace SnaaPlatform.Api.DTOs
{
    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty; // "Individual" or "Facility"
        public string? CommercialRegistration { get; set; }
        public string? Specialization { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
    }
}

    public class UpdateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string? CommercialRegistration { get; set; }
        public string? Specialization { get; set; }
    }
