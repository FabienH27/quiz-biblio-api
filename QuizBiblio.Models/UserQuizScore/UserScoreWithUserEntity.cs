
using QuizBiblio.Models.UserQuiz;
using System.Runtime.CompilerServices;

namespace QuizBiblio.Models.UserQuizScore;

public class UserScoreWithUserEntity : UserQuizScoreEntity
{
    public required string UserName { get; set; }
}
