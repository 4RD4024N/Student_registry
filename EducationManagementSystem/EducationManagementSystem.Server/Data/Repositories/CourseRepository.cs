using EducationManagementSystem.Server.Data;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

public class CourseRepository : ICourseRepository
{
    private readonly ApplicationDbContext _context;

    public CourseRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .Include(c => c.CourseSchedules) // CourseSchedules olarak güncellendi
            .Include(c => c.StudentCourses)
            .FirstOrDefaultAsync(c => c.CourseId == id);
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Department)
            .Include(c => c.CourseSchedules) // CourseSchedules olarak güncellendi
            .ToListAsync();
    }

    public async Task<Course?> GetByCourseCodeAsync(string courseCode)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .FirstOrDefaultAsync(c => c.CourseCode == courseCode);
    }

    public async Task<IEnumerable<Course>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .Where(c => c.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetEnrolledStudentsAsync(int courseId)
    {
        var course = await _context.Courses
            .Include(c => c.StudentCourses)
            .ThenInclude(sc => sc.Student)
            .ThenInclude(s => s.Department)
            .FirstOrDefaultAsync(c => c.CourseId == courseId);

        return course?.StudentCourses.Select(sc => sc.Student) ?? new List<Student>();
    }

    public async Task<Course> AddAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        return course;
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Entry(course).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> CourseExistsAsync(string courseCode)
    {
        return await _context.Courses.AnyAsync(c => c.CourseCode == courseCode);
    }

    public async Task<bool> EnrollStudentAsync(int courseId, int studentId)
    {
        var studentCourse = new StudentCourse
        {
            CourseId = courseId,
            StudentId = studentId,
            RegistrationDate = DateTime.UtcNow
        };

        try
        {
            await _context.StudentCourses.AddAsync(studentCourse);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UnenrollStudentAsync(int courseId, int studentId)
    {
        var enrollment = await _context.StudentCourses
            .FirstOrDefaultAsync(sc => sc.CourseId == courseId && sc.StudentId == studentId);

        if (enrollment == null) return false;

        _context.StudentCourses.Remove(enrollment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CourseSchedule>> GetCourseScheduleAsync(int courseId)
    {
        return await _context.CourseSchedules
            .Where(cs => cs.CourseId == courseId)
            .ToListAsync();
    }
}
