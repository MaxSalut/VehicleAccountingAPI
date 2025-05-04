using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleAccountingAPI.Data;
using VehicleAccountingAPI.Models;

namespace VehicleAccountingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly VehicleAccountingContext _context;

        public AssignmentsController(VehicleAccountingContext context)
        {
            _context = context;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Assignment>>> GetAssignments()
        {
            return await _context.Assignments
                .Include(a => a.Vehicle)
                .Include(a => a.Driver)
                .ToListAsync();
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Assignment>> GetAssignment(int id)
        {
            var assignment = await _context.Assignments
                .Include(a => a.Vehicle)
                .Include(a => a.Driver)
                .FirstOrDefaultAsync(a => a.AssignmentId == id);

            if (assignment == null)
            {
                return NotFound();
            }

            return assignment;
        }

        // PUT: api/Assignments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssignment(int id, Assignment assignment)
        {
            if (id != assignment.AssignmentId)
            {
                return BadRequest();
            }

            var validationResults = assignment.Validate(new ValidationContext(assignment));
            if (validationResults.Any())
            {
                return BadRequest(validationResults.Select(vr => vr.ErrorMessage).FirstOrDefault());
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(assignment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssignmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Assignments
        [HttpPost]
        public async Task<ActionResult<Assignment>> PostAssignment(Assignment assignment)
        {
            var validationResults = assignment.Validate(new ValidationContext(assignment));
            if (validationResults.Any())
            {
                return BadRequest(validationResults.Select(vr => vr.ErrorMessage).FirstOrDefault());
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAssignment), new { id = assignment.AssignmentId }, assignment);
        }

        // DELETE: api/Assignments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }

            _context.Assignments.Remove(assignment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.AssignmentId == id);
        }
        // GET: api/Assignments/status-count
        [HttpGet("status-count")]
        public async Task<ActionResult<object>> GetAssignmentStatusCount()
        {
            var activeCount = await _context.Assignments
                .CountAsync(a => a.EndDate == null);
            var inactiveCount = await _context.Assignments
                .CountAsync(a => a.EndDate != null);

            return new
            {
                Active = activeCount,
                Inactive = inactiveCount
            };
        }
    }
}