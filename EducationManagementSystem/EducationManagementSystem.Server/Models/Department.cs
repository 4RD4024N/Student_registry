using EducationManagementSystem.Server.Models;

public class Department
{
    public int DepartmentId { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}