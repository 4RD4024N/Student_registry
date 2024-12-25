using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Interfaces;

public interface IStudentRepository
{
    Task<IEnumerable<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(int id);
    Task<Student?> GetByStudentNumberAsync(string studentNumber);
    Task<IEnumerable<Student>> GetByDepartmentIdAsync(int departmentId);
    Task<bool> DepartmentExistsAsync(int departmentId);
    Task<Student> AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
}