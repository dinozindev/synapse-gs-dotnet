using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class RegistroBemEstarResourceResponseExample : IExamplesProvider<ResourceResponse<RegistroBemEstarReadDto>>
{
    public ResourceResponse<RegistroBemEstarReadDto> GetExamples()
    {
        var registro = new RegistroBemEstarReadDto(1, DateTime.UtcNow, "Estressado", 6, 10, 5, 8, "Muita demanda no trabalho", new UsuarioResumoDto(1, "maria.silva", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior"));

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/registros-bem-estar/1", "GET"),
            new LinkDto("update", "/registros-bem-estar/1", "PUT"),
            new LinkDto("delete", "/registros-bem-estar/1", "DELETE"),
            new LinkDto("list", "/registros-bem-estar", "GET")
        };

        return new ResourceResponse<RegistroBemEstarReadDto>(registro, links);
    }
}