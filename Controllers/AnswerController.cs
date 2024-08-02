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

        // New method to get answers by questionID
        [HttpGet("answers-by-question/{questionId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAnswersByQuestion(int questionId)
        {
            var answers = await _context.AnswerList
                .Where(a => a.QuestionID == questionId)
                .Select(a => new
                {
                    a.AnswerID,
                    a.QuestionID,
                    a.AnswerText,
                    a.IsCorrect
                })
                .ToListAsync();

            if (answers == null || answers.Count == 0)
            {
                return NotFound($"No answers found for QuestionID: {questionId}");
            }

            return Ok(answers);
        }

        // New method to delete an answer by answerID
        [HttpDelete("{answerId}")]
        public async Task<IActionResult> DeleteAnswer(int answerId)
        {
            var answer = await _context.AnswerList.FindAsync(answerId);

            if (answer == null)
            {
                return NotFound("Answer not found");
            }

            _context.AnswerList.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}