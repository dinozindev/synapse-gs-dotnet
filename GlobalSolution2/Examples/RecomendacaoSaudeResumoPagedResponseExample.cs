using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RecomendacaoSaudeResumoPagedResponseExample : IExamplesProvider<PagedResponse<RecomendacaoSaudeResumoDto>>
{
    public PagedResponse<RecomendacaoSaudeResumoDto> GetExamples()
    {
        var recomendacoes = new List<RecomendacaoSaudeResumoDto>
        {
            new RecomendacaoSaudeResumoDto(
           1, DateTime.UtcNow, "Melhorar qualidade do sono", "Evite cafeína e telas antes de dormir", "Sono", "Moderado", "Estabeleça rotina de sono consistente"),
            new RecomendacaoSaudeResumoDto(2, DateTime.UtcNow, "Aumentar produtividade", "Organize tarefas com pausas regulares", "Produtividade", "Baixo", "Utilize a técnica Pomodoro para melhor desempenho"),
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/recomendacoes-saude?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/recomendacoes-saude?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<RecomendacaoSaudeResumoDto>(
            TotalCount: recomendacoes.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: recomendacoes,
            Links: links
        );
    }
}