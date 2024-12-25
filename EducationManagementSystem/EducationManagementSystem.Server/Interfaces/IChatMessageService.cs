using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Interfaces;

public interface IChatMessageService
{
    Task<IEnumerable<ChatMessageDTO>> GetAllMessagesAsync();
    Task<ChatMessageDTO?> GetMessageByIdAsync(int id);
    Task<IEnumerable<ChatMessageDTO>> GetConversationAsync(int user1Id, int user2Id);
    Task<ChatMessageDTO> SendMessageAsync(CreateChatMessageDTO messageDto);
    Task DeleteMessageAsync(int id);
    Task MarkMessageAsReadAsync(int messageId);
    Task<IEnumerable<ChatMessageDTO>> GetUnreadMessagesAsync(int userId);
}