using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Server.Models;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Settings;
using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailMessage emailMessage)
        {
            try
            {
                await _emailService.SendEmailAsync(emailMessage);
                return Ok(new { message = "Email başarıyla gönderildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email gönderimi sırasında hata oluştu");
                return StatusCode(500, "Email gönderilirken bir hata oluştu.");
            }
        }

        [HttpPost("welcome")]
        public async Task<IActionResult> SendWelcomeEmail([FromBody] WelcomeEmailRequest request)
        {
            try
            {
                await _emailService.SendWelcomeEmailAsync(request.Email, request.UserName);
                return Ok(new { message = "Hoş geldiniz emaili gönderildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hoş geldiniz emaili gönderimi sırasında hata oluştu");
                return StatusCode(500, "Email gönderilirken bir hata oluştu.");
            }
        }
    }
}