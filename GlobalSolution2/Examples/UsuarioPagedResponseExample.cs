using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class UsuarioPagedResponseExample : IExamplesProvider<PagedResponse<UsuarioReadDto>>
{
    public PagedResponse<UsuarioReadDto> GetExamples()
    {
        var usuarios = new List<UsuarioReadDto>
        {
            new UsuarioReadDto(1, "maria.silva", "senha123", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior", []),
            new UsuarioReadDto(2, "joao.santos", "pass456", "Analista de Sistemas", "Data Science", 
        "Tornar-me cientista de dados especializado em IA", "Pleno", []),
            new UsuarioReadDto(3, "ana.costa", "secure789", "Designer Gráfico", "UX/UI", 
        "Transição para design de experiência do usuário", "Júnior", [])
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/usuarios?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/usuarios?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<UsuarioReadDto>(
            TotalCount: usuarios.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: usuarios,
            Links: links
        );
    }
}