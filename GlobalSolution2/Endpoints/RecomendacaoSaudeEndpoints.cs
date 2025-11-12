using System.ComponentModel;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

public static class RecomendacaoSaudeEndpoints
{
    public static void MapRecomendacaoSaudeEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var recSaude = app.MapGroup("/api/v{version:apiVersion}/recomendacoes/saude")
            .WithApiVersionSet(versionSet)
            .WithTags("Recomendações de Saúde");

        recSaude.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoSaudeService service) =>
            await service.GetAllRecomendacoesAsync(pageNumber, pageSize))
            .WithSummary("Retorna todas as recomendações de saúde (paginação) (V1)")
            .MapToApiVersion(1, 0)
            .Produces<PagedResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);

        recSaude.MapGet("/{id:int}", async ([Description("Identificador único de Recomendação Saúde")] int id, RecomendacaoSaudeService service) =>
            await service.GetRecomendacaoByIdAsync(id))
            .WithSummary("Retorna recomendação de saúde pelo ID (V1)")
            .MapToApiVersion(1, 0)
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        recSaude.MapGet("/usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoSaudeService service) =>
            await service.GetRecomendacoesByUsuarioAsync(usuarioId))
            .WithSummary("Retorna todas as recomendações de saúde de um usuário (V1)")
            .MapToApiVersion(1, 0)
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);
        
        recSaude.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoSaudeService service) =>
            await service.GetAllRecomendacoesAsync(pageNumber, pageSize))
            .WithSummary("Retorna todas as recomendações de saúde (paginação) (V2)")
            .MapToApiVersion(2, 0)
            .Produces<PagedResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recSaude.MapGet("/{id:int}", async ([Description("Identificador único de Recomendação Saúde")] int id, RecomendacaoSaudeService service) =>
            await service.GetRecomendacaoByIdAsync(id))
            .WithSummary("Retorna recomendação de saúde pelo ID (V2)")
            .MapToApiVersion(2, 0)
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recSaude.MapGet("/usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoSaudeService service) =>
            await service.GetRecomendacoesByUsuarioAsync(usuarioId))
            .WithSummary("Retorna todas as recomendações de saúde de um usuário (V2)")
            .MapToApiVersion(2, 0)
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}