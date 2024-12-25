import React, { useEffect, useState } from "react";
import axios from "axios";

interface CourseSchedule {
  courseId: number;
  courseCode: string;
  dayOfWeek: string;
  startTime: string;
  endTime: string;
}

const days = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
const times = Array.from({ length: 8 }, (_, i) => `${8 + i}:00`);

export function CourseSchedulePage() {
  const [schedules, setSchedules] = useState<CourseSchedule[]>([]);

  useEffect(() => {
    fetchSchedules();
  }, []);

  const fetchSchedules = async () => {
    try {
      const response = await axios.get<CourseSchedule[]>('/api/courseschedules/schedule');
      setSchedules(response.data);
    } catch (error) {
      console.error('Error fetching course schedules:', error);
    }
  };

  const getCourseCode = (day: string, time: string) => {
    const course = schedules.find(
      (schedule) => schedule.dayOfWeek === day && schedule.startTime === time
    );
    return course ? course.courseCode : "";
  };

  return (
    <div style={styles.container}>
      <h1 style={styles.title}>Course Schedule</h1>
      <div style={styles.gridContainer}>
        <div style={styles.timeColumn}>
          <div style={styles.emptyHeader}></div>
          {times.map((time) => (
            <div key={time} style={styles.timeCell}>{time}</div>
          ))}
        </div>
        {days.map((day) => (
          <div key={day} style={styles.dayColumn}>
            <div style={styles.dayHeader}>{day}</div>
            {times.map((time) => (
              <div key={`${day}-${time}`} style={styles.cell}>
                {getCourseCode(day, time)}
              </div>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
}

const styles: { [key: string]: React.CSSProperties } = {
  container: {
    padding: "2rem",
    textAlign: "center",
    backgroundColor: "#f4f6f8",
    minHeight: "100vh",
  },
  title: {
    fontSize: "2rem",
    marginBottom: "1.5rem",
  },
  gridContainer: {
    display: "grid",
    gridTemplateColumns: `120px repeat(${days.length}, 1fr)`,
    gap: "1px",
    backgroundColor: "#ccc",
  },
  timeColumn: {
    display: "flex",
    flexDirection: "column",
    backgroundColor: "#f9f9f9",
  },
  timeCell: {
    height: "50px",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    border: "1px solid #ccc",
    backgroundColor: "#fff",
    fontWeight: "bold",
  },
  dayColumn: {
    display: "flex",
    flexDirection: "column",
    backgroundColor: "#f9f9f9",
  },
  dayHeader: {
    height: "50px",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    fontWeight: "bold",
    backgroundColor: "#4b4bf5",
    color: "#fff",
    border: "1px solid #ccc",
  },
  cell: {
    height: "50px",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    border: "1px solid #ccc",
    backgroundColor: "#fff",
    fontSize: "0.9rem",
  },
  emptyHeader: {
    height: "50px",
    backgroundColor: "#f9f9f9",
  },
};

export default CourseSchedulePage;
