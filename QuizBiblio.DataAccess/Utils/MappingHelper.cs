using MongoDB.Driver;
using QuizBiblio.Models.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.DataAccess.Utils;

public static class MappingHelper
{
    /// <summary>
    /// Projects the query to the Dto object. It is the object exposed in the API
    /// </summary>
    /// <param name="query">query performed</param>
    /// <returns>quiz dto object</returns>
    public static IFindFluent<QuizEntity, QuizDto> ProjectToDto(this IFindFluent<QuizEntity, QuizEntity> query)
    {
        return query.Project(quiz => new QuizDto
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Creator = quiz.Creator,
            Image = quiz.Image,
            Questions = quiz.Questions,
            Themes = quiz.Themes
        });
    }


    /// <summary>
    /// Projects the query to the info object. Gets quiz basics information.
    /// </summary>
    /// <param name="query">query performed</param>
    /// <returns>quiz info object</returns>
    public static IFindFluent<QuizEntity, QuizInfo> ProjectToInfo(this IFindFluent<QuizEntity, QuizEntity> query)
    {
        return query.Project(q => new QuizInfo
        {
            Id = q.Id,
            Title = q.Title,
            CreatorName = q.Creator.Name,
            Image = q.Image,
            Themes = q.Themes,
            QuestionCount = q.Questions.Count
        });
    }
}
