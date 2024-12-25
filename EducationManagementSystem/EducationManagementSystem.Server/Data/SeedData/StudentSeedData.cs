using EducationManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationManagementSystem.Server.Data.Seeds
{
    public static class StudentSeedData
    {
        private static readonly List<string> FirstNames = new()
        {
            "Ahmet", "Mehmet", "Ali", "Ayşe", "Fatma", "Zeynep", "Can", "Ece",
            "Deniz", "Elif", "Burak", "Canan", "Derya", "Emre", "Fulya", "Gökhan",
            "Hande", "İrem", "Kemal", "Leyla", "Murat", "Nilüfer", "Orhan", "Pelin",
            "Rüya", "Selim", "Tülay", "Ufuk", "Veli", "Yasemin", "Arda", "Beyza",
            "Cem", "Dilan", "Eren", "Ferhat", "Gizem", "Halil", "Işık", "Kerem",
            "Melis", "Naz", "Onur", "Pınar", "Sinem", "Tolga", "Umut", "Volkan",
            "Yağmur", "Zehra"
        };

        private static readonly List<string> LastNames = new()
        {
            "Yılmaz", "Kaya", "Demir", "Çelik", "Şahin", "Yıldız", "Öztürk",
            "Aydın", "Özdemir", "Arslan", "Doğan", "Kılıç", "Aslan", "Çetin",
            "Şen", "Erdoğan", "Tekin", "Güneş", "Kurt", "Özkan", "Şimşek",
            "Korkmaz", "Kaplan", "Acar", "Tunç", "Alkan", "Bulut", "Koç", "Altın",
            "Bayraktar", "Can", "Durmaz", "Eren", "Fidan", "Gül", "Hacıoğlu", "Işık",
            "Kaya", "Limon", "Mavi", "Nar", "Ocak", "Pak", "Sönmez", "Turan", "Özen"
        };

        public static async Task SeedAsync(ApplicationDbContext context, ILogger logger)
        {
            if (await context.Students.AnyAsync())
                return;

            logger.LogInformation("\u00d6\u011frenciler ekleniyor...");

            var random = new Random();
            var usedEmails = new HashSet<string>();
            var usedNumbers = new HashSet<string>();
            var departments = await context.Departments.ToListAsync();

            var users = new List<User>();
            var students = new List<Student>();

            for (int i = 0; i < 200; i++)
            {
                string firstName, lastName, email, studentNumber;
                do
                {
                    firstName = FirstNames[random.Next(FirstNames.Count)];
                    lastName = LastNames[random.Next(LastNames.Count)];
                    email = $"{firstName.ToLower()}.{lastName.ToLower()}{random.Next(1000)}@university.edu.tr";
                    studentNumber = random.Next(10000000, 99999999).ToString();
                } while (!usedEmails.Add(email) || !usedNumbers.Add(studentNumber));

                var department = departments[random.Next(departments.Count)];

                // User kaydı oluştur
                var user = new User
                {
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123"),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                users.Add(user);

                // Student kaydı oluştur
                var student = new Student
                {
                    StudentNumber = studentNumber,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    DepartmentId = department.DepartmentId,
                    PhoneNumber = $"05{random.Next(10, 99)}{random.Next(1000000, 9999999)}",
                    DateOfBirth = DateTime.UtcNow.AddYears(-random.Next(18, 25)).AddDays(-random.Next(365)),
                    EnrollmentDate = DateTime.UtcNow.AddYears(-random.Next(1, 4)),
                    User = user
                };
                students.Add(student);

                // Batch kaydetme
                if ((i + 1) % 50 == 0)
                {
                    await context.Users.AddRangeAsync(users);
                    await context.Students.AddRangeAsync(students);
                    await context.SaveChangesAsync();

                    users.Clear();
                    students.Clear();
                    logger.LogInformation($"{i + 1} \u00f6\u011frenci eklendi.");
                }
            }

            // Kalan kayıtları kaydet
            if (users.Count > 0 || students.Count > 0)
            {
                await context.Users.AddRangeAsync(users);
                await context.Students.AddRangeAsync(students);
                await context.SaveChangesAsync();
                logger.LogInformation("Tüm \u00f6ğrenciler başarıyla eklendi.");
            }
        }
    }
}
