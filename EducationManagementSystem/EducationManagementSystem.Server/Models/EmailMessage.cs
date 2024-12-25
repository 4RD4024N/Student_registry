namespace EducationManagementSystem.Server.Models
{
    public class EmailMessage
    {
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public DateTime SentDate { get; set; } = DateTime.UtcNow;
        public bool IsHtml { get; set; } = true;
    }
}