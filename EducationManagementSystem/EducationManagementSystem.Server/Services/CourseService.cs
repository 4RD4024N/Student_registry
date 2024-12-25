using EducationManagementSystem.Server.Data.DTOs;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger)
        {
            _courseRepository = courseRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<CourseDTO>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return courses.Select(MapToDTO);
        }

        public async Task<CourseDetailDTO?> GetCourseDetailsAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null) return null;
            return MapToDetailDTO(course);
        }

        public async Task<IEnumerable<CourseDTO>> GetCoursesByDepartmentAsync(int departmentId)
        {
            var courses = await _courseRepository.GetByDepartmentIdAsync(departmentId);
            return courses.Select(MapToDTO);
        }

        public async Task<IEnumerable<StudentDTO>> GetEnrolledStudentsAsync(int courseId)
        {
            var students = await _courseRepository.GetEnrolledStudentsAsync(courseId);
            return students.Select(MapToStudentDTO);
        }

        public async Task<CourseDTO> AddCourseAsync(CreateCourseDTO courseDto)
        {
            var exists = await _courseRepository.CourseExistsAsync(courseDto.CourseCode);
            if (exists)
            {
                throw new InvalidOperationException($"Course code {courseDto.CourseCode} already exists.");
            }

            var course = new Course
            {
                CourseCode = courseDto.CourseCode,
                Name = courseDto.Name,
                Description = courseDto.Description,
                Credits = courseDto.Credits,
                DepartmentId = courseDto.DepartmentId
            };

            var addedCourse = await _courseRepository.AddAsync(course);
            return MapToDTO(addedCourse);
        }

        public async Task UpdateCourseAsync(int id, UpdateCourseDTO courseDto)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found.");
            }

            if (course.CourseCode != courseDto.CourseCode)
            {
                var exists = await _courseRepository.CourseExistsAsync(courseDto.CourseCode);
                if (exists)
                {
                    throw new InvalidOperationException($"Course code {courseDto.CourseCode} already exists.");
                }
            }

            course.CourseCode = courseDto.CourseCode;
            course.Name = courseDto.Name;
            course.Description = courseDto.Description;
            course.Credits = courseDto.Credits;
            course.DepartmentId = courseDto.DepartmentId;

            await _courseRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found.");
            }

            await _courseRepository.DeleteAsync(id);
        }

        public async Task EnrollStudentAsync(int courseId, int studentId)
        {
            var success = await _courseRepository.EnrollStudentAsync(courseId, studentId);
            if (!success)
            {
                throw new InvalidOperationException("Failed to enroll student in course.");
            }
        }

        public async Task UnenrollStudentAsync(int courseId, int studentId)
        {
            var success = await _courseRepository.UnenrollStudentAsync(courseId, studentId);
            if (!success)
            {
                throw new InvalidOperationException("Failed to unenroll student from course.");
            }
        }

        public async Task<IEnumerable<CourseScheduleDTO>> GetCourseScheduleAsync(int courseId)
        {
            var schedules = await _courseRepository.GetCourseScheduleAsync(courseId);
            return schedules.Select(MapToScheduleDTO);
        }

        private static CourseDTO MapToDTO(Course course) =>
            new()
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                Name = course.Name,
                Description = course.Description ?? string.Empty,
                Credits = course.Credits,
                DepartmentName = course.Department.Name
            };

        private static CourseDetailDTO MapToDetailDTO(Course course) =>
            new()
            {
                CourseId = course.CourseId,
                CourseCode = course.CourseCode,
                Name = course.Name,
                Description = course.Description ?? string.Empty,
                Credits = course.Credits,
                DepartmentName = course.Department.Name,
                EnrolledStudentsCount = course.StudentCourses.Count,
                EnrolledStudents = course.StudentCourses.Select(sc => MapToStudentDTO(sc.Student)).ToList(),
                Schedule = course.CourseSchedules.Select(MapToScheduleDTO).ToList()
            };

        private static StudentDTO MapToStudentDTO(Student student) =>
            new()
            {
                StudentId = student.StudentId,
                StudentNumber = student.StudentNumber,
                FullName = $"{student.FirstName} {student.LastName}",
                DepartmentName = student.Department.Name,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber
            };

        private static CourseScheduleDTO MapToScheduleDTO(CourseSchedule schedule) =>
            new()
            {
                ScheduleId = schedule.ScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                
               
                CourseCode = schedule.Course.CourseCode,
                CourseName = schedule.Course.Name
            };
    }
}
