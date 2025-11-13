using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoSaudeResourceResponseExample : IExamplesProvider<ResourceResponse<RecomendacaoSaudeReadDto>>
{
    public ResourceResponse<RecomendacaoSaudeReadDto> GetExamples()
    {
        var recomendacao = new RecomendacaoSaudeReadDto(
           1, DateTime.UtcNow, "Melhorar qualidade do sono", "Evite cafeína e telas antes de dormir", "Sono", "Moderado", "Estabeleça rotina de sono consistente", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")
        );

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/recomendacoes/saude/1", "GET"),
            new LinkDto("delete", "/recomendacoes/saude/1", "DELETE"),
            new LinkDto("list", "/recomendacoes/saude", "GET")
        };

        return new ResourceResponse<RecomendacaoSaudeReadDto>(recomendacao, links);
    }
}