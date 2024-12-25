import React, { useEffect, useState } from "react";
import axios from "axios";


interface StudentDTO {
  studentId: number;
  studentNumber: string;
  fullName: string;
  departmentName: string;
  email: string;
  phoneNumber?: string;
}

export default function Students() {
  const [students, setStudents] = useState<StudentDTO[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedDepartment, setSelectedDepartment] = useState<string>("all");
  const [departments, setDepartments] = useState<string[]>([]);

  useEffect(() => {
    fetchStudents();
  }, []);

  useEffect(() => {
    const uniqueDepartments = Array.from(new Set(students.map((s) => s.departmentName)));
    setDepartments(uniqueDepartments);
  }, [students]);

  const fetchStudents = async () => {
    try {
      const response = await axios.get<StudentDTO[]>("api/students");
      setStudents(response.data);
      setLoading(false);
    } catch (err: any) {
      if (err.response?.status === 404) {
        setError("Hiç öğrenci bulunamadı.");
      } else {
        setError("Öğrenciler getirilirken bir hata oluştu.");
      }
      setLoading(false);
    }
  };

  const filteredStudents =
    selectedDepartment === "all"
      ? students
      : students.filter((student) => student.departmentName === selectedDepartment);

  if (loading)
    return (
      <div style={styles.centeredContainer}>
        <div style={styles.loader}></div>
      </div>
    );

  if (error)
    return (
      <div style={styles.centeredContainer}>
        <div style={styles.errorBox}>{error}</div>
      </div>
    );

  return (
    <div style={styles.pageContainer}>
      
      <div style={styles.mainContent}>
        <div style={styles.card}>
          <div style={styles.header}>
            <h1 style={styles.title}>Öğrenci Listesi</h1>
            <div style={styles.filterContainer}>
              <label htmlFor="department" style={styles.filterLabel}>
                Bölüm:
              </label>
              <select
                id="department"
                style={styles.select}
                value={selectedDepartment}
                onChange={(e) => setSelectedDepartment(e.target.value)}
              >
                <option value="all">Tüm Bölümler</option>
                {departments.map((dept) => (
                  <option key={dept} value={dept}>
                    {dept}
                  </option>
                ))}
              </select>
            </div>
          </div>
          <div style={styles.tableContainer}>
            <table style={styles.table}>
              <thead style={styles.tableHeader}>
                <tr>
                  <th style={styles.th}>Öğrenci No</th>
                  <th style={styles.th}>Ad Soyad</th>
                  <th style={styles.th}>Bölüm</th>
                  <th style={styles.th}>E-posta</th>
                  <th style={styles.th}>Telefon</th>
                </tr>
              </thead>
              <tbody>
                {filteredStudents.map((student) => (
                  <tr key={student.studentId} style={styles.tableRow}>
                    <td style={styles.td}>{student.studentNumber}</td>
                    <td style={styles.td}>{student.fullName}</td>
                    <td style={styles.td}>{student.departmentName}</td>
                    <td style={styles.td}>{student.email}</td>
                    <td style={styles.td}>{student.phoneNumber || "-"}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}

const styles: { [key: string]: React.CSSProperties } = {
  pageContainer: {
    display: "flex",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #ff7eb3, #4b4bf5)",
  },
  mainContent: {
    flex: 1,
    padding: "2rem",
  },
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
    color: "#2c3e50",
  },
  header: {
    marginBottom: "1.5rem",
  },
  title: {
    fontSize: "2rem",
    fontWeight: "bold",
    marginBottom: "1rem",
  },
  filterContainer: {
    display: "flex",
    alignItems: "center",
    gap: "1rem",
  },
  filterLabel: {
    fontSize: "1rem",
    fontWeight: "500",
    color: "#2c3e50",
  },
  select: {
    padding: "0.5rem",
    borderRadius: "8px",
    border: "1px solid #ccc",
    fontSize: "1rem",
  },
  tableContainer: {
    overflowY: "auto",
    maxHeight: "calc(80vh - 100px)",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
    textAlign: "left",
    backgroundColor: "#ffffff",
  },
  tableHeader: {
    backgroundColor: "#4b4bf5",
    color: "#ffffff",
  },
  th: {
    padding: "1rem",
    borderBottom: "1px solid #ccc",
  },
  td: {
    padding: "1rem",
    borderBottom: "1px solid #ccc",
    color: "#2c3e50",
  },
  tableRow: {
    transition: "background-color 0.3s",
    cursor: "pointer",
  },
  loader: {
    border: "4px solid #f3f3f3",
    borderTop: "4px solid #4b4bf5",
    borderRadius: "50%",
    width: "50px",
    height: "50px",
    animation: "spin 1s linear infinite",
  },
  centeredContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    minHeight: "100vh",
    background: "linear-gradient(135deg, #ff7eb3, #4b4bf5)",
    color: "#ffffff",
  },
  errorBox: {
    backgroundColor: "#ffe2e2",
    border: "1px solid #f76c6c",
    color: "#d9534f",
    padding: "1rem",
    borderRadius: "8px",
    textAlign: "center",
  },
};

