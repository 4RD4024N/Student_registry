namespace EducationManagementSystem.Server.Data.DTOs;



public class StudentDTO
{
    public int StudentId { get; set; }
    public required string StudentNumber { get; set; } = string.Empty;
    public required string FullName { get; set; } = string.Empty;
    public required string DepartmentName { get; set; } = string.Empty;
    public required string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }


    public string? Password { get; set; }
}

public class StudentDetailDTO : StudentDTO
{
    public DateTime? DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public List<CourseDTO> EnrolledCourses { get; set; } = new();
}
