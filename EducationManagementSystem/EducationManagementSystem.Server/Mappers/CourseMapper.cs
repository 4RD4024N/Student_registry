using EducationManagementSystem.Server.Models;
using EducationManagementSystem.Server.Data.DTOs;

namespace EducationManagementSystem.Server.Mappers
{
    public static class CourseMapper
    {
        public static CourseDTO MapToDTO(Course course) => new()
        {
            CourseId = course.CourseId,
            CourseCode = course.CourseCode,
            Name = course.Name,
            Description = course.Description,
            Credits = course.Credits,
            DepartmentName = course.Department.Name
        };

        public static CourseScheduleDTO MapToScheduleDTO(CourseSchedule schedule) =>
     new CourseScheduleDTO
     {
         ScheduleId = schedule.ScheduleId,
         CourseCode = schedule.Course.CourseCode, // Course ilişkisinin null olmamasına dikkat edin.
         CourseName = schedule.Course.Name,
         DayOfWeek = schedule.DayOfWeek,
         StartTime = schedule.StartTime,
         EndTime = schedule.EndTime,
         
     };


    }
}
