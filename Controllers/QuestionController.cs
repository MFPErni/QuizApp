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
    public class QuestionController : ControllerBase
    {
        private readonly DataContext _context;

        public QuestionController(DataContext context)
        {
            _context = context;
        }

        // New method to get questions by quizID
        [HttpGet("questions-by-quiz/{quizId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetQuestionsByQuiz(int quizId)
        {
            var questions = await _context.QuestionList
                .Where(q => q.QuizID == quizId)
                .Select(q => new { q.QuestionID, q.QuestionText })
                .ToListAsync();

            if (questions == null || questions.Count == 0)
            {
                return NotFound($"No questions found for QuizID: {quizId}");
            }

            return Ok(questions);
        }

        // New method to delete a question by questionID
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int questionId)
        {
            var question = await _context.QuestionList.FindAsync(questionId);

            if (question == null)
            {
                return NotFound("Question not found");
            }

            _context.QuestionList.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}