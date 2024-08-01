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

        // Existing method to get unique category titles
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetUniqueCategoryTitles()
        {
            var uniqueCategoryIds = await _context.QuizList
                .Select(q => q.CategoryID)
                .Distinct()
                .ToListAsync();

            var categoryTitles = await _context.CategoryList
                .Where(c => uniqueCategoryIds.Contains(c.CategoryID))
                .Select(c => c.Title)
                .ToListAsync();

            return categoryTitles; // Return the list directly
        }

        // New method to get quizzes by category
        [HttpGet("quizzes-by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuizzesByCategory(int categoryId)
        {
            var quizzes = await _context.QuizList
                .Where(q => q.CategoryID == categoryId)
                .Select(q => new
                {
                    q.QuizID,
                    q.Title,
                    q.Description,
                    CategoryTitle = q.Category.Title
                })
                .ToListAsync();

            return Ok(quizzes);
        }
    }
}