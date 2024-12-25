using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseDTO>> GetAllCoursesAsync();
        Task<CourseDetailDTO?> GetCourseDetailsAsync(int id);
        Task<IEnumerable<CourseDTO>> GetCoursesByDepartmentAsync(int departmentId);
        Task<IEnumerable<StudentDTO>> GetEnrolledStudentsAsync(int courseId);
        Task<CourseDTO> AddCourseAsync(CreateCourseDTO courseDto);
        Task UpdateCourseAsync(int id, UpdateCourseDTO courseDto);
        Task DeleteCourseAsync(int id);
        Task EnrollStudentAsync(int courseId, int studentId);
        Task UnenrollStudentAsync(int courseId, int studentId);
        Task<IEnumerable<CourseScheduleDTO>> GetCourseScheduleAsync(int courseId);
    }
}