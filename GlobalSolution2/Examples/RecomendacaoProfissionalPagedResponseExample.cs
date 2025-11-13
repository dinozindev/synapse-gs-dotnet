using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoProfissionalPagedResponseExample : IExamplesProvider<PagedResponse<RecomendacaoProfissionalReadDto>>
{
    public PagedResponse<RecomendacaoProfissionalReadDto> GetExamples()
    {
        var recomendacoes = new List<RecomendacaoProfissionalReadDto>
        {
            new RecomendacaoProfissionalReadDto(1, DateTime.UtcNow, "Vaga Front-end Júnior", "Oportunidade para desenvolvedor front-end iniciante", "Vaga", "Front-end", "LinkedIn", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")),
            new RecomendacaoProfissionalReadDto(2, DateTime.UtcNow, "Curso de Back-end com Spring Boot", "Aprofunde seus conhecimentos em APIs Java", "Curso", "Back-end", "Alura", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")),
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/recomendacoes-profissionais?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/recomendacoes-profissionais?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<RecomendacaoProfissionalReadDto>(
            TotalCount: recomendacoes.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: recomendacoes,
            Links: links
        );
    }
}