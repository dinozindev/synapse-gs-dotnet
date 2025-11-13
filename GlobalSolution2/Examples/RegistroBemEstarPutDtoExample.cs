using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RegistroBemEstarPutDtoExample : IExamplesProvider<RegistroBemEstarPutDto>
{
    public RegistroBemEstarPutDto GetExamples()
    {
        return new RegistroBemEstarPutDto(
            "Bravo",
            5,
            9,
            5,
            9,
            "Extremamente incomodado com certos comportamentos no trabalho"
        );
    }
}