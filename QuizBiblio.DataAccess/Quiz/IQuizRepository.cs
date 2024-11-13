namespace QuizBiblio.DataAccess.Quiz;

public interface IQuizRepository
{
    public Task<List<Models.Quiz>> GetQuizzesAsync();

    public void CreateQuiz(Models.Quiz quiz);

    public void UpdateQuiz(Models.Quiz quiz);
}
