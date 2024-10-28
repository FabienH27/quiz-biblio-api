namespace QuizBiblio.Services.Quiz;

public interface IQuizService
{
    public Task<List<Models.Quiz>> GetQuizzesAsync();

    public void CreateQuiz(Models.Quiz quiz);
}
