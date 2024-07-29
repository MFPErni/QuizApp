// Entities/Admin.cs
namespace IntroBE.Entities
{
    public class Admin
    {
        public int AdminID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation property
        public ICollection<Quiz> Quizzes { get; set; }
    }
}