namespace QuizBiblio.Models.UserQuizScore;

public class UserScoreWithUserEntity : UserQuizScoreEntity
{
    public required string UserName { get; set; }

    public required string Role { get; set; }
}
