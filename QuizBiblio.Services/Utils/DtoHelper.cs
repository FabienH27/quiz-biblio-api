using QuizBiblio.Models;
using QuizBiblio.Models.Quiz;

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
            Image = quizDto.Image,
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
    public static QuizDto ToDto(this CreateQuizResponse quiz, QuizCreator user)
    {
        return new QuizDto
        {
            Title = quiz.Title,
            Image = quiz.Image,
            Questions = quiz.Questions,
            Themes = quiz.Themes,
            Creator = user
        };
    }
}
