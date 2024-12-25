/* eslint-disable @typescript-eslint/no-unused-vars */
import React, { useEffect, useState } from "react";
import axios from "axios";

interface CourseDTO {
  courseId: number;
  courseCode: string;
  name: string;
  description?: string;
  credits: number;
  departmentName: string;
}

const CourseList = () => {
  const [courses, setCourses] = useState<CourseDTO[]>([]);
  const [departments, setDepartments] = useState<string[]>([]);
  const [selectedDepartment, setSelectedDepartment] = useState<string>("all");
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [newCourse, setNewCourse] = useState<Omit<CourseDTO, "courseId">>({
    courseCode: "",
    name: "",
    description: "",
    credits: 0,
    departmentName: "",
  });

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        const response = await axios.get<CourseDTO[]>("api/courses");
        setCourses(response.data);
        const uniqueDepartments = [
          ...new Set(response.data.map((course) => course.departmentName)),
        ];
        setDepartments(uniqueDepartments);
      } catch (err) {
        console.error("API Error:", err);
        setError("Dersler yüklenirken hata oluştu");
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  const filteredCourses =
    selectedDepartment === "all"
      ? courses
      : courses.filter((course) => course.departmentName === selectedDepartment);

  const addCourse = async () => {
    try {
      const response = await axios.post<CourseDTO>("api/courses", newCourse);
      setCourses((prevCourses) => [...prevCourses, response.data]);
      setNewCourse({
        courseCode: "",
        name: "",
        description: "",
        credits: 0,
        departmentName: "",
      });
    } catch (err) {
      console.error("API Error:", err);
      setError("Ders eklenirken hata oluştu");
    }
  };

  const deleteCourse = async (courseId: number) => {
    try {
      await axios.delete(`api/courses/${courseId}`);
      setCourses((prevCourses) =>
        prevCourses.filter((course) => course.courseId !== courseId)
      );
    } catch (err) {
      console.error("API Error:", err);
      setError("Ders silinirken hata oluştu");
    }
  };

  if (loading)
    return (
      <div style={styles.loadingContainer}>
        <div style={styles.spinner}></div>
      </div>
    );

  if (error)
    return (
      <div style={styles.errorContainer}>
        <div style={styles.errorBox}>
          <p style={styles.errorText}>{error}</p>
          <button style={styles.retryButton} onClick={() => window.location.reload()}>
            Yeniden Dene
          </button>
        </div>
      </div>
    );

  return (
    <div style={styles.mainContainer}>
      <div style={styles.header}>
        <h1 style={styles.title}>Ders Listesi</h1>
        <select
          style={styles.filterDropdown}
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

      <table style={styles.table}>
        <thead>
          <tr>
            <th style={styles.th}>Ders Kodu</th>
            <th style={styles.th}>Ad</th>
            <th style={styles.th}>Kredi</th>
            <th style={styles.th}>Bölüm</th>
            <th style={styles.th}>Aksiyon</th>
          </tr>
        </thead>
        <tbody>
          {filteredCourses.map((course) => (
            <tr key={course.courseId}>
              <td style={styles.td}>{course.courseCode}</td>
              <td style={styles.td}>{course.name}</td>
              <td style={styles.td}>{course.credits}</td>
              <td style={styles.td}>{course.departmentName}</td>
              <td style={styles.td}>
                <button
                  style={styles.deleteButton}
                  onClick={() => deleteCourse(course.courseId)}
                >
                  Sil
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <div style={styles.addCourseForm}>
        <h2 style={styles.formTitle}>Yeni Ders Ekle</h2>
        <input
          style={styles.input}
          type="text"
          placeholder="Ders Kodu"
          value={newCourse.courseCode}
          onChange={(e) =>
            setNewCourse((prev) => ({ ...prev, courseCode: e.target.value }))
          }
        />
        <input
          style={styles.input}
          type="text"
          placeholder="Ad"
          value={newCourse.name}
          onChange={(e) =>
            setNewCourse((prev) => ({ ...prev, name: e.target.value }))
          }
        />
        <input
          style={styles.input}
          type="number"
          placeholder="Kredi"
          value={newCourse.credits}
          onChange={(e) =>
            setNewCourse((prev) => ({ ...prev, credits: +e.target.value }))
          }
        />
        <input
          style={styles.input}
          type="text"
          placeholder="Bölüm"
          value={newCourse.departmentName}
          onChange={(e) =>
            setNewCourse((prev) => ({ ...prev, departmentName: e.target.value }))
          }
        />
        <button style={styles.addButton} onClick={addCourse}>
          Ekle
        </button>
      </div>
    </div>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  mainContainer: {
    padding: "2rem",
    maxWidth: "1200px",
    margin: "0 auto",
    textAlign: "center",
    backgroundColor: "#f9fafb",
  },
  header: {
    display: "flex",
    justifyContent: "space-between",
    marginBottom: "1.5rem",
    alignItems: "center",
  },
  title: {
    fontSize: "2rem",
    fontWeight: "bold",
    color: "#1f2937",
  },
  filterDropdown: {
    padding: "0.5rem",
    borderRadius: "8px",
    border: "1px solid #d1d5db",
    outline: "none",
    backgroundColor: "#ffffff",
    color: "#1f2937",
    fontWeight: "500",
    cursor: "pointer",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
    marginBottom: "1.5rem",
  },
  th: {
    padding: "1rem",
    fontWeight: "bold",
    backgroundColor: "#3b82f6",
    color: "#ffffff",
    textAlign: "left",
  },
  td: {
    padding: "1rem",
    borderBottom: "1px solid #d1d5db",
    textAlign: "left",
  },
  deleteButton: {
    backgroundColor: "#ef4444",
    color: "#ffffff",
    border: "none",
    padding: "0.5rem 1rem",
    borderRadius: "8px",
    cursor: "pointer",
  },
  addCourseForm: {
    textAlign: "left",
    marginTop: "2rem",
  },
  formTitle: {
    fontSize: "1.5rem",
    marginBottom: "1rem",
  },
  input: {
    display: "block",
    width: "100%",
    padding: "0.5rem",
    marginBottom: "1rem",
    borderRadius: "8px",
    border: "1px solid #d1d5db",
  },
  addButton: {
    backgroundColor: "#10b981",
    color: "#ffffff",
    border: "none",
    padding: "0.5rem 1rem",
    borderRadius: "8px",
    cursor: "pointer",
  },
  loadingContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    height: "100vh",
    backgroundColor: "#f9fafb",
  },
  spinner: {
    width: "50px",
    height: "50px",
    border: "4px solid #e5e7eb",
    borderTop: "4px solid #3b82f6",
    borderRadius: "50%",
    animation: "spin 1s linear infinite",
  },
  errorContainer: {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    height: "100vh",
    backgroundColor: "#fef2f2",
  },
  errorBox: {
    textAlign: "center",
    backgroundColor: "#fee2e2",
    padding: "1rem",
    borderRadius: "8px",
    border: "1px solid #fca5a5",
  },
  errorText: {
    color: "#b91c1c",
    fontSize: "1.2rem",
    marginBottom: "0.5rem",
  },
  retryButton: {
    padding: "0.5rem 1rem",
    backgroundColor: "#3b82f6",
    color: "#ffffff",
    border: "none",
    borderRadius: "8px",
    cursor: "pointer",
    transition: "background-color 0.3s ease",
  },
};

export default CourseList;
