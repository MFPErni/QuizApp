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

        // New method to update a quiz
        [HttpPut("update-quiz/{quizId}")]
        public async Task<IActionResult> UpdateQuiz(int quizId, [FromBody] QuizCreateDto quizDto)
        {
            var quiz = await _context.QuizList
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuizID == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            quiz.AdminID = quizDto.AdminID;
            quiz.Title = quizDto.Title;
            quiz.Description = quizDto.Description;
            quiz.CategoryID = quizDto.CategoryID;

            // Update existing questions and answers, and add new ones
            foreach (var questionDto in quizDto.Questions)
            {
                var question = quiz.Questions.FirstOrDefault(q => q.QuestionID == questionDto.QuestionID);

                if (question != null)
                {
                    // Update existing question
                    question.QuestionText = questionDto.QuestionText;

                    // Update existing answers and add new ones
                    foreach (var answerDto in questionDto.Answers)
                    {
                        var answer = question.Answers.FirstOrDefault(a => a.AnswerID == answerDto.AnswerID);

                        if (answer != null)
                        {
                            // Update existing answer
                            answer.AnswerText = answerDto.AnswerText;
                            answer.IsCorrect = answerDto.IsCorrect;
                        }
                        else
                        {
                            // Add new answer
                            question.Answers.Add(new Answer
                            {
                                AnswerText = answerDto.AnswerText,
                                IsCorrect = answerDto.IsCorrect
                            });
                        }
                    }

                    // Remove answers that are not in the DTO
                    var answerIdsToRemove = question.Answers
                        .Where(a => !questionDto.Answers.Any(adto => adto.AnswerID == a.AnswerID))
                        .Select(a => a.AnswerID)
                        .ToList();

                    question.Answers = question.Answers.Where(a => !answerIdsToRemove.Contains(a.AnswerID)).ToList();
                }
                else
                {
                    // Add new question with answers
                    quiz.Questions.Add(new Question
                    {
                        QuestionText = questionDto.QuestionText,
                        Answers = questionDto.Answers.Select(a => new Answer
                        {
                            AnswerText = a.AnswerText,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    });
                }
            }

            // Remove questions that are not in the DTO
            var questionIdsToRemove = quiz.Questions
                .Where(q => !quizDto.Questions.Any(qdto => qdto.QuestionID == q.QuestionID))
                .Select(q => q.QuestionID)
                .ToList();

            quiz.Questions = quiz.Questions.Where(q => !questionIdsToRemove.Contains(q.QuestionID)).ToList();

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

        [HttpPost]
        public async Task<ActionResult<Quiz>> CreateQuiz([FromBody] QuizCreateDto quizDto)
        {
            var quiz = new Quiz
            {
                AdminID = quizDto.AdminID,
                Title = quizDto.Title,
                Description = quizDto.Description,
                CategoryID = quizDto.CategoryID,
                Questions = quizDto.Questions.Select(q => new Question
                {
                    QuestionText = q.QuestionText,
                    Answers = q.Answers.Select(a => new Answer
                    {
                        AnswerText = a.AnswerText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                }).ToList()
            };

            _context.QuizList.Add(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuizById", new { quizId = quiz.QuizID }, quiz);
        }

        [HttpGet("{quizId}")]
        public async Task<ActionResult<Quiz>> GetQuizById(int quizId)
        {
            var quiz = await _context.QuizList
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.QuizID == quizId);

            if (quiz == null)
            {
                return NotFound("Quiz not found");
            }

            return Ok(quiz);
        }
    }

    // DTOs
    public class QuizCreateDto
    {
        public int AdminID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public List<QuestionCreateDto> Questions { get; set; }
    }

    public class QuestionCreateDto
    {
        public int QuestionID { get; set; } // Add this line
        public string QuestionText { get; set; }
        public List<AnswerCreateDto> Answers { get; set; }
    }

    public class AnswerCreateDto
    {
        public int AnswerID { get; set; } // Add this line
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}