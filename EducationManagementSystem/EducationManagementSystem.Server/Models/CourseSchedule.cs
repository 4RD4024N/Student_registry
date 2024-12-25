using System.ComponentModel.DataAnnotations;
using EducationManagementSystem.Server.Models;
using Microsoft.EntityFrameworkCore;
public class CourseSchedule
{
    [Key]
    public int ScheduleId { get; set; } 
    public int CourseId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Room { get; set; } = null!;
    public string Semester { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
