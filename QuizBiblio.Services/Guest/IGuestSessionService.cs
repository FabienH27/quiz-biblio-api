using QuizBiblio.Models.Guest;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.Services.Guest;

public interface IGuestSessionService
{
    public Task<string> StartGuestSessionAsync(string userName);

    public Task<IEnumerable<AnswerDto>> MergeToUserAsync(string guestId, string userId);

    public Task<bool> DeleteGuestSessionAsync(string guestId);

    public Task<bool> SaveUserAnswersAsync(string guestId, QuizAnswerDto answerDtos);
}
