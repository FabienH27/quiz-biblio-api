using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Models.Guest;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio.DataAccess.Guest;

public class GuestSessionRepository : IGuestSessionRepository
{
    private readonly IMongoDbContext _dbContext;

    IMongoCollection<GuestSession> GuestSession => _dbContext.GetCollection<GuestSession>("GuestSession");

    public GuestSessionRepository(IMongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<string> StartGuestSessionAsync(string userName)
    {
        var session = new GuestSession(userName);

        await GuestSession.InsertOneAsync(session);

        return session.Id;
    }

    public async Task<bool> DeleteGuestSessionAsync(string guestId)
    {
        var filter = Builders<GuestSession>.Filter.Eq(s => s.Id, guestId);

        var deleteRequest = await GuestSession.DeleteOneAsync(filter);

        return deleteRequest.IsAcknowledged;
    }

    public async Task<bool> SaveGuestScoreAsync(string guestId, int userScore)
    {
        var filter = Builders<GuestSession>.Filter.Eq(s => s.Id, guestId);

        var update = Builders<GuestSession>.Update
            .Set(s => s.Score, userScore);

        var updateRequest = await GuestSession.UpdateOneAsync(filter, update);

        return updateRequest.ModifiedCount == 1;
    }

    public async Task<GuestSession> GetGuestSessionAsync(string guestId)
    {
        var filter = Builders<GuestSession>.Filter.Eq(s => s.Id, guestId);

        return await GuestSession.Find(filter).FirstOrDefaultAsync();
    }
}