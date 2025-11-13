using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RegistroBemEstarResumoPagedResponseExample : IExamplesProvider<PagedResponse<RegistroBemEstarResumoDto>>
{
    public PagedResponse<RegistroBemEstarResumoDto> GetExamples()
    {
        var registros = new List<RegistroBemEstarResumoDto>
        {
            new RegistroBemEstarResumoDto(1, DateTime.UtcNow, "Estressado", 6, 10, 5, 8, "Muita demanda no trabalho"),
            new RegistroBemEstarResumoDto(2, DateTime.UtcNow, "Calmo", 7, 8, 7, 5, "Dia mais tranquilo"),
            new RegistroBemEstarResumoDto(3, DateTime.UtcNow, "Feliz", 8, 7, 8, 4, "Finalizei projeto importante")
        };

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/registros-bem-estar?pageNumber=1&pageSize=10", "GET"),
            new LinkDto("next", "/registros-bem-estar?pageNumber=2&pageSize=10", "GET"),
            new LinkDto("prev", "", "GET")
        };

        return new PagedResponse<RegistroBemEstarResumoDto>(
            TotalCount: registros.Count,
            PageNumber: 1,
            PageSize: 10,
            TotalPages: 1,
            Data: registros,
            Links: links
        );
    }
}