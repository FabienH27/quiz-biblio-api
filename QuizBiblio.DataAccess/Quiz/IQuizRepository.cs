namespace QuizBiblio.DataAccess.Quiz;

public interface IQuizRepository
{
    public Task<List<Models.Quiz>> GetQuizzesAsync();
}
