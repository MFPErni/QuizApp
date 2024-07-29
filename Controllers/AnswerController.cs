// Controllers/AnswerController.cs
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

        // GET: api/Answer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Answer>>> GetAnswers()
        {
            return await _context.AnswerList.ToListAsync();
        }

        // GET: api/Answer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
            var answer = await _context.AnswerList.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        // POST: api/Answer
        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
        {
            _context.AnswerList.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAnswer", new { id = answer.AnswerID }, answer);
        }

        // PUT: api/Answer/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, Answer answer)
        {
            if (id != answer.AnswerID)
            {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
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

        // DELETE: api/Answer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnswer(int id)
        {
            var answer = await _context.AnswerList.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }

            _context.AnswerList.Remove(answer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AnswerExists(int id)
        {
            return _context.AnswerList.Any(e => e.AnswerID == id);
        }
    }
}