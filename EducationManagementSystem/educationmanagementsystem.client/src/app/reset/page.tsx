import React, { useState } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const ResetPassword = () => {
  const [email, setEmail] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate(); // React Router yönlendirme kancası

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    try {
      await axios.post("/api/auth/forgot-password", { email });
      setSubmitted(true);
      setTimeout(() => {
        navigate("/reset-password"); // Yeni sayfaya yönlendirme
      }, 3000); // 3 saniye bekledikten sonra yönlendirme
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
            <p>
              If an account exists for {email}, you will receive an email with
              password reset instructions shortly.
            </p>
          </div>
        ) : (
          <>
            <h1 style={styles.title}>Reset Your Password</h1>
            <p style={styles.description}>
              Enter your email address to receive a password reset link.
            </p>
            <form style={styles.form} onSubmit={handleSubmit}>
              <input
                type="email"
                placeholder="Your Email Address"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                style={styles.input}
              />
              {error && <p style={styles.error}>{error}</p>}
              <button type="submit" style={styles.button}>
                Send Reset Link
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

export default ResetPassword;
