using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoProfissionalResourceResponseExample : IExamplesProvider<ResourceResponse<RecomendacaoProfissionalReadDto>>
{
    public ResourceResponse<RecomendacaoProfissionalReadDto> GetExamples()
    {
        var recomendacao = new RecomendacaoProfissionalReadDto(
            1, DateTime.UtcNow, "Vaga Front-end Júnior", "Oportunidade para desenvolvedor front-end iniciante", "Vaga", "Front-end", "LinkedIn", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")
        );

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/recomendacoes-profissionais/1", "GET"),
            new LinkDto("update", "/recomendacoes-profissionais/1", "PUT"),
            new LinkDto("delete", "/recomendacoes-profissionais/1", "DELETE"),
            new LinkDto("list", "/recomendacoes-profissionais", "GET")
        };

        return new ResourceResponse<RecomendacaoProfissionalReadDto>(recomendacao, links);
    }
}