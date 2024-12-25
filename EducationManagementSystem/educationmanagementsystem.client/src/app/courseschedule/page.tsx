import React, { useEffect, useState } from "react";

interface CourseSchedule {
  courseName: string;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
}

const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
const times = Array.from({ length: 10 }, (_, i) => `${8 + i}:00`);

export function CourseSchedulePage() {
  const [schedules, setSchedules] = useState<CourseSchedule[]>([]);

  useEffect(() => {
    fetchSchedules();
  }, []);

  const fetchSchedules = async () => {
    try {
      const response = await fetch("/api/courses/schedule");
      const data = await response.json();
      setSchedules(data);
    } catch (error) {
      console.error("Error fetching course schedules:", error);
    }
  };

  const getSchedule = (day: string, time: string) => {
    const schedule = schedules.find(
      (s) =>
        s.dayOfWeek === day &&
        s.startTime === time
    );
    return schedule ? schedule.courseName : "";
  };

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Course Schedule</h1>
      <table style={styles.table}>
        <thead>
          <tr>
            <th style={styles.headerCell}>Time/Day</th>
            {days.map((day) => (
              <th key={day} style={styles.headerCell}>{day}</th>
            ))}
          </tr>
        </thead>
        <tbody>
          {times.map((time) => (
            <tr key={time}>
              <td style={styles.timeCell}>{time}</td>
              {days.map((day) => (
                <td key={`${day}-${time}`} style={styles.cell}>
                  {getSchedule(day, time)}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    padding: "2rem",
    textAlign: "center",
  },
  title: {
    fontSize: "2rem",
    marginBottom: "1rem",
  },
  table: {
    width: "100%",
    borderCollapse: "collapse",
    textAlign: "center",
  },
  headerCell: {
    padding: "1rem",
    fontWeight: "bold",
    border: "1px solid #ccc",
    backgroundColor: "#f4f4f4",
  },
  timeCell: {
    padding: "0.5rem",
    border: "1px solid #ccc",
    backgroundColor: "#fafafa",
    fontWeight: "bold",
  },
  cell: {
    padding: "0.5rem",
    border: "1px solid #ccc",
    minWidth: "100px",
  },
};

export default CourseSchedule;

