import React from "react";
import { motion } from "framer-motion";

const NavigationPage = () => {
  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Navigation</h1>
      <div style={styles.buttonGrid}>
        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/";
          }}
        >
          Login
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/register";
          }}
        >
          Register
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/studentinfo";
          }}
        >
          Students
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/announcement";
          }}
        >
          Announcements
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/courses";
          }}
        >
          Courses
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/chat";
          }}
        >
          Chat
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/courseschedule";
          }}
        >
          Course Schedule
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/reset";
          }}
        >
          Reset Password
        </motion.button>

        <motion.button
          whileHover={{ scale: 1.05 }}
          whileTap={{ scale: 0.95 }}
          style={styles.button}
          onClick={() => {
            window.location.href = "/reset-password";
          }}
        >
          Set New Password
        </motion.button>
      </div>
    </div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    justifyContent: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #4b4bf5, #9333ea)",
    color: "#fff",
  },
  title: {
    fontSize: "2.5rem",
    fontWeight: "bold",
    marginBottom: "2rem",
  },
  buttonGrid: {
    display: "grid",
    gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
    gap: "1.5rem",
    width: "80%",
    maxWidth: "1000px",
  },
  button: {
    padding: "1rem 2rem",
    borderRadius: "8px",
    textDecoration: "none",
    fontSize: "1.2rem",
    fontWeight: "bold",
    color: "#fff",
    background: "#1e3a8a",
    textAlign: "center",
    transition: "background-color 0.3s ease",
  },
};

export default NavigationPage;
