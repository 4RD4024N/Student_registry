using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Interfaces
{

    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetByDepartmentIdAsync(int departmentId);
        Task<Course?> GetByCourseCodeAsync(string courseCode);
        Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int courseId);
        Task<Course> AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
        Task<bool> CourseExistsAsync(string courseCode);
        Task<bool> EnrollStudentAsync(int courseId, int studentId);
        Task<bool> UnenrollStudentAsync(int courseId, int studentId);
        Task<IEnumerable<CourseSchedule>> GetCourseScheduleAsync(int courseId);
    }
}