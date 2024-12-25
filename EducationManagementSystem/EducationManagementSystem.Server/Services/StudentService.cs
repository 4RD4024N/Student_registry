using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<StudentDTO>> GetAllStudentsAsync()
    {
        var students = await _studentRepository.GetAllAsync();
        return students.Select(s => new StudentDTO
        {
            StudentId = s.StudentId,
            StudentNumber = s.StudentNumber,
            FullName = $"{s.FirstName} {s.LastName}",
            DepartmentName = s.Department.Name,
            Email = s.Email,
            PhoneNumber = s.PhoneNumber
        });
    }

    public async Task<StudentDetailDTO?> GetStudentDetailsAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student == null) return null;

        return new StudentDetailDTO
        {
            StudentId = student.StudentId,
            StudentNumber = student.StudentNumber,
            FullName = $"{student.FirstName} {student.LastName}",
            DepartmentName = student.Department.Name,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            DateOfBirth = student.DateOfBirth,
            EnrollmentDate = student.EnrollmentDate,
            EnrolledCourses = student.StudentCourses.Select(sc => new CourseDTO
            {
                CourseId = sc.Course.CourseId,
                CourseCode = sc.Course.CourseCode,
                Name = sc.Course.Name,
                Description = sc.Course.Description ?? string.Empty,
                Credits = sc.Course.Credits,
                DepartmentName = student.Department.Name
            }).ToList()
        };
    }

    public async Task<StudentDTO?> GetStudentByNumberAsync(string studentNumber)
    {
        var student = await _studentRepository.GetByStudentNumberAsync(studentNumber);
        if (student == null) return null;

        return new StudentDTO
        {
            StudentId = student.StudentId,
            StudentNumber = student.StudentNumber,
            FullName = $"{student.FirstName} {student.LastName}",
            DepartmentName = student.Department.Name,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber
        };
    }

    public async Task<IEnumerable<StudentDTO>> GetStudentsByDepartmentAsync(int departmentId)
    {
        var exists = await _studentRepository.DepartmentExistsAsync(departmentId);
        if (!exists) throw new KeyNotFoundException($"{departmentId} ID'li bölüm bulunamadı.");

        var students = await _studentRepository.GetByDepartmentIdAsync(departmentId);
        return students.Select(s => new StudentDTO
        {
            StudentId = s.StudentId,
            StudentNumber = s.StudentNumber,
            FullName = $"{s.FirstName} {s.LastName}",
            DepartmentName = s.Department.Name,
            Email = s.Email,
            PhoneNumber = s.PhoneNumber
        });
    }
}