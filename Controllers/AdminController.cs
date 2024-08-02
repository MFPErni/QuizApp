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
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        // Existing method to get all admins
        [HttpGet("list-all-admins")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllAdmins()
        {
            var admins = await _context.AdminList
                .Select(a => new
                {
                    a.Username,
                    a.Password
                })
                .ToListAsync();

            return Ok(admins);
        }

        // New method to check if a username and password match
        [HttpGet("check-credentials")]
        public async Task<ActionResult<string>> CheckCredentials([FromQuery] string username, [FromQuery] string password)
        {
            var admin = await _context.AdminList
                .FirstOrDefaultAsync(a => a.Username == username);

            if (admin == null)
            {
                return NotFound("Username doesn't exist");
            }

            if (admin.Password != password)
            {
                return BadRequest("Wrong password");
            }

            return Ok("Login successful");
        }

        // New method to check if a username already exists
        [HttpGet("username-exists")]
        public async Task<ActionResult<bool>> UsernameExists([FromQuery] string username)
        {
            var exists = await _context.AdminList
                .AnyAsync(a => a.Username == username);

            return Ok(exists);
        }

        // New method to create a new admin
        [HttpPost("create-admin")]
        public async Task<ActionResult<Admin>> CreateAdmin([FromBody] AdminCreateRequest request)
        {
            // Check if the username already exists
            var exists = await _context.AdminList.AnyAsync(a => a.Username == request.Username);
            if (exists)
            {
                return BadRequest("Username already exists");
            }

            // Create a new admin entity
            var newAdmin = new Admin
            {
                Username = request.Username,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            // Add the new admin to the database
            _context.AdminList.Add(newAdmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminById), new { id = newAdmin.AdminID }, newAdmin);
        }

        // Method to get an admin by ID (for CreatedAtAction)
        [HttpGet("{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(int id)
        {
            var admin = await _context.AdminList.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            return admin;
        }

        // Request model for creating an admin
        public class AdminCreateRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}