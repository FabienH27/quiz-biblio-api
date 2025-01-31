using AutoMapper;
using MongoDB.Bson;
using QuizBiblio.Models.Quiz;

namespace QuizBiblio.DataAccess;

public class MappingProfile : Profile
{
    public MappingProfile() {

        CreateMap<QuizEntity, QuizDto>();
            //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        //.ReverseMap()
        //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => new ObjectId(src.Id)));
    }
}
