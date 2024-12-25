import React, { useState } from "react";
import axios from "axios";

const ResetPasswordPage = () => {
  const [token, setToken] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (newPassword !== confirmPassword) {
      setError("Passwords do not match.");
      return;
    }

    try {
      await axios.post("/api/auth/reset-password", { token, newPassword });
      setSubmitted(true);
    } catch (err: any) {
      if (err.response?.data?.message) {
        setError(err.response.data.message);
      } else {
        setError("An error occurred. Please try again later.");
      }
    }
  };

  return (
    <div style={styles.mainContainer}>
      <div style={styles.card}>
        {submitted ? (
          <div style={styles.successMessage}>
            <h1>🎉 Success!</h1>
            <p>Your password has been successfully reset. You can now log in with your new password.</p>
          </div>
        ) : (
          <>
            <h1 style={styles.title}>Reset Your Password</h1>
            <p style={styles.description}>
              Enter the reset token sent to your email and set a new password.
            </p>
            <form style={styles.form} onSubmit={handleSubmit}>
              <input
                type="text"
                placeholder="Reset Token"
                value={token}
                onChange={(e) => setToken(e.target.value)}
                required
                style={styles.input}
              />
              <input
                type="password"
                placeholder="New Password"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                required
                style={styles.input}
              />
              <input
                type="password"
                placeholder="Confirm Password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
                style={styles.input}
              />
              {error && <p style={styles.error}>{error}</p>}
              <button type="submit" style={styles.button}>
                Reset Password
              </button>
            </form>
          </>
        )}
      </div>
    </div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  mainContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #ff7eb3, #4b4bf5)",
    color: "#ffffff",
  },
  card: {
    backgroundColor: "#ffffff",
    borderRadius: "12px",
    boxShadow: "0 12px 24px rgba(0,0,0,0.2)",
    padding: "2rem",
    maxWidth: "400px",
    textAlign: "center",
    color: "#2c3e50",
  },
  title: {
    fontSize: "1.5rem",
    fontWeight: "bold",
    marginBottom: "1rem",
  },
  description: {
    fontSize: "1rem",
    marginBottom: "1.5rem",
    color: "#555",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1rem",
  },
  input: {
    padding: "0.75rem",
    borderRadius: "8px",
    border: "1px solid #ccc",
    fontSize: "1rem",
  },
  button: {
    padding: "0.75rem",
    borderRadius: "8px",
    backgroundColor: "#1a73e8",
    color: "#fff",
    border: "none",
    fontSize: "1rem",
    cursor: "pointer",
    transition: "background-color 0.3s",
  },
  successMessage: {
    textAlign: "center",
  },
  error: {
    color: "#d9534f",
    fontSize: "0.9rem",
    marginTop: "-0.5rem",
  },
};

export default ResetPasswordPage;
