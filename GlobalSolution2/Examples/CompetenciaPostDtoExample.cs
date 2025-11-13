using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class CompetenciaPostDtoExample : IExamplesProvider<CompetenciaPostDto>
{
    public CompetenciaPostDto GetExamples()
    {
        return new CompetenciaPostDto(
            "Flutter",
            "Front-end",
            "Kit de desenvolvimento de software de interface de usuário"
        );
    }
}