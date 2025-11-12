using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Resposta paginada de uma coleção de recursos")]
public record PagedResponse<T>(
    [property : Description("Total de registros disponíveis")]
    int TotalCount,
    [property : Description("Número da página atual")]
    int PageNumber,
    [property : Description("Tamanho da página")]
    int PageSize,
    [property : Description("Total de páginas disponíveis")]
    int TotalPages,
    [property : Description("Lista de recursos da paginação")]
    IEnumerable<T> Data,
    [property : Description("Links HATEOAS relacionados à paginação")]
    IEnumerable<LinkDto> Links
    );