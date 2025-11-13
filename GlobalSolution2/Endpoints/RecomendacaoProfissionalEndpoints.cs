using System.ComponentModel;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

namespace GlobalSolution2.Endpoints;

public static class RecomendacaoProfissionalEndpoints
{
    public static void MapRecomendacaoProfissionalEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var recProfissionais = app.MapGroup("/api/v{version:apiVersion}/recomendacoes/profissional")
            .WithApiVersionSet(versionSet)
            .WithTags("Recomendações Profissionais");

        recProfissionais.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoProfissionalService service) =>
            await service.GetAllRecomendacoesAsync(pageNumber, pageSize))
            .WithSummary("Retorna todas as recomendações profissionais (paginação) (V1)")
            .MapToApiVersion(1, 0)
            .Produces<PagedResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);

        recProfissionais.MapGet("/{id:int}", async ([Description("Identificador único de Recomendação Profissional")] int id, RecomendacaoProfissionalService service) =>
            await service.GetRecomendacaoByIdAsync(id))
            .WithSummary("Retorna recomendação profissional pelo ID (V1)")
            .MapToApiVersion(1, 0)
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        recProfissionais.MapGet("/usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoProfissionalService service) =>
            await service.GetRecomendacoesByUsuarioAsync(usuarioId))
            .WithSummary("Retorna todas as recomendações profissionais de um usuário (V1)")
            .MapToApiVersion(1, 0)
            .Produces<PagedResponse<RecomendacaoProfissionalResumoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);

        recProfissionais.MapPost("/", async (RecomendacaoProfissionalPostDto dto, RecomendacaoProfissionalService service) => await service.CreateRecomendacaoProfissionalAsync(dto))
            .WithSummary("Cria uma recomendação profissional (V1)")
            .WithDescription("Este endpoint é responsável por criar uma nova recomendação profissional. Retorna 201 Created caso seja criada, ou erro caso não seja possível.")
            .MapToApiVersion(1, 0)
            .Accepts<RecomendacaoProfissionalPostDto>("application/json")
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        recProfissionais.MapDelete("/{id:int}", async ([Description("Identificador único de Recomendação")] int id, RecomendacaoProfissionalService service) => await service.DeleteRecomendacaoProfissionalAsync(id))
            .WithSummary("Deleta uma recomendação profissional (V1)")
            .WithDescription("Este endpoint é responsável por remover uma recomendação profissional. Retorna 204 No Content se for removida, ou erro caso não seja possível.")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        recProfissionais.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoProfissionalService service) =>
            await service.GetAllRecomendacoesAsync(pageNumber, pageSize))
            .WithSummary("Retorna todas as recomendações profissionais (paginação) (V2)")
            .MapToApiVersion(2, 0)
            .Produces<PagedResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recProfissionais.MapGet("/{id:int}", async ([Description("Identificador único de Recomendação Profissional")] int id, RecomendacaoProfissionalService service) =>
            await service.GetRecomendacaoByIdAsync(id))
            .WithSummary("Retorna recomendação profissional pelo ID (V2)")
            .MapToApiVersion(2, 0)
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recProfissionais.MapGet("/usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoProfissionalService service) =>
            await service.GetRecomendacoesByUsuarioAsync(usuarioId))
            .WithSummary("Retorna todas as recomendações profissionais de um usuário (V2)")
            .MapToApiVersion(2, 0)
            .Produces<PagedResponse<RecomendacaoProfissionalResumoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recProfissionais.MapPost("/", async (RecomendacaoProfissionalPostDto dto, RecomendacaoProfissionalService service) => await service.CreateRecomendacaoProfissionalAsync(dto))
            .WithSummary("Cria uma recomendação profissional (V2)")
            .WithDescription("Este endpoint é responsável por criar uma nova recomendação profissional. Retorna 201 Created caso seja criada, ou erro caso não seja possível.")
            .MapToApiVersion(2, 0)
            .Accepts<RecomendacaoProfissionalPostDto>("application/json")
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        
        recProfissionais.MapDelete("/{id:int}", async ([Description("Identificador único de Recomendação")] int id, RecomendacaoProfissionalService service) => await service.DeleteRecomendacaoProfissionalAsync(id))
            .WithSummary("Deleta uma recomendação profissional (V2)")
            .WithDescription("Este endpoint é responsável por remover uma recomendação profissional. Retorna 204 No Content se for removida, ou erro caso não seja possível.")
            .MapToApiVersion(2, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}