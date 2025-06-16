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

    public async Task<GuestScoreResponse> MergeToUserAsync(string guestId, string userId)
    {
        var guestSession = await GetGuestSessionAsync(guestId);

        return await _userQuizScoreService.SaveUserScoreAsync(userId, guestSession.Score);
    }

    public async Task<bool> DeleteGuestSessionAsync(string guestId)
    {
        return await _guestSessionRepository.DeleteGuestSessionAsync(guestId);
    }

    public async Task<bool> SaveGuestAnswersAsync(string guestId, IEnumerable<AnswerDto> answers)
    {
        var score = ScoreHelper.CalculateUserScore(answers);

        return await _guestSessionRepository.SaveGuestScoreAsync(guestId, score);
    }

    public async Task<GuestSession> GetGuestSessionAsync(string guestId)
    {
        return await _guestSessionRepository.GetGuestSessionAsync(guestId);
    }
}
