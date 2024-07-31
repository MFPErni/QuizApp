// Entities/Category.cs
namespace IntroBE.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Title { get; set; }

        // Navigation property
        public ICollection<Quiz> Quizzes { get; set; }
    }
}