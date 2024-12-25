using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendEmailAsync(EmailMessage emailMessage);
        Task SendWelcomeEmailAsync(string to, string userName);
        Task SendPasswordResetEmailAsync(string to, string resetToken);
        Task SendCourseEnrollmentConfirmationAsync(string to, string courseName);
    }
}