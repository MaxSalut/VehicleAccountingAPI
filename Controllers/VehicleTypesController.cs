using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehicleAccountingAPI.Data;
using VehicleAccountingAPI.Models;

namespace VehicleAccountingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleTypesController : ControllerBase
    {
        private readonly VehicleAccountingContext _context;

        public VehicleTypesController(VehicleAccountingContext context)
        {
            _context = context;
        }

        // GET: api/VehicleTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
        {
            return await _context.VehicleTypes
                .ToListAsync();
        }

        // GET: api/VehicleTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VehicleType>> GetVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes
                .Include(vt => vt.Vehicles)
                .FirstOrDefaultAsync(vt => vt.VehicleTypeId == id);

            if (vehicleType == null)
            {
                return NotFound();
            }

            return vehicleType;
        }

        // PUT: api/VehicleTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehicleType(int id, VehicleType vehicleType)
        {
            if (id != vehicleType.VehicleTypeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(vehicleType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleTypeExists(id))
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

        // POST: api/VehicleTypes
        [HttpPost]
        public async Task<ActionResult<VehicleType>> PostVehicleType(VehicleType vehicleType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehicleType), new { id = vehicleType.VehicleTypeId }, vehicleType);
        }

        // DELETE: api/VehicleTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.VehicleTypeId == id);
        }
    }
}