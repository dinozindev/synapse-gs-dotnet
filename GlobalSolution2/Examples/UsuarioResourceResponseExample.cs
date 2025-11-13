using GlobalSolution2.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace GlobalSolution2.Examples;

public class UsuarioResourceResponseExample : IExamplesProvider<ResourceResponse<UsuarioReadDto>>
{
    public ResourceResponse<UsuarioReadDto> GetExamples()
    {
        var usuario = new UsuarioReadDto(
           1, "maria.silva", "senha123", "Suporte Técnico", "DevOps", 
        "Migrar para área de infraestrutura e automação", "Júnior", []
        );

        var links = new List<LinkDto>
        {
            new LinkDto("self", "/usuarios/1", "GET"),
            new LinkDto("update", "/usuarios/1", "PUT"),
            new LinkDto("delete", "/usuarios/1", "DELETE"),
            new LinkDto("list", "/usuarios", "GET")
        };

        return new ResourceResponse<UsuarioReadDto>(usuario, links);
    }
}