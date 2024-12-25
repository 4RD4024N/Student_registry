using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Server.Data.Seeds
{
    public static class DepartmentSeedData
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if (await context.Departments.AnyAsync())
                return;

            logger.LogInformation("Bölümler ekleniyor...");

            var departments = new List<Department>
            {
                new Department { Name = "Bilgisayar Mühendisliği", Code = "CSE" },
                new Department { Name = "Elektrik-Elektronik Mühendisliği", Code = "EEE" },
                new Department { Name = "Makine Mühendisliği", Code = "ME" },
                new Department { Name = "Endüstri Mühendisliği", Code = "IE" },
                new Department { Name = "İnşaat Mühendisliği", Code = "CE" },
                new Department { Name = "Kimya Mühendisliği", Code = "CHE" },
                new Department { Name = "Matematik", Code = "MATH" },
                new Department { Name = "Fizik", Code = "PHY" },
                new Department { Name = "Kimya", Code = "CHEM" },
                new Department { Name = "Biyoloji", Code = "BIO" },
                new Department { Name = "İşletme", Code = "BUS" },
                new Department { Name = "Ekonomi", Code = "ECON" },
                new Department { Name = "Psikoloji", Code = "PSY" },
                new Department { Name = "Sosyoloji", Code = "SOC" },
                new Department { Name = "Hukuk", Code = "LAW" }
            };

            await context.Departments.AddRangeAsync(departments);
            await context.SaveChangesAsync();
            logger.LogInformation("Bölümler başarıyla eklendi.");
        }
    }
}