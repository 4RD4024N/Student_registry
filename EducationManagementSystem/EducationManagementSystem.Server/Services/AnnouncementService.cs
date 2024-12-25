using EducationManagementSystem.Server.Data;
using Microsoft.EntityFrameworkCore;

public class AnnouncementService
{
    private readonly ApplicationDbContext _context;

    public AnnouncementService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Announcement>> GetAnnouncementsAsync()
    {
        return await _context.Announcements
            .OrderByDescending(a => a.CreatedAt) // Sadece sıralama yapıyoruz
            .ToListAsync();
    }
}
