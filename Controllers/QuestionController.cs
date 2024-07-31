// Controllers/QuestionController.cs
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

        // GET: api/Question
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Question>>> GetQuestions()
        {
            return await _context.QuestionList.ToListAsync();
        }

        // GET: api/Question/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.QuestionList.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        // GET: api/Question/5/answers
        [HttpGet("{id}/answers")]
        public async Task<ActionResult<IEnumerable<object>>> GetAnswersForQuestion(int id)
        {
            var question = await _context.QuestionList
                                        .Include(q => q.Answers)
                                        .FirstOrDefaultAsync(q => q.QuestionID == id);

            if (question == null)
            {
                return NotFound();
            }

            var answers = question.Answers.Select(a => new {
                a.AnswerText,
                a.IsCorrect
            }).ToList();

            return Ok(answers);
        }

        // POST: api/Question
        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion(Question question)
        {
            _context.QuestionList.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.QuestionID }, question);
        }

        // PUT: api/Question/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionID)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // DELETE: api/Question/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.QuestionList.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.QuestionList.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool QuestionExists(int id)
        {
            return _context.QuestionList.Any(e => e.QuestionID == id);
        }
    }
}