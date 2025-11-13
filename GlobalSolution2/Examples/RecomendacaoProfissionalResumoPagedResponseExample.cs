using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoProfissionalResumoPagedResponseExample : IExamplesProvider<PagedResponse<RecomendacaoProfissionalResumoDto>>
{
    public PagedResponse<RecomendacaoProfissionalResumoDto> GetExamples()
    {
        var recomendacoes = new List<RecomendacaoProfissionalResumoDto>
        {
            new RecomendacaoProfissionalResumoDto(1, DateTime.UtcNow, "Vaga Front-end Júnior", "Oportunidade para desenvolvedor front-end iniciante", "Vaga", "Front-end", "LinkedIn"),
            new RecomendacaoProfissionalResumoDto(2, DateTime.UtcNow, "Curso de Back-end com Spring Boot", "Aprofunde seus conhecimentos em APIs Java", "Curso", "Back-end", "Alura"),
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/recomendacoes-profissionais?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/recomendacoes-profissionais?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<RecomendacaoProfissionalResumoDto>(
            TotalCount: recomendacoes.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: recomendacoes,
            Links: links
        );
    }
}