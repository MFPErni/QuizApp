// Controllers/CategoryController.cs
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
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        // Method to get all category titles
        [HttpGet("titles")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategoryTitles()
        {
            var categoryTitles = await _context.CategoryList
                .Select(c => c.Title)
                .ToListAsync();

            return Ok(categoryTitles);
        }
    }
}