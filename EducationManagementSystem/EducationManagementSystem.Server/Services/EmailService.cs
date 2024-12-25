using System.Net;
using System.Net.Mail;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Models;
using Microsoft.Extensions.Options;
using EducationManagementSystem.Server.Settings;

namespace EducationManagementSystem.Server.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            var emailMessage = new EmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                IsHtml = isHtml
            };

            await SendEmailAsync(emailMessage);
        }

        public async Task SendEmailAsync(EmailMessage emailMessage)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
                {
                    Port = _emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword),
                    EnableSsl = true,
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
                    Subject = emailMessage.Subject,
                    Body = emailMessage.Body,
                    IsBodyHtml = emailMessage.IsHtml
                };

                message.To.Add(emailMessage.To);

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Email sent successfully to {emailMessage.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {emailMessage.To}");
                throw;
            }
        }

        public async Task SendWelcomeEmailAsync(string to, string userName)
        {
            var subject = "Hoş Geldiniz!";
            var body = $@"
                <h2>Merhaba {userName},</h2>
                <p>Eğitim yönetim sistemimize hoş geldiniz!</p>
                <p>Hesabınız başarıyla oluşturuldu.</p>
                <br>
                <p>İyi çalışmalar dileriz!</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetToken)
        {
            var subject = "Şifre Sıfırlama";
            var resetLink = $"{_emailSettings.WebsiteUrl}/reset-password?token={resetToken}";
            var body = $@"
                <h2>Şifre Sıfırlama İsteği</h2>
                <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                <p><a href='{resetLink}'>Şifremi Sıfırla</a></p>
                <br>
                <p>Bu işlemi siz yapmadıysanız, lütfen bu e-postayı dikkate almayın.</p>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendCourseEnrollmentConfirmationAsync(string to, string courseName)
        {
            var subject = $"{courseName} Dersine Kayıt Onayı";
            var body = $@"
                <h2>Ders Kaydı Onaylandı</h2>
                <p>{courseName} dersine kaydınız başarıyla tamamlanmıştır.</p>
                <p>Ders programınızı kontrol etmeyi unutmayın.</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
}