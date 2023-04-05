using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab6.Data;
using Lab6.Models;

namespace Lab6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)] //returned when we return a list of students successfully
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //returned when there is an error processing the request
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //returned when we successfully return a student by their id
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//returned when there is an error processing the request
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returned when the student is not found
        public async Task<ActionResult<Student>> GetStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)] //returned when we update the student with the new student information and return a student
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//returned when there is an error processing the request
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returned when the student ID isn't found
        public async Task<IActionResult> PutStudent(Guid id, Student student)
        {
            if (id.Equals(null))
            {  
                return BadRequest();
            }
            
            var oldStudent = await _context.Students.FindAsync(id);
            oldStudent.FirstName = student.FirstName;
            oldStudent.LastName = student.LastName;
            oldStudent.Program = student.Program;
            _context.Entry(oldStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(oldStudent);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)] //returned when new student is created
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//returned when there is an error processing the request
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]//returned when there is an error processing the request
        [ProducesResponseType(StatusCodes.Status404NotFound)] //returned if student is null
        [ProducesResponseType(StatusCodes.Status204NoContent)] //returned when successful but returns nothing
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
