// Entities/GuestQuizScore.cs
namespace IntroBE.Entities
{
    public class GuestQuizScore
    {
        public int GuestQuizScoreID { get; set; }
        public int GuestID { get; set; }
        public int QuizID { get; set; }
        public int Score { get; set; }

        // Navigation properties
        public Guest Guest { get; set; }
        public Quiz Quiz { get; set; }
    }
}