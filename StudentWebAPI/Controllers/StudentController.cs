using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.Models;
using StudentWebAPI.Dtos;
using StudentWebAPI.Mappers;
using System.Text.RegularExpressions;

namespace StudentWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private static readonly List<Student> students = new List<Student>();

        
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return Ok(students);
        }

        
        [HttpGet("{id:int}")]
        public ActionResult<Student> GetStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        
        [HttpPost]
        public ActionResult<Student> AddStudent([FromBody] AddStudentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Student data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.StudentNumber))
            {
                return BadRequest("StudentNumber is required.");
            }

            if (!Regex.IsMatch(dto.StudentNumber, @"^\d{4}-\d{4}$"))
            {
                return BadRequest("StudentNumber must follow the format YYYY-####.");
            }

            if (students.Any(s => s.StudentNumber.Equals(
                dto.StudentNumber.Trim(),
                StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict("StudentNumber already exists.");
            }

            var student = StudentMapper.ToStudent(dto);

            student.Id = students.Count == 0
                ? 1
                : students.Max(s => s.Id) + 1;

            students.Add(student);

            return CreatedAtAction(
                nameof(GetStudent),
                new { id = student.Id },
                student);
        }

        
        [HttpPut("{id:int}")]
        public ActionResult<Student> EditStudent(
            int id,
            [FromBody] EditStudentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Student data is required.");
            }

            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(dto.StudentNumber))
            {
                return BadRequest("StudentNumber is required.");
            }

            if (!Regex.IsMatch(dto.StudentNumber, @"^\d{4}-\d{4}$"))
            {
                return BadRequest("StudentNumber must follow the format YYYY-####.");
            }

            if (students.Any(s =>
                s.Id != id &&
                s.StudentNumber.Equals(
                    dto.StudentNumber.Trim(),
                    StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict("StudentNumber already exists.");
            }

            StudentMapper.UpdateStudent(student, dto);

            return Ok(student);
        }
        
        [HttpDelete("{id:int}")]
        public ActionResult DeleteStudent(int id)
        {
            var student = students.FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            students.Remove(student);

            return NoContent();
        }
        
        [HttpGet("search")]
        public ActionResult<IEnumerable<Student>> SearchStudents(
            [FromQuery] string? lastName,
            [FromQuery] string? firstName)
        {
            var results = students.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                results = results.Where(s =>
                    s.LastName.Contains(
                        lastName.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                results = results.Where(s =>
                    s.FirstName.Contains(
                        firstName.Trim(),
                        StringComparison.OrdinalIgnoreCase));
            }

            return Ok(results.ToList());
        }
    }
}