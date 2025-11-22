using MongoDB.Driver;
using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Users;

namespace QuizBiblio.DataAccess.Utils;

public static class MappingHelper
{
    /// <summary>
    /// Projects the query to the Dto object. It is the object exposed in the API
    /// </summary>
    /// <param name="query">performed query</param>
    /// <returns>quiz dto object</returns>
    public static IFindFluent<QuizEntity, QuizDto> ProjectToDto(this IFindFluent<QuizEntity, QuizEntity> query)
    {
        return query.Project(quiz => new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Creator = quiz.Creator,
            ImageId = quiz.ImageId,
            Questions = quiz.Questions,
            Themes = quiz.Themes
        });
    }


    /// <summary>
    /// Projects the query to the info object. Gets quiz basics information.
    /// </summary>
    /// <param name="query">performed query</param>
    /// <returns>quiz info object</returns>
    public static IFindFluent<QuizEntity, QuizInfo> ProjectToInfo(this IFindFluent<QuizEntity, QuizEntity> query)
    {
        return query.Project(q => new QuizInfo
        {
            Id = q.Id,
            Title = q.Title,
            CreatorName = q.Creator.Name,
            ImageId = q.ImageId,
            Themes = q.Themes,
            QuestionCount = q.Questions.Count()
        });
    }
}
