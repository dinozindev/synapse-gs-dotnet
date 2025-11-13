using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class CompetenciaResourceResponseExample : IExamplesProvider<ResourceResponse<CompetenciaReadDto>>
{
    public ResourceResponse<CompetenciaReadDto> GetExamples()
    {
        var competencia = new CompetenciaReadDto(
            1, "Python", "Back-end", "Linguagem versátil para desenvolvimento e ciência de dados"
        );

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/competencias/1", "GET"),
            new LinkDto("update", "/competencias/1", "PUT"),
            new LinkDto("delete", "/competencias/1", "DELETE"),
            new LinkDto("list", "/competencias", "GET")
        };

        return new ResourceResponse<CompetenciaReadDto>(competencia, links);
    }
}