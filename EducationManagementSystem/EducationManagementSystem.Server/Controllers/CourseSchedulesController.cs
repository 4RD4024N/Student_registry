using EducationManagementSystem.Server.Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationManagementSystem.Server.Data;

[ApiController]
[Route("api/[controller]")]
public class CourseSchedulesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CourseSchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("schedule")]
    public async Task<ActionResult<IEnumerable<CourseScheduleDTO>>> GetCourseSchedules()
    {
        var schedules = await _context.CourseSchedules
            .Include(cs => cs.Course)
            .Select(cs => new CourseScheduleDTO
            {
                CourseName = cs.Course.Name,
                DayOfWeek = cs.DayOfWeek,
                StartTime = cs.StartTime,
                EndTime = cs.EndTime
            })
            .ToListAsync();

        return Ok(schedules);
    }
}
