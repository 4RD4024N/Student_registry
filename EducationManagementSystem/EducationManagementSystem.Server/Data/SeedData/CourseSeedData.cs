using EducationManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Server.Data.Seeds
{
    public static class CourseSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if (await context.Courses.AnyAsync())
                return;

            logger.LogInformation("Dersler ekleniyor...");

            var departments = await context.Departments.ToListAsync();
            var courses = new List<Course>();

            foreach (var department in departments)
            {
                courses.AddRange(new List<Course>
                {
                    new Course
                    {
                        CourseCode = $"{department.Code}101",
                        Name = $"{department.Name} Giriş",
                        Description = $"{department.Name} giriş dersi.",
                        Credits = 3,
                        DepartmentId = department.DepartmentId
                    },
                    new Course
                    {
                        CourseCode = $"{department.Code}102",
                        Name = $"{department.Name} Temel Kavramlar",
                        Description = $"{department.Name} temel kavramları içeren ders.",
                        Credits = 4,
                        DepartmentId = department.DepartmentId
                    },
                    new Course
                    {
                        CourseCode = $"{department.Code}201",
                        Name = $"{department.Name} İleri Konular",
                        Description = $"{department.Name} ileri düzey ders.",
                        Credits = 4,
                        DepartmentId = department.DepartmentId
                    },
                    new Course
                    {
                        CourseCode = $"{department.Code}301",
                        Name = $"{department.Name} Projeler",
                        Description = $"{department.Name} projelerine yönelik ders.",
                        Credits = 5,
                        DepartmentId = department.DepartmentId
                    }
                });
            }

            await context.Courses.AddRangeAsync(courses);
            await context.SaveChangesAsync();
            logger.LogInformation("Dersler başarıyla eklendi.");
        }
    }
}