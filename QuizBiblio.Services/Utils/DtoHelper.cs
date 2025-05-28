using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Requests;

namespace QuizBiblio.Services.Utils;

public static class DtoHelper
{
    /// <summary>
    /// Converts a dto object to an entity
    /// </summary>
    /// <param name="quizDto">dto object to convert</param>
    /// <returns></returns>
    public static QuizEntity ToEntity(this QuizDto quizDto)
    {
        return new QuizEntity
        {
            Id = quizDto.Id,
            Title = quizDto.Title,
            ImageId = quizDto.ImageId,
            Questions = quizDto.Questions,
            Themes = quizDto.Themes,
            Creator = quizDto.Creator
        };
    }

    /// <summary>
    /// Converts a quiz response object to a dto object
    /// </summary>
    /// <param name="quiz">quiz from the front-end</param>
    /// <param name="user">quiz creator</param>
    /// <returns></returns>
    public static QuizDto ToDto(this CreateQuizRequest quiz, QuizCreator user)
    {
        return new QuizDto
        {
            Title = quiz.Title,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes,
            Creator = user
        };
    }
}
