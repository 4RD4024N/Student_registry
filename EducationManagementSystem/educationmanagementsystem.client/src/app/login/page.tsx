import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import axios from "axios";
import { motion } from "framer-motion";

// Yup Validasyon Şeması
const schema = yup.object().shape({
  email: yup.string().email("Invalid email").required("Email is required"),
  password: yup.string().required("Password is required"),
});

export default function Login() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [showPopup, setShowPopup] = useState(false); // Pop-up durumu

  const onSubmit = async (data: any) => {
    setLoading(true);
    setError(null);

    try {
      const response = await axios.post("api/auth/login", data);

      if (response.status === 200) {
        const { token, email, role } = response.data;
        localStorage.setItem("token", token);
        setShowPopup(true); // Pop-up göster
      } else {
        setError("Login failed. Please try again.");
      }
    } catch (err: any) {
      setError(err.response?.data?.message || "An error occurred.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={styles.mainContainer}>
      <div style={styles.card}>
        <motion.h1
          initial={{ opacity: 0, y: -20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.6 }}
          style={styles.title}
        >
          Login
        </motion.h1>
        
        <motion.div
          initial={{ scale: 0.9, opacity: 0.8 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ duration: 0.5 }}
          style={styles.formContainer}
        >
          {error && <div style={styles.errorBox}>{error}</div>}
          <form onSubmit={handleSubmit(onSubmit)} style={styles.form}>
            <div style={styles.inputGroup}>
              <label htmlFor="email" style={styles.label}>
                Email
              </label>
              <input
                id="email"
                type="email"
                {...register("email")}
                style={styles.input}
              />
              <p style={styles.error}>{errors.email?.message}</p>
            </div>
            <div style={styles.inputGroup}>
              <label htmlFor="password" style={styles.label}>
                Password
              </label>
              <input
                id="password"
                type="password"
                {...register("password")}
                style={styles.input}
              />
              <p style={styles.error}>{errors.password?.message}</p>
            </div>
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              type="submit"
              style={styles.submitButton}
              disabled={loading}
            >
              {loading ? "Logging in..." : "Login"}
            </motion.button>
            <motion.button
            whileHover={{ scale: 1.05 }}
            whileTap={{ scale: 0.95 }}
            style={styles.submitButton}
            onClick={() => {
              window.location.href = "/register";
            }}
          >
            Register
          </motion.button>

          </form>
        </motion.div>
      </div>

      {/* Pop-up (modal) */}
      {showPopup && (
        <div style={styles.popupOverlay}>
          <div style={styles.popup}>
            <h2 style={styles.popupTitle}>Login Successful</h2>
            <p style={styles.popupMessage}>Welcome back!</p>
            <button
              style={styles.popupButton}
              onClick={() => setShowPopup(false)}
            >
              Close
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

// CSS-in-JS stilleri
const styles: { [key: string]: React.CSSProperties } = {
  mainContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #ff7eb3, #4b4bf5)",
  },
  card: {
    width: "400px",
    backgroundColor: "#ffffff",
    borderRadius: "12px",
    boxShadow: "0 8px 24px rgba(0,0,0,0.2)",
    padding: "2rem",
    textAlign: "center",
  },
  title: {
    fontSize: "2rem",
    fontWeight: "bold",
    color: "#4b4bf5",
    marginBottom: "1rem",
  },
  formContainer: {
    marginBottom: "1.5rem",
  },
  form: {
    display: "flex",
    flexDirection: "column",
    gap: "1rem",
  },
  inputGroup: {
    textAlign: "left",
  },
  label: {
    fontSize: "0.9rem",
    fontWeight: "500",
    color: "#4b5563",
    marginBottom: "0.5rem",
    display: "block",
  },
  input: {
    width: "100%",
    padding: "0.75rem",
    border: "1px solid #d1d5db",
    borderRadius: "6px",
    fontSize: "1rem",
    outline: "none",
    transition: "border-color 0.3s",
  },
  error: {
    color: "#f87171",
    fontSize: "0.85rem",
    marginTop: "0.25rem",
  },
  submitButton: {
    width: "100%",
    padding: "0.75rem",
    backgroundColor: "#4b4bf5",
    color: "#ffffff",
    borderRadius: "6px",
    fontWeight: "600",
    border: "none",
    cursor: "pointer",
    transition: "background-color 0.3s",
  },
  errorBox: {
    backgroundColor: "#ffe2e2",
    border: "1px solid #f76c6c",
    color: "#d63031",
    padding: "1rem",
    borderRadius: "8px",
    textAlign: "center",
    marginBottom: "1rem",
  },
  popupOverlay: {
    position: "fixed",
    top: 0,
    left: 0,
    width: "100%",
    height: "100%",
    backgroundColor: "rgba(0, 0, 0, 0.5)",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    zIndex: 1000,
  },
  popup: {
    width: "300px",
    backgroundColor: "#fff",
    borderRadius: "12px",
    padding: "1.5rem",
    textAlign: "center",
    boxShadow: "0 8px 24px rgba(0,0,0,0.2)",
  },
  popupTitle: {
    fontSize: "1.5rem",
    fontWeight: "bold",
    marginBottom: "1rem",
    color: "#4b4bf5",
  },
  popupMessage: {
    fontSize: "1rem",
    marginBottom: "1.5rem",
  },
  popupButton: {
    padding: "0.5rem 1rem",
    backgroundColor: "#4b4bf5",
    color: "#fff",
    border: "none",
    borderRadius: "6px",
    cursor: "pointer",
  },
};
