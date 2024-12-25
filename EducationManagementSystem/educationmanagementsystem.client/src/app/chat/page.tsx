import React, { useEffect, useState } from "react";
import axios from "axios";
import { motion } from "framer-motion";

interface ChatMessage {
  messageId: number;
  content: string;
  senderName: string;
  receiverName: string;
  sentAt: string;
  isRead: boolean;
}

const ChatScreen = () => {
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [newMessage, setNewMessage] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const senderEmail = "arda03ozan@gmail.com"; // Placeholder sender email
  const receiverEmail = "arda05ozan@gmail.com"; // Placeholder receiver email

  useEffect(() => {
    fetchMessages();
  }, []);

  const fetchMessages = async () => {
    try {
      const response = await axios.get<ChatMessage[]>(
        `/api/chatmessages/conversation?senderEmail=${senderEmail}&receiverEmail=${receiverEmail}`
      );
      setMessages(response.data);
    } catch (err) {
      console.error("Error fetching messages:", err);
      setError("Mesajlar yüklenirken bir hata oluştu.");
    } finally {
      setLoading(false);
    }
  };

  const handleSendMessage = async () => {
    if (!newMessage.trim()) return;

    try {
      const response = await axios.post<ChatMessage>("/api/chatmessages", {
        content: newMessage,
        senderEmail,
        receiverEmail,
      });

      setMessages([...messages, response.data]);
      setNewMessage("");
    } catch (err) {
      console.error("Error sending message:", err);
      setError("Mesaj gönderilirken bir hata oluştu.");
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0 }}
      animate={{ opacity: 1 }}
      transition={{ duration: 0.5 }}
      style={styles.chatContainer}
    >
      <div style={styles.chatHeader}>Chat with {receiverEmail}</div>
      <div style={styles.messagesContainer}>
        {loading ? (
          <motion.div
            initial={{ scale: 0.8 }}
            animate={{ scale: 1 }}
            transition={{ duration: 0.4 }}
            style={styles.loadingText}
          >
            Loading messages...
          </motion.div>
        ) : error ? (
          <motion.div
            initial={{ scale: 0.8 }}
            animate={{ scale: 1 }}
            transition={{ duration: 0.4 }}
            style={styles.errorText}
          >
            {error}
          </motion.div>
        ) : (
          <ul style={styles.messageList}>
            {messages.map((message) => (
              <motion.li
                key={message.messageId}
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ duration: 0.3 }}
                style={
                  message.senderName === senderEmail
                    ? { ...styles.messageItem, ...styles.outgoingMessage }
                    : { ...styles.messageItem, ...styles.incomingMessage }
                }
              >
                <p style={styles.messageContent}>{message.content}</p>
                <small style={styles.messageTime}>
                  {new Date(message.sentAt).toLocaleTimeString()}
                </small>
              </motion.li>
            ))}
          </ul>
        )}
      </div>
      <div style={styles.inputContainer}>
        <input
          type="text"
          style={styles.messageInput}
          placeholder="Type a message..."
          value={newMessage}
          onChange={(e) => setNewMessage(e.target.value)}
        />
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.sendButton}
          onClick={handleSendMessage}
        >
          Send
        </motion.button>
      </div>
    </motion.div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  chatContainer: {
    display: "flex",
    flexDirection: "column",
    height: "100vh",
    width: "400px",
    margin: "0 auto",
    borderRadius: "12px",
    boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
    overflow: "hidden",
    backgroundColor: "#ffffff",
  },
  chatHeader: {
    padding: "1rem",
    backgroundColor: "#4f46e5",
    color: "#ffffff",
    fontSize: "1.5rem",
    fontWeight: "bold",
    textAlign: "center",
  },
  messagesContainer: {
    flex: 1,
    padding: "1rem",
    overflowY: "auto",
    backgroundColor: "#f9fafb",
  },
  messageList: {
    listStyle: "none",
    margin: 0,
    padding: 0,
  },
  messageItem: {
    marginBottom: "1rem",
    padding: "0.5rem",
    borderRadius: "8px",
    maxWidth: "70%",
  },
  outgoingMessage: {
    backgroundColor: "#4f46e5",
    color: "#ffffff",
    marginLeft: "auto",
    textAlign: "right",
  },
  incomingMessage: {
    backgroundColor: "#e5e7eb",
    color: "#111827",
  },
  messageContent: {
    margin: 0,
    fontSize: "1rem",
  },
  messageTime: {
    marginTop: "0.5rem",
    fontSize: "0.75rem",
    color: "#9ca3af",
  },
  inputContainer: {
    display: "flex",
    borderTop: "1px solid #e5e7eb",
    padding: "0.5rem",
    backgroundColor: "#ffffff",
  },
  messageInput: {
    flex: 1,
    padding: "0.5rem",
    border: "1px solid #d1d5db",
    borderRadius: "8px",
    marginRight: "0.5rem",
    outline: "none",
  },
  sendButton: {
    backgroundColor: "#4f46e5",
    color: "#ffffff",
    border: "none",
    padding: "0.5rem 1rem",
    borderRadius: "8px",
    cursor: "pointer",
    transition: "background-color 0.3s ease",
  },
  loadingText: {
    textAlign: "center",
    color: "#6b7280",
  },
  errorText: {
    textAlign: "center",
    color: "#b91c1c",
  },
};

export default ChatScreen;
