using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationManagementSystem.Server.Data;
using EducationManagementSystem.Server.Data.DTOs;
using EducationManagementSystem.Server.Mappers;
using EducationManagementSystem.Server.Models;

namespace EducationManagementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Department)
                .ToListAsync();

            return Ok(courses.Select(CourseMapper.MapToDTO));
        }

        [HttpPost]
        public async Task<ActionResult<CourseDTO>> CreateCourse(CreateCourseDTO createCourseDto)
        {
            var course = new Course
            {
                CourseCode = createCourseDto.CourseCode,
                Name = createCourseDto.Name,
                Description = createCourseDto.Description,
                Credits = createCourseDto.Credits,
                DepartmentId = createCourseDto.DepartmentId
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourses), new { id = course.CourseId }, CourseMapper.MapToDTO(course));
        }

        [HttpGet("schedules")]
        public async Task<ActionResult<IEnumerable<CourseScheduleDTO>>> GetCourseSchedules()
        {
            var schedules = await _context.CourseSchedules
                .Include(cs => cs.Course)
                .ToListAsync();

            return Ok(schedules.Select(CourseMapper.MapToScheduleDTO));
        }
    }
}
