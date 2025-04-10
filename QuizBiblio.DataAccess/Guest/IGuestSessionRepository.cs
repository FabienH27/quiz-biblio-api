using QuizBiblio.Models.Guest;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.DataAccess.Guest;

public interface IGuestSessionRepository
{
    public Task<string> StartGuestSessionAsync(string userName);

    public Task<bool> DeleteGuestSessionAsync(string guestId);

    public Task<bool> SaveUserAnswersAsync(string guestId, QuizAnswerDto answerDtos);

    public Task<GuestSession> GetGuestSessionAsync(string guestId);
}
