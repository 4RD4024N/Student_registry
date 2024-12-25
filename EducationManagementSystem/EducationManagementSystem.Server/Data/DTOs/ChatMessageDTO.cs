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

public record CreateChatMessageDTO
{
    public required string Content { get; init; }
    public int SenderId { get; init; }
    public int ReceiverId { get; init; }
}