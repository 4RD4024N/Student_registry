namespace EducationManagementSystem.Server.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public required string CourseCode { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Credits { get; set; }

        // İlişkiler
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public ICollection<CourseSchedule> CourseSchedules { get; set; } = new List<CourseSchedule>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();
    }
}
