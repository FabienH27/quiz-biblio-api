using AutoMapper;
using QuizBiblio.Models.UserQuizScore;

namespace QuizBiblio;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AnswerRequest, AnswerDto>();
        CreateMap<QuizAnswersRequest, QuizAnswerDto>();
    }
}
