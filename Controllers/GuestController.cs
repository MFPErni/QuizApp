// Controllers/GuestController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntroBE.Data;
using IntroBE.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly DataContext _context;

        public GuestController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Guest
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Guest>>> GetGuests()
        {
            return await _context.GuestList.ToListAsync();
        }

        // GET: api/Guest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Guest>> GetGuest(int id)
        {
            var guest = await _context.GuestList.FindAsync(id);

            if (guest == null)
            {
                return NotFound();
            }

            return guest;
        }

        // POST: api/Guest
        [HttpPost]
        public async Task<ActionResult<Guest>> PostGuest(Guest guest)
        {
            _context.GuestList.Add(guest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGuest", new { id = guest.GuestID }, guest);
        }

        // PUT: api/Guest/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuest(int id, Guest guest)
        {
            if (id != guest.GuestID)
            {
                return BadRequest();
            }

            _context.Entry(guest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuestExists(id))
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

        // DELETE: api/Guest/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            var guest = await _context.GuestList.FindAsync(id);
            if (guest == null)
            {
                return NotFound();
            }

            _context.GuestList.Remove(guest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GuestExists(int id)
        {
            return _context.GuestList.Any(e => e.GuestID == id);
        }
    }
}