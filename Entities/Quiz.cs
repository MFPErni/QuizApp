// Entities/Quiz.cs
namespace IntroBE.Entities
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public int AdminID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; } // Change from string to int

        // Navigation properties
        public Admin Admin { get; set; }
        public Category Category { get; set; } // Add this line
        public ICollection<Question> Questions { get; set; }
    }
}