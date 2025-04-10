using QuizBiblio.DataAccess.Guest;
using QuizBiblio.Models.Guest;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Services.UserQuizScore;
using QuizBiblio.Services.Utils;

namespace QuizBiblio.Services.Guest;

public class GuestSessionService : IGuestSessionService
{

    private readonly IGuestSessionRepository _guestSessionRepository;
    private readonly IUserQuizScoreService _userQuizScoreService;

    public GuestSessionService(IGuestSessionRepository guestSessionRepository, IUserQuizScoreService userQuizScoreService)
    {
        _guestSessionRepository = guestSessionRepository;
        _userQuizScoreService = userQuizScoreService;
    }

    public async Task<string> StartGuestSessionAsync(string userName)
    {
        return await _guestSessionRepository.StartGuestSessionAsync(userName);
    }

    public async Task<IEnumerable<AnswerDto>> MergeToUserAsync(string guestId, string userId)
    {
        var guestSession = await GetGuestSessionAsync(guestId);

        var guestScore = ScoreHelper.CalculateUserScore(guestSession.Answers?.Answers ?? []);

        await _userQuizScoreService.SaveUserScoreAsync(userId, guestScore);

        return guestSession.Answers?.Answers ?? [];
    }

    public async Task<bool> DeleteGuestSessionAsync(string guestId)
    {
        return await _guestSessionRepository.DeleteGuestSessionAsync(guestId);
    }

    public async Task<bool> SaveUserAnswersAsync(string guestId, QuizAnswerDto answerDto)
    {
        return await _guestSessionRepository.SaveUserAnswersAsync(guestId, answerDto);
    }

    public async Task<GuestSession> GetGuestSessionAsync(string guestId)
    {
        return await _guestSessionRepository.GetGuestSessionAsync(guestId);
    }
}
