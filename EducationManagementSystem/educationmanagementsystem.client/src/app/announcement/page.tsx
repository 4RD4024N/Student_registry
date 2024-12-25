import React, { useEffect, useState } from "react";
import { motion } from "framer-motion";

interface Announcement {
  id: number;
  title: string;
  content: string;
  createdAt: string;
}

export function Announcement() {
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchAnnouncements();
  }, []);

  const fetchAnnouncements = async () => {
    try {
      const response = await fetch("/api/announcements");
      const data = await response.json();
      setAnnouncements(data);
    } catch (error) {
      console.error("Error fetching announcements:", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 50 }}
      animate={{ opacity: 1, y: 0 }}
      exit={{ opacity: 0, y: 50 }}
      style={announcementStyles.container}
    >
      <div style={announcementStyles.card}>
        <motion.h1
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
          style={announcementStyles.title}
        >
          Announcements
        </motion.h1>
        {loading ? (
          <p style={announcementStyles.loadingText}>Loading announcements...</p>
        ) : announcements.length === 0 ? (
          <p style={announcementStyles.noDataText}>
            No announcements available at the moment.
          </p>
        ) : (
          <div style={announcementStyles.scrollableList}>
            <ul style={announcementStyles.list}>
              {announcements.map((announcement) => (
                <li key={announcement.id} style={announcementStyles.listItem}>
                  <strong style={announcementStyles.itemTitle}>
                    {announcement.title}
                  </strong>
                  <p style={announcementStyles.itemContent}>
                    {announcement.content}
                  </p>
                  <small style={announcementStyles.itemDate}>
                    {new Date(announcement.createdAt).toLocaleString()}
                  </small>
                </li>
              ))}
            </ul>
          </div>
        )}
      </div>
    </motion.div>
  );
}

const announcementStyles: { [key: string]: React.CSSProperties } = {
  container: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #4f46e5, #9333ea)",
    padding: "2rem",
  },
  card: {
    width: "90%",
    maxWidth: "500px",
    backgroundColor: "#ffffff",
    borderRadius: "12px",
    boxShadow: "0 8px 24px rgba(0,0,0,0.15)",
    padding: "2rem",
    textAlign: "center",
  },
  title: {
    fontSize: "2rem",
    fontWeight: "bold",
    color: "#4f46e5",
    marginBottom: "1rem",
  },
  loadingText: {
    fontSize: "1rem",
    color: "#6b7280",
  },
  noDataText: {
    fontSize: "1rem",
    color: "#9ca3af",
  },
  scrollableList: {
    maxHeight: "300px", // Maksimum yükseklik
    overflowY: "auto", // Dikey kaydırma
    paddingRight: "1rem",
  },
  list: {
    textAlign: "left",
    marginTop: "1rem",
    padding: "0",
    listStyleType: "none",
  },
  listItem: {
    fontSize: "1rem",
    color: "#4b5563",
    marginBottom: "1.5rem",
    padding: "1rem",
    borderRadius: "8px",
    border: "1px solid #e5e7eb",
    backgroundColor: "#f9fafb",
    boxShadow: "0 2px 4px rgba(0,0,0,0.1)",
  },
  itemTitle: {
    fontSize: "1.2rem",
    fontWeight: "bold",
    color: "#4f46e5",
    marginBottom: "0.5rem",
  },
  itemContent: {
    fontSize: "1rem",
    color: "#6b7280",
    marginBottom: "0.5rem",
  },
  itemDate: {
    fontSize: "0.8rem",
    color: "#9ca3af",
  },
};
