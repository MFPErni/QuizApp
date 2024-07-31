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

        // GET: api/Quiz/UniqueCategories
        [HttpGet("UniqueCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetUniqueCategories()
        {
            var uniqueCategoryIDs = await _context.QuizList
                                                  .Select(q => q.CategoryID)
                                                  .Distinct()
                                                  .ToListAsync();

            var uniqueCategories = await _context.CategoryList
                                                 .Where(c => uniqueCategoryIDs.Contains(c.CategoryID))
                                                 .ToListAsync();

            return Ok(uniqueCategories);
        }

        [HttpGet("ByCategory/{categoryId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetQuizzesByCategory(int categoryId)
        {
            var quizTitles = await _context.QuizList
                                           .Where(q => q.CategoryID == categoryId)
                                           .Select(q => q.Title)
                                           .ToListAsync();

            if (quizTitles == null || !quizTitles.Any())
            {
                return NotFound($"No quizzes found for CategoryID {categoryId}");
            }

            return Ok(quizTitles);
        }

        

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<object>> GetQuizById(int id)
        {
            var quiz = await _context.QuizList
                                    .Include(q => q.Category) // Include related data
                                    .FirstOrDefaultAsync(q => q.QuizID == id);

            if (quiz == null)
            {
                return NotFound();
            }

            var result = new
            {
                QuizTitle = quiz.Title,
                QuizDescription = quiz.Description,
                CategoryTitle = quiz.Category.Title
            };

            return Ok(result);
        }

        private bool QuizExists(int id)
        {
            return _context.QuizList.Any(e => e.QuizID == id);
        }
    }
}