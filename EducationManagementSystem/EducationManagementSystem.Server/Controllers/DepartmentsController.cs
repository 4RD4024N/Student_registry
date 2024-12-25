using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationManagementSystem.Server.Models;
using EducationManagementSystem.Server.Data;
using System.Net;

namespace EducationManagementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(ApplicationDbContext context, ILogger<DepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Tüm bölümleri listeler
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Department>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            try
            {
                var departments = await _context.Departments
                    .Include(d => d.Students)
                    .Include(d => d.Courses)
                    .ToListAsync();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bölümler getirilirken hata oluştu");
                return StatusCode(500, "Bölümler getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Belirli bir bölümün detaylarını getirir
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Department), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            try
            {
                var department = await _context.Departments
                    .Include(d => d.Students)
                    .Include(d => d.Courses)
                    .FirstOrDefaultAsync(d => d.DepartmentId == id);

                if (department == null)
                {
                    return NotFound($"{id} ID'li bölüm bulunamadı");
                }

                return Ok(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {DepartmentId} olan bölüm getirilirken hata oluştu", id);
                return StatusCode(500, "Bölüm bilgileri getirilirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Bölümdeki öğrenci sayısını getirir
        /// </summary>
        [HttpGet("{id}/student-count")]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<int>> GetDepartmentStudentCount(int id)
        {
            try
            {
                var exists = await _context.Departments.AnyAsync(d => d.DepartmentId == id);
                if (!exists)
                {
                    return NotFound($"{id} ID'li bölüm bulunamadı");
                }

                var studentCount = await _context.Students
                    .Where(s => s.DepartmentId == id)
                    .CountAsync();

                return Ok(studentCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {DepartmentId} olan bölümün öğrenci sayısı getirilirken hata oluştu", id);
                return StatusCode(500, "Öğrenci sayısı hesaplanırken bir hata oluştu");
            }
        }

        /// <summary>
        /// Yeni bölüm ekler
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Department), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Department>> CreateDepartment(Department department)
        {
            try
            {
                if (await _context.Departments.AnyAsync(d => d.Code == department.Code))
                {
                    return BadRequest($"{department.Code} kodlu bölüm zaten mevcut");
                }

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetDepartment), new { id = department.DepartmentId }, department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yeni bölüm eklenirken hata oluştu");
                return StatusCode(500, "Bölüm eklenirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Bölüm bilgilerini günceller
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateDepartment(int id, Department department)
        {
            if (id != department.DepartmentId)
            {
                return BadRequest("ID'ler eşleşmiyor");
            }

            try
            {
                var existingDepartment = await _context.Departments.FindAsync(id);
                if (existingDepartment == null)
                {
                    return NotFound($"{id} ID'li bölüm bulunamadı");
                }

                // Code değişmişse ve yeni kod başka bir bölümde kullanılıyorsa
                if (existingDepartment.Code != department.Code &&
                    await _context.Departments.AnyAsync(d => d.Code == department.Code))
                {
                    return BadRequest($"{department.Code} kodlu başka bir bölüm zaten mevcut");
                }

                _context.Entry(existingDepartment).CurrentValues.SetValues(department);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {DepartmentId} olan bölüm güncellenirken hata oluştu", id);
                return StatusCode(500, "Bölüm güncellenirken bir hata oluştu");
            }
        }

        /// <summary>
        /// Bölümü siler
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                {
                    return NotFound($"{id} ID'li bölüm bulunamadı");
                }

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ID: {DepartmentId} olan bölüm silinirken hata oluştu", id);
                return StatusCode(500, "Bölüm silinirken bir hata oluştu");
            }
        }
    }
}