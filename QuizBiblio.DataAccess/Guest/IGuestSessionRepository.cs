using QuizBiblio.Models.Guest;

namespace QuizBiblio.DataAccess.Guest;

public interface IGuestSessionRepository
{
    public Task<string> StartGuestSessionAsync(string userName);

    public Task<bool> DeleteGuestSessionAsync(string guestId);

    public Task<bool> SaveGuestScoreAsync(string guestId, int score);

    public Task<GuestSession> GetGuestSessionAsync(string guestId);
}
