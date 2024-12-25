using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationManagementSystem.Server.Models;
using EducationManagementSystem.Server.Data;
using EducationManagementSystem.Server.Data.DTOs;
using EducationManagementSystem.Server.Interfaces;
using System.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace EducationManagementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;

        public AuthController(
            ApplicationDbContext context,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IEmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginDTO loginDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                {
                    return Unauthorized("E-posta veya şifre yanlış");
                }

                var token = GenerateJwtToken(user);

                var isStudent = await _context.Students.AnyAsync(s => s.UserId == user.UserId);

                var response = new LoginResponseDTO
                {
                    Token = token,
                    UserId = user.UserId,
                    Email = user.Email,
                    Role = isStudent ? "Student" : "User"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş yapılırken bir hata oluştu");
                return StatusCode(500, "Giriş işlemi sırasında bir hata oluştu");
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponseDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<RegisterResponseDTO>> Register(RegisterDTO registerDto)
        {
            try
            {
                if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
                {
                    return BadRequest("Bu e-posta adresi zaten kullanımda");
                }

                if (await _context.Students.AnyAsync(s => s.StudentNumber == registerDto.StudentNumber))
                {
                    return BadRequest("Bu öğrenci numarası zaten kullanımda");
                }

                var department = await _context.Departments.FindAsync(registerDto.DepartmentId);
                if (department == null)
                {
                    return BadRequest("Geçersiz bölüm ID'si");
                }

                var user = new User
                {
                    Email = registerDto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var student = new Student
                {
                    UserId = user.UserId,
                    StudentNumber = registerDto.StudentNumber,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Email = registerDto.Email,
                    DepartmentId = registerDto.DepartmentId,
                    PhoneNumber = registerDto.PhoneNumber,
                    DateOfBirth = registerDto.DateOfBirth,
                    EnrollmentDate = DateTime.UtcNow
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);

                var response = new RegisterResponseDTO
                {
                    Token = token,
                    UserId = user.UserId,
                    StudentId = student.StudentId,
                    Email = user.Email
                };

                return CreatedAtAction(nameof(Login), response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kayıt olunurken bir hata oluştu");
                return StatusCode(500, "Kayıt işlemi sırasında bir hata oluştu");
            }
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);

                if (user == null)
                {
                    return NotFound("Bu e-posta adresiyle kayıtlı kullanıcı bulunamadı");
                }

                var resetToken = Guid.NewGuid().ToString();
                user.ResetToken = resetToken;
                user.ResetTokenExpiry = DateTime.UtcNow.AddHours(24);
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                var resetLink = $"https://localhost.com/reset-password";

                var emailMessage = new EmailMessage
                {
                    To = user.Email,
                    Subject = "Şifre Sıfırlama Talebi",
                    Body = $"<p>Şifre sıfırlama talebiniz alındı.Eğer işlemi siz yapmadıysanız lütfen destek ile iletişime geçiniz. </p><p><a '>Şifreyi Sıfırla</a></p><p>Şifre sıfırlama kodunuz: {resetToken}</p>"
                };

                await _emailService.SendEmailAsync(emailMessage);

                return Ok(new { message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre sıfırlama işlemi sırasında bir hata oluştu");
                return StatusCode(500, "Şifre sıfırlama işlemi sırasında bir hata oluştu");
            }
        }

        [HttpPost("reset-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.ResetToken == resetPasswordDto.Token);

                if (user == null || user.ResetTokenExpiry < DateTime.UtcNow)
                {
                    return BadRequest("Geçersiz veya süresi dolmuş şifre sıfırlama bağlantısı");
                }

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
                user.ResetToken = null;
                user.ResetTokenExpiry = null;
                user.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Ok(new { message = "Şifreniz başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre sıfırlama işlemi sırasında bir hata oluştu");
                return StatusCode(500, "Şifre sıfırlama işlemi sırasında bir hata oluştu");
            }
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
