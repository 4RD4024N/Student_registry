public class Announcement
{
    public int AnnouncementId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // İlişkili Departman
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    
   
}
