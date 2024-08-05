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

        // New method to get quizzes by admin username
        [HttpGet("quizzes-by-admin")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuizzesByAdmin([FromQuery] string username)
        {
            var admin = await _context.AdminList
                .FirstOrDefaultAsync(a => a.Username == username);

            if (admin == null)
            {
                return NotFound("Admin not found");
            }

            var quizzes = await _context.QuizList
                .Where(q => q.AdminID == admin.AdminID)
                .Select(q => new
                {
                    q.QuizID,
                    q.Title,
                    CategoryTitle = q.Category.Title
                })
                .ToListAsync();

            return Ok(quizzes);
        }

        // New method to get a quiz by ID
        [HttpGet("{quizId}")]
        public async Task<ActionResult<object>> GetQuizById(int quizId)
        {
            var quiz = await _context.QuizList
                .Where(q => q.QuizID == quizId)
                .Select(q => new
                {
                    q.QuizID,
                    q.AdminID,
                    q.Title,
                    q.Description,
                    q.CategoryID
                })
                .FirstOrDefaultAsync();

            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            return Ok(quiz);
        }

        // New method to update a quiz
        [HttpPut("update-quiz/{quizId}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] Quiz updatedQuiz)
        {
            if (quizId != updatedQuiz.QuizID)
            {
                return BadRequest("Quiz ID mismatch");
            }

            var quiz = await _context.QuizList.FindAsync(quizId);

            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            quiz.Title = updatedQuiz.Title;
            quiz.Description = updatedQuiz.Description;
            quiz.CategoryID = updatedQuiz.CategoryID;

            _context.Entry(quiz).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // New method to delete a quiz by quizID
        [HttpDelete("{quizId}")]
        public async Task<IActionResult> DeleteQuiz(int quizId)
        {
            var quiz = await _context.QuizList.FindAsync(quizId);

            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            _context.QuizList.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("quiz-titles-by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetQuizTitlesByCategory(int categoryId)
        {
            var quizTitles = await _context.QuizList
                .Where(q => q.CategoryID == categoryId)
                .Select(q => q.Title)
                .ToListAsync();

            if (quizTitles == null || quizTitles.Count == 0)
            {
                return NotFound("No quizzes found for the given category ID");
            }

            return Ok(quizTitles);
        }
    }
}