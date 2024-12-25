namespace EducationManagementSystem.Server.Data.DTOs
{
    public class CourseDTO
    {
        public int CourseId { get; init; }
        public required string CourseCode { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public int Credits { get; init; }
        public required string DepartmentName { get; init; }
    }

    public class CourseDetailDTO : CourseDTO
    {
        public int EnrolledStudentsCount { get; init; }
        public required ICollection<StudentDTO> EnrolledStudents { get; init; } = new List<StudentDTO>();
        public required ICollection<CourseScheduleDTO> Schedule { get; init; } = new List<CourseScheduleDTO>();
    }

    public class CourseScheduleDTO
    {
        public int ScheduleId { get; init; }
        public string? CourseCode { get; set; } // Nullable yapılabilir
        public required string CourseName { get; init; }
        public required DayOfWeek DayOfWeek { get; init; }
        public required TimeSpan StartTime { get; init; }
        public required TimeSpan EndTime { get; init; }
        
       
    }

    public class CreateCourseDTO
    {
        public required string CourseCode { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public int Credits { get; init; }
        public int DepartmentId { get; init; }
        public required ICollection<CourseScheduleDTO> Schedule { get; init; } = new List<CourseScheduleDTO>();
    }

    public class UpdateCourseDTO
    {
        public required string CourseCode { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
        public int Credits { get; init; }
        public int DepartmentId { get; init; }
        public required ICollection<CourseScheduleDTO> Schedule { get; init; } = new List<CourseScheduleDTO>();
    }

    public class CourseResponseDTO
    {
        public required string Message { get; init; }
        public bool Success { get; init; }
        public CourseDTO? Course { get; init; }
        public string? ErrorMessage { get; init; }
    }

    public class CourseEnrollmentDTO
    {
        public int StudentId { get; init; }
        public int CourseId { get; init; }
        public DateTime RegistrationDate { get; init; } = DateTime.UtcNow;
    }

    public class CourseEnrollmentResponseDTO
    {
        public required string Message { get; init; }
        public bool Success { get; init; }
        public string? ErrorMessage { get; init; }
        public required StudentDTO Student { get; init; }
        public required CourseDTO Course { get; init; }
    }
}