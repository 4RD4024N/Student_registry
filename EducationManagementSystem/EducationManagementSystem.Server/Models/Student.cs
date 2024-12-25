namespace EducationManagementSystem.Server.Models;

public class Student
{
    public Student()
    {
        StudentNumber = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        StudentCourses = new List<StudentCourse>();
    }

    public int StudentId { get; set; }
    public required string StudentNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int DepartmentId { get; set; }
    public int UserId { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;

    // şifreli password
    public string PasswordHash { get; set; } = string.Empty;

    
    public virtual Department Department { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<StudentCourse> StudentCourses { get; set; }
}
