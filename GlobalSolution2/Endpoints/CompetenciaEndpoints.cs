
using System.ComponentModel;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

namespace GlobalSolution2.Endpoints;

public static class CompetenciaEndpoints
{
    public static void MapCompetenciaEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var competencias = app.MapGroup("/api/v{version:apiVersion}/competencias")
        .WithApiVersionSet(versionSet)
        .WithTags("Competências");

        competencias.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, CompetenciaService service) => await service.GetAllCompetenciasAsync(pageNumber, pageSize))
        .WithSummary("Retorna todas as competências cadastradas (paginação) (V1)")
        .WithDescription("Este endpoint retorna a lista de todas as Competências, paginadas de acordo com os parâmetros **pageNumber** e **pageSize**.")
        .MapToApiVersion(1, 0)
        .Produces<PagedResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status500InternalServerError);

        competencias.MapGet("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaService service) => await service.GetCompetenciaByIdAsync(id))
        .WithSummary("Retorna uma Competência pelo ID (V1)")
        .WithDescription("Este endpoint retorna os dados de uma Competência pelo ID. Retorna 200 OK se encontrado, caso não, retorna erro.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        competencias.MapPost("/criar-para-usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, CompetenciaPostDto dto, CompetenciaService service) => await service.CreateCompetenciaParaUsuarioAsync(usuarioId, dto))
        .Accepts<CompetenciaPostDto>("application/json")
        .WithSummary("Cria uma Competência, adicionando ela a um Usuário (V1)")
        .WithDescription("Este endpoint cria uma Competência e em seguida adiciona ela a um Usuário. Retorna 201 Created se a competência for criada com sucesso, ou erro caso não seja.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        competencias.MapPut("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaPostDto dto, CompetenciaService service) => await service.UpdateCompetenciaAsync(id, dto))
        .WithSummary("Atualiza uma Competência (V1)")
        .WithDescription("Este endpoint atualiza os dados de uma Competência. Retorna 200 OK se atualizado com sucesso, caso não, retorna erro.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        competencias.MapDelete("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaService service) => await service.DeleteCompetenciaAsync(id))
        .WithSummary("Remove uma Competência (V1)")
        .WithDescription("Este endpoint remove uma Competência. Retorna 204 No Content se removida com sucesso, ou erro caso não seja encontrada.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
        
        competencias.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, CompetenciaService service) => await service.GetAllCompetenciasAsync(pageNumber, pageSize))
        .WithSummary("Retorna todas as competências cadastradas (paginação) (V2)")
        .WithDescription("Este endpoint retorna a lista de todas as Competências, paginadas de acordo com os parâmetros **pageNumber** e **pageSize**.")
        .MapToApiVersion(2, 0)
        .Produces<PagedResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        competencias.MapGet("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaService service) => await service.GetCompetenciaByIdAsync(id))
        .WithSummary("Retorna uma Competência pelo ID (V2)")
        .WithDescription("Este endpoint retorna os dados de uma Competência pelo ID. Retorna 200 OK se encontrado, caso não, retorna erro.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        competencias.MapPost("/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, CompetenciaPostDto dto, CompetenciaService service) => await service.CreateCompetenciaParaUsuarioAsync(usuarioId, dto))
        .Accepts<CompetenciaPostDto>("application/json")
        .WithSummary("Cria uma Competência, adicionando ela a um Usuário (V2)")
        .WithDescription("Este endpoint cria uma Competência e em seguida adiciona ela a um Usuário. Retorna 201 Created se a competência for criada com sucesso, ou erro caso não seja.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
        
        competencias.MapPut("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaPostDto dto, CompetenciaService service) => await service.UpdateCompetenciaAsync(id, dto))
        .WithSummary("Atualiza uma Competência (V2)")
        .WithDescription("Este endpoint atualiza os dados de uma Competência. Retorna 200 OK se atualizado com sucesso, caso não, retorna erro.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<CompetenciaReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        competencias.MapDelete("/{id:int}", async ([Description("Identificador único de Competência")] int id, CompetenciaService service) => await service.DeleteCompetenciaAsync(id))
        .WithSummary("Remove uma Competência (V2)")
        .WithDescription("Este endpoint remove uma Competência. Retorna 204 No Content se removida com sucesso, ou erro caso não seja encontrada.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
    }
}