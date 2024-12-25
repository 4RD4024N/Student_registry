using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Interfaces
{
    public interface IChatMessageRepository
    {
        Task<IEnumerable<ChatMessage>> GetAllAsync();
        Task<ChatMessage?> GetByIdAsync(int id);
        Task<IEnumerable<ChatMessage>> GetMessagesBetweenUsersAsync(int senderId, int receiverId);
        Task<ChatMessage> AddAsync(ChatMessage message);
        Task UpdateAsync(ChatMessage message);
        Task DeleteAsync(int id);
        Task MarkAsReadAsync(int messageId);
        Task<IEnumerable<ChatMessage>> GetUnreadMessagesAsync(int userId);
    }
}