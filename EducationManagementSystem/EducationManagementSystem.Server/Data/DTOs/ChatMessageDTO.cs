namespace EducationManagementSystem.Server.Data.DTOs;

public record ChatMessageDTO
{
    public int MessageId { get; init; }
    public required string Content { get; init; }
    public required string SenderName { get; init; }
    public required string ReceiverName { get; init; }
    public DateTime SentAt { get; init; }
    public bool IsRead { get; init; }
}

public class CreateChatMessageDTO
{
    public required string Content { get; set; }
    public required string SenderEmail { get; set; } // Gönderici e-posta adresi
    public required string ReceiverEmail { get; set; } // Alıcı e-posta adresi
}
