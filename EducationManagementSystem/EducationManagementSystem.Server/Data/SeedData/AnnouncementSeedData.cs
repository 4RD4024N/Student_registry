using EducationManagementSystem.Server.Data;
using EducationManagementSystem.Server.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationManagementSystem.Server.Data.Seeds
{
    public static class AnnouncementSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if (context.Announcements.Any())
            {
                logger.LogInformation("Announcements already exist in the database. Skipping seeding.");
                return;
            }

            var departments = context.Departments.ToList();
            if (!departments.Any())
            {
                logger.LogWarning("No departments found in the database. Skipping announcement seeding.");
                return;
            }

            var announcements = new List<Announcement>();
            var random = new Random();

            foreach (var department in departments)
            {
                for (int i = 1; i <= 30; i++)
                {
                    announcements.Add(new Announcement
                    {
                        Title = $"Duyuru {i} - {department.Name}",
                        Content = $"Bu, {department.Name} için {i}. duyurudur. Daha fazla detay için lütfen iletişime geçiniz.",
                        DepartmentId = department.DepartmentId, // Departman kimliği ekleniyor
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365)), // Rastgele oluşturulma tarihi
                        UpdatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)), // Rastgele güncelleme tarihi
                        
                    });
                }
            }


            context.Announcements.AddRange(announcements);
            await context.SaveChangesAsync();
            logger.LogInformation("Announcements seeded successfully.");
        }
    }
}
