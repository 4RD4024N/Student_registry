using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Data.DTOs;
using System.Net;

namespace EducationManagementSystem.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService ?? throw new ArgumentNullException(nameof(studentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<StudentDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
    {
        try
        {
            var students = await _studentService.GetAllStudentsAsync();
            if (!students.Any())
            {
                return NotFound("Hiç öğrenci bulunamadı.");
            }
            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Öğrenciler listelenirken hata oluştu");
            return StatusCode(500, "Öğrenciler listelenirken bir hata oluştu.");
        }
    }

    [HttpGet("details/{id}")]
    [ProducesResponseType(typeof(StudentDetailDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<StudentDetailDTO>> GetStudentDetails(int id)
    {
        try
        {
            var studentDetails = await _studentService.GetStudentDetailsAsync(id);
            if (studentDetails == null)
            {
                return NotFound($"{id} ID'li öğrenci bulunamadı.");
            }
            return Ok(studentDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Öğrenci detayları getirilirken hata oluştu. ID: {StudentId}", id);
            return StatusCode(500, "Öğrenci detayları getirilirken bir hata oluştu.");
        }
    }

    [HttpGet("search/number/{studentNumber}")]
    [ProducesResponseType(typeof(StudentDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<StudentDTO>> SearchByNumber(string studentNumber)
    {
        try
        {
            var student = await _studentService.GetStudentByNumberAsync(studentNumber);
            if (student == null)
            {
                return NotFound($"{studentNumber} numaralı öğrenci bulunamadı.");
            }
            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Öğrenci aranırken hata oluştu. Numara: {StudentNumber}", studentNumber);
            return StatusCode(500, "Öğrenci aranırken bir hata oluştu.");
        }
    }

    [HttpGet("department/{departmentId}")]
    [ProducesResponseType(typeof(IEnumerable<StudentDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<IEnumerable<StudentDTO>>> GetDepartmentStudents(int departmentId)
    {
        try
        {
            var students = await _studentService.GetStudentsByDepartmentAsync(departmentId);
            if (!students.Any())
            {
                return NotFound($"Bu bölümde kayıtlı öğrenci bulunamadı.");
            }
            return Ok(students);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Bölüm öğrencileri listelenirken hata oluştu. Bölüm ID: {DepartmentId}", departmentId);
            return StatusCode(500, "Bölüm öğrencileri listelenirken bir hata oluştu.");
        }
    }
}