// Entities/Quiz.cs
namespace IntroBE.Entities
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public int AdminID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        // Navigation properties
        public Admin Admin { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<GuestQuizScore> GuestQuizScores { get; set; }
    }
}