using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Interfaces;

public interface IStudentService
{
    Task<IEnumerable<StudentDTO>> GetAllStudentsAsync();
    Task<StudentDetailDTO?> GetStudentDetailsAsync(int id);
    Task<StudentDTO?> GetStudentByNumberAsync(string studentNumber);
    Task<IEnumerable<StudentDTO>> GetStudentsByDepartmentAsync(int departmentId);
}