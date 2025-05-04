using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VehicleAccountingAPI.Data;
using VehicleAccountingAPI.Models;

namespace VehicleAccountingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRecordsController : ControllerBase
    {
        private readonly VehicleAccountingContext _context;

        public MaintenanceRecordsController(VehicleAccountingContext context)
        {
            _context = context;
        }

        // GET: api/MaintenanceRecords
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetMaintenanceRecords()
        {
            return await _context.MaintenanceRecords
                .Include(mr => mr.Vehicle)
                .ToListAsync();
        }

        // GET: api/MaintenanceRecords/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceRecord>> GetMaintenanceRecord(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(mr => mr.Vehicle)
                .FirstOrDefaultAsync(mr => mr.MaintenanceRecordId == id);

            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            return maintenanceRecord;
        }

        // PUT: api/MaintenanceRecords/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenanceRecord(int id, MaintenanceRecord maintenanceRecord)
        {
            if (id != maintenanceRecord.MaintenanceRecordId)
            {
                return BadRequest();
            }

            var validationResults = maintenanceRecord.Validate(new ValidationContext(maintenanceRecord));
            if (validationResults.Any())
            {
                return BadRequest(validationResults.Select(vr => vr.ErrorMessage).FirstOrDefault());
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(maintenanceRecord).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceRecordExists(id))
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

        // POST: api/MaintenanceRecords
        [HttpPost]
        public async Task<ActionResult<MaintenanceRecord>> PostMaintenanceRecord(MaintenanceRecord maintenanceRecord)
        {
            var validationResults = maintenanceRecord.Validate(new ValidationContext(maintenanceRecord));
            if (validationResults.Any())
            {
                return BadRequest(validationResults.Select(vr => vr.ErrorMessage).FirstOrDefault());
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MaintenanceRecords.Add(maintenanceRecord);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaintenanceRecord), new { id = maintenanceRecord.MaintenanceRecordId }, maintenanceRecord);
        }

        // DELETE: api/MaintenanceRecords/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenanceRecord(int id)
        {
            var maintenanceRecord = await _context.MaintenanceRecords.FindAsync(id);
            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            _context.MaintenanceRecords.Remove(maintenanceRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaintenanceRecordExists(int id)
        {
            return _context.MaintenanceRecords.Any(e => e.MaintenanceRecordId == id);
        }

        // GET: api/MaintenanceRecords/costs-by-month
        [HttpGet("costs-by-month")]
        public async Task<ActionResult<IEnumerable<object>>> GetCostsByMonth()
        {
            var costsByMonth = await _context.MaintenanceRecords
                .GroupBy(mr => new { mr.MaintenanceDate.Year, mr.MaintenanceDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalCost = g.Sum(mr => mr.Cost)
                })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToListAsync();

            return costsByMonth;
        }

        // GET: api/MaintenanceRecords/recent
        [HttpGet("recent")]
        public async Task<ActionResult<IEnumerable<MaintenanceRecord>>> GetRecentMaintenanceRecords()
        {
            return await _context.MaintenanceRecords
                .Include(mr => mr.Vehicle)
                .OrderByDescending(mr => mr.MaintenanceDate)
                .Take(5)
                .ToListAsync();
        }
    }

}