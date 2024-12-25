using EducationManagementSystem.Server.Data;
using EducationManagementSystem.Server.Data.DTOs;
using EducationManagementSystem.Server.Interfaces;
using EducationManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationManagementSystem.Server.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ChatMessageService> _logger;

        public ChatMessageService(ApplicationDbContext context, ILogger<ChatMessageService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetAllMessagesAsync()
        {
            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return messages.Select(MapToDTO);
        }

        public async Task<ChatMessageDTO?> GetMessageByIdAsync(int id)
        {
            var message = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .FirstOrDefaultAsync(m => m.MessageId == id);

            return message == null ? null : MapToDTO(message);
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetConversationAsync(int user1Id, int user2Id)
        {
            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                            (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            return messages.Select(MapToDTO);
        }

        public async Task<ChatMessageDTO> SendMessageAsync(CreateChatMessageDTO messageDto)
        {
            var sender = await _context.Users.FirstOrDefaultAsync(u => u.Email == messageDto.SenderEmail)
                ?? throw new InvalidOperationException("Sender not found");
            var receiver = await _context.Users.FirstOrDefaultAsync(u => u.Email == messageDto.ReceiverEmail)
                ?? throw new InvalidOperationException("Receiver not found");

            var message = new ChatMessage
            {
                Content = messageDto.Content,
                SenderId = sender.UserId,
                ReceiverId = receiver.UserId,
                SentAt = DateTime.UtcNow,
                Sender = sender,
                Receiver = receiver
            };

            _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();

            return MapToDTO(message);
        }

        public async Task DeleteMessageAsync(int id)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message != null)
            {
                _context.ChatMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkMessageAsReadAsync(int messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null)
            {
                message.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChatMessageDTO>> GetUnreadMessagesAsync(int userId)
        {
            var messages = await _context.ChatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.ReceiverId == userId && !m.ReadAt.HasValue)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();

            return messages.Select(MapToDTO);
        }

        private ChatMessageDTO MapToDTO(ChatMessage message)
        {
            return new ChatMessageDTO
            {
                MessageId = message.MessageId,
                Content = message.Content,
                SenderName = message.Sender?.Email ?? "Unknown",
                ReceiverName = message.Receiver?.Email ?? "Unknown",
                SentAt = message.SentAt,
                IsRead = message.ReadAt.HasValue
            };
        }
    }
}
