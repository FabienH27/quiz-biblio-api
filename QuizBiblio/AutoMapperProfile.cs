using AutoMapper;
using QuizBiblio.Models.UserQuizScore;
using QuizBiblio.Models.Users;

namespace QuizBiblio;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AnswerRequest, AnswerDto>();
        CreateMap<QuizAnswersRequest, QuizAnswerDto>();
        CreateMap<UserEntity, User>();
        CreateMap<User, UserDto>();
    }
}
