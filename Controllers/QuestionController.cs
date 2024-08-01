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

        // GET: api/question/quiz/title/{quizTitle}
        [HttpGet("quiz/title/{quizTitle}")]
        public async Task<ActionResult<IEnumerable<string>>> GetQuestionsByQuizTitle(string quizTitle)
        {
            var questions = await _context.QuestionList
                .Where(q => q.Quiz.Title == quizTitle)
                .Select(q => q.QuestionText)
                .ToListAsync();

            if (questions == null || questions.Count == 0)
            {
                return NotFound($"No questions found for QuizTitle {quizTitle}");
            }

            return Ok(questions);
        }
    }
}