using EducationManagementSystem.Server.Data.Seeds;
using EducationManagementSystem.Server.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        try
        {
            logger.LogInformation("Starting database seeding...");

            // Seed Departments
            await DepartmentSeedData.SeedAsync(context, logger);

            // Seed Courses
            await CourseSeedData.SeedAsync(context, logger);

            // Seed Students
            await StudentSeedData.SeedAsync(context, logger);

            // Seed Announcements
            await AnnouncementSeedData.SeedAsync(context, logger);

            logger.LogInformation("Database seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
