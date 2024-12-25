namespace EducationManagementSystem.Server.Data.DTOs
{
    public class WelcomeEmailRequest
    {
        public required string Email { get; set; }
        public required string UserName { get; set; }
    }
}
