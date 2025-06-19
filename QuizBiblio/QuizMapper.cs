using QuizBiblio.Models.Quiz;
using QuizBiblio.Models.Quiz.Requests;
using Riok.Mapperly.Abstractions;

namespace QuizBiblio;

[Mapper]
public partial class QuizMapper
{
    public partial QuizDto Map(CreateQuizRequest request, QuizCreator creator);
}