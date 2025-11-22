using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Services.UserQuizScore;

public interface IUserQuizScoreService
{
    public Task<List<UserScoreWithUserEntity>> GetUserQuizScores(int pageSize);

    public Task<GuestScoreResponse> SaveUserScoreAsync(string userId, IEnumerable<AnswerDto> answers);

    public Task<GuestScoreResponse> SaveUserScoreAsync(string userId, int newScore);
}
