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

        // Existing method to get all category titles
        [HttpGet("titles")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategoryTitles()
        {
            var categoryTitles = await _context.CategoryList
                .Select(c => c.Title)
                .ToListAsync();

            return Ok(categoryTitles);
        }

        // New method to get CategoryID by category title
        [HttpGet("category-id")]
        public async Task<ActionResult<int>> GetCategoryIdByTitle([FromQuery] string title)
        {
            var category = await _context.CategoryList
                .Where(c => c.Title == title)
                .Select(c => c.CategoryID)
                .FirstOrDefaultAsync();

            if (category == 0)
            {
                return NotFound("Category not found");
            }

            return Ok(category);
        }
    }
}