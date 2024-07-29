// Entities/Answer.cs
namespace IntroBE.Entities
{
    public class Answer
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        // Navigation property
        public Question Question { get; set; }
    }
}