using System;

namespace EducationManagementSystem.Server.Models
{
    public class ChatMessage
    {
        public int MessageId { get; set; } // Primary Key
        public string Content { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}
