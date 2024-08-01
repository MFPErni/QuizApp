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
    public class QuizController : ControllerBase
    {
        private readonly DataContext _context;

        public QuizController(DataContext context)
        {
            _context = context;
        }

        // HTTP GET method to get unique CategoryIDs from QuizList and their Titles from CategoryList
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetUniqueCategories()
        {
            // Get unique CategoryIDs from QuizList
            var uniqueCategoryIds = await _context.QuizList
                .Select(q => q.CategoryID)
                .Distinct()
                .ToListAsync();

            // Get Titles from CategoryList based on unique CategoryIDs
            var categoryTitles = await _context.CategoryList
                .Where(c => uniqueCategoryIds.Contains(c.CategoryID))
                .Select(c => c.Title)
                .ToListAsync();

            return Ok(categoryTitles);
        }

        // HTTP GET method to list all quizzes associated with a specific CategoryID
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuizzesByCategory(int categoryId)
        {
            var quizzes = await _context.QuizList
                .Where(q => q.CategoryID == categoryId)
                .Select(q => new
                {
                    q.Title,
                    q.Description,
                    CategoryTitle = q.Category.Title
                })
                .ToListAsync();

            if (quizzes == null || quizzes.Count == 0)
            {
                return NotFound($"No quizzes found for CategoryID {categoryId}");
            }

            return Ok(quizzes);
        }
    }
}