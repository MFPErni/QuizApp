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
    public class AnswerController : ControllerBase
    {
        private readonly DataContext _context;

        public AnswerController(DataContext context)
        {
            _context = context;
        }

        // GET: api/answer/question/{questionText}
        [HttpGet("question/{questionText}")]
        public async Task<ActionResult<IEnumerable<string>>> GetAnswersByQuestionText(string questionText)
        {
            // Find the question by questionText
            var question = await _context.QuestionList
                .FirstOrDefaultAsync(q => q.QuestionText == questionText);

            if (question == null)
            {
                return NotFound($"No question found with text: {questionText}");
            }

            // Get the answers associated with the question and select only the answerText
            var answerTexts = await _context.AnswerList
                .Where(a => a.QuestionID == question.QuestionID)
                .Select(a => a.AnswerText)
                .ToListAsync();

            if (answerTexts == null || answerTexts.Count == 0)
            {
                return NotFound($"No answers found for question: {questionText}");
            }

            return Ok(answerTexts);
        }

        // GET: api/answer/check/{answerText}
        [HttpGet("check/{answerText}")]
        public async Task<ActionResult<object>> CheckAnswerText(string answerText)
        {
            // Find the answer by answerText
            var answer = await _context.AnswerList
                .FirstOrDefaultAsync(a => a.AnswerText == answerText);

            if (answer == null)
            {
                return NotFound($"No answer found with text: {answerText}");
            }

            // Return the answerText and isCorrect value
            return Ok(new { answer.AnswerText, answer.IsCorrect });
        }
    }
}