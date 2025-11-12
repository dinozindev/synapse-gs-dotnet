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
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
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
            .Produces<ResourceResponse<RecomendacaoProfissionalReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}