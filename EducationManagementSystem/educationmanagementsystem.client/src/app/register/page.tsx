import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import axios from "axios";
import { motion } from "framer-motion";

// Yup Validation Schema
const schema = yup.object().shape({
  firstName: yup.string().required("First name is required"),
  lastName: yup.string().required("Last name is required"),
  email: yup.string().email("Invalid email").required("Email is required"),
  password: yup
    .string()
    .min(6, "Password must be at least 6 characters")
    .required("Password is required"),
  studentNumber: yup.string().required("Student number is required"),
  departmentId: yup
    .number()
    .typeError("Department ID must be a number")
    .required("Department ID is required"),
  phoneNumber: yup.string().nullable(),
  dateOfBirth: yup.date().nullable(),
});

export default function Register() {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    resolver: yupResolver(schema),
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onSubmit = async (data: any) => {
    setLoading(true);
    setError(null);

    try {
      // Prepare data to match API expectations
      const transformedData = {
        ...data,
        departmentId: Number(data.departmentId),
      };

      // API request
      const response = await axios.post(
        "http://localhost:7214/api/auth/register",
        transformedData
      );

      if (response.status === 201) {
        alert("Registration successful! Welcome!");
      } else {
        setError("Registration failed. Please try again.");
      }
    } catch (err: any) {
      console.error("Registration error:", err);
      setError(
        err.response?.data?.message ||
          "A server error occurred. Please try again later."
      );
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
          Register
        </motion.h1>
        <motion.div
          initial={{ scale: 0.9, opacity: 0.8 }}
          animate={{ scale: 1, opacity: 1 }}
          transition={{ duration: 0.5 }}
          style={styles.formContainer}
        >
          {error && <div style={styles.errorBox}>{error}</div>}
          <form onSubmit={handleSubmit(onSubmit)} style={styles.form}>
            <div style={styles.inputGroupContainer}>
              {/* Input Fields */}
              <div style={styles.inputGroup}>
                <label htmlFor="firstName" style={styles.label}>First Name</label>
                <input id="firstName" type="text" {...register("firstName")} style={styles.input} />
                <p style={styles.error}>{errors.firstName?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="lastName" style={styles.label}>Last Name</label>
                <input id="lastName" type="text" {...register("lastName")} style={styles.input} />
                <p style={styles.error}>{errors.lastName?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="email" style={styles.label}>Email</label>
                <input id="email" type="email" {...register("email")} style={styles.input} />
                <p style={styles.error}>{errors.email?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="password" style={styles.label}>Password</label>
                <input id="password" type="password" {...register("password")} style={styles.input} />
                <p style={styles.error}>{errors.password?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="studentNumber" style={styles.label}>Student Number</label>
                <input id="studentNumber" type="text" {...register("studentNumber")} style={styles.input} />
                <p style={styles.error}>{errors.studentNumber?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="departmentId" style={styles.label}>Department ID</label>
                <input id="departmentId" type="number" {...register("departmentId")} style={styles.input} />
                <p style={styles.error}>{errors.departmentId?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="phoneNumber" style={styles.label}>Phone Number</label>
                <input id="phoneNumber" type="text" {...register("phoneNumber")} style={styles.input} />
                <p style={styles.error}>{errors.phoneNumber?.message}</p>
              </div>

              <div style={styles.inputGroup}>
                <label htmlFor="dateOfBirth" style={styles.label}>Date of Birth</label>
                <input id="dateOfBirth" type="date" {...register("dateOfBirth")} style={styles.input} />
                <p style={styles.error}>{errors.dateOfBirth?.message}</p>
              </div>
            </div>
          
            <motion.button
              whileHover={{ scale: 1.05 }}
              whileTap={{ scale: 0.95 }}
              type="submit"
              style={styles.submitButton}
              disabled={loading}
            >
              {loading ? "Registering..." : "Register"}
            </motion.button>
          </form>
        </motion.div>
      </div>
    </div>
  );
}

// Styles
const styles: { [key: string]: React.CSSProperties } = {
  mainContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #ff7eb3, #4b4bf5)",
  },
  card: {
    width: "500px",
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
  inputGroupContainer: {
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
};
