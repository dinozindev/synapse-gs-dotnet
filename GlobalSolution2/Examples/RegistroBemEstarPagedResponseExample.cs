using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RegistroBemEstarPagedResponseExample : IExamplesProvider<PagedResponse<RegistroBemEstarReadDto>>
{
    public PagedResponse<RegistroBemEstarReadDto> GetExamples()
    {
        var registros = new List<RegistroBemEstarReadDto>
        {
            new RegistroBemEstarReadDto(1, DateTime.UtcNow, "Estressado", 6, 10, 5, 8, "Muita demanda no trabalho", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")),
            new RegistroBemEstarReadDto(2, DateTime.UtcNow, "Calmo", 7, 8, 7, 5, "Dia mais tranquilo", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior")),
            new RegistroBemEstarReadDto(3, DateTime.UtcNow, "Feliz", 8, 7, 8, 4, "Finalizei projeto importante", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior"))
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/registros-bem-estar?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/registros-bem-estar?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<RegistroBemEstarReadDto>(
            TotalCount: registros.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: registros,
            Links: links
        );
    }
}