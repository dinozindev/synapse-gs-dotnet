using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class CompetenciaPagedResponseExample : IExamplesProvider<PagedResponse<CompetenciaReadDto>>
{
    public PagedResponse<CompetenciaReadDto> GetExamples()
    {
        var competencias = new List<CompetenciaReadDto>
        {
            new CompetenciaReadDto(1, "Python", "Back-end", "Linguagem versátil para desenvolvimento e ciência de dados"),
            new CompetenciaReadDto(2, "JavaScript", "Front-end", "Linguagem essencial para desenvolvimento web"),
            new CompetenciaReadDto(3, "React", "Front-end", "Biblioteca moderna para interfaces de usuário")
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/competencias?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/competencias?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<CompetenciaReadDto>(
            TotalCount: competencias.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: competencias,
            Links: links
        );
    }
}