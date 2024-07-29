// Entities/Guest.cs
namespace IntroBE.Entities
{
    public class Guest
    {
        public int GuestID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Navigation property
        public ICollection<GuestQuizScore> GuestQuizScores { get; set; }
    }
}