// Entities/Question.cs
namespace IntroBE.Entities
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int QuizID { get; set; }
        public string QuestionText { get; set; }

        // Navigation properties
        public Quiz Quiz { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}