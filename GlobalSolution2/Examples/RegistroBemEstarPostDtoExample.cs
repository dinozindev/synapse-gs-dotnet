using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RegistroBemEstarPostDtoExample : IExamplesProvider<RegistroBemEstarPostDto>
{
    public RegistroBemEstarPostDto GetExamples()
    {
        return new RegistroBemEstarPostDto(
            DateTime.UtcNow,
            "Feliz",
            9,
            6,
            8,
            4,
            "Finalizei as demandas no trabalho",
            1
        );
    }
}