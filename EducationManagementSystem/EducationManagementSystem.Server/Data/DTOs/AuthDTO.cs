namespace EducationManagementSystem.Server.Data.DTOs
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class RegisterDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string StudentNumber { get; set; }
        public int DepartmentId { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class RegisterResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int StudentId { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class ForgotPasswordDTO
    {
        public required string Email { get; set; }
    }

    public class ResetPasswordDTO
    {
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}