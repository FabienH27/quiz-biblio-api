namespace QuizBiblio.Services.Quiz;

public interface IQuizService
{
    public Task<List<Models.Quiz.Quiz>> GetQuizzesAsync();
}
