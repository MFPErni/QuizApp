// Controllers/QuizController.cs
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

        // GET: api/Quiz
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quiz>>> GetQuizzes()
        {
            return await _context.QuizList.ToListAsync();
        }

        // GET: api/Quiz/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            var quiz = await _context.QuizList.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        // POST: api/Quiz
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(Quiz quiz)
        {
            _context.QuizList.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.QuizID }, quiz);
        }

        // PUT: api/Quiz/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, Quiz quiz)
        {
            if (id != quiz.QuizID)
            {
                return BadRequest();
            }

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
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

        // DELETE: api/Quiz/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.QuizList.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.QuizList.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
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

        private bool QuizExists(int id)
        {
            return _context.QuizList.Any(e => e.QuizID == id);
        }
    }
}