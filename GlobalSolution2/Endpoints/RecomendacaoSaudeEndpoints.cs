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
            .WithDescription("Este endpoint retorna todas as recomendações de saúde presentes no sistema. Retorna 200 OK se encontrado, ou 204 No Content caso não haja nenhuma.")
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
            .Produces<PagedResponse<RecomendacaoSaudeResumoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError);

        recSaude.MapPost("/", async (RecomendacaoSaudePostDto dto, RecomendacaoSaudeService service) => await service.CreateRecomendacaoSaudeAsync(dto))
            .WithSummary("Cria uma recomendação de saúde (V1)")
            .WithDescription("Este endpoint é responsável por criar uma nova recomendação de saúde. Retorna 201 Created caso seja criada, ou erro caso não seja possível.")
            .MapToApiVersion(1, 0)
            .Accepts<RecomendacaoSaudePostDto>("application/json")
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        recSaude.MapDelete("/{id:int}", async ([Description("Identificador único de Recomendação")] int id, RecomendacaoSaudeService service) => await service.DeleteRecomendacaoSaudeAsync(id))
            .WithSummary("Deleta uma recomendação de saúde (V1)")
            .WithDescription("Este endpoint é responsável por remover uma recomendação de saúde. Retorna 204 No Content se for removida, ou erro caso não seja possível.")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        recSaude.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RecomendacaoSaudeService service) =>
            await service.GetAllRecomendacoesAsync(pageNumber, pageSize))
            .WithSummary("Retorna todas as recomendações de saúde (paginação) (V2)")
            .WithDescription("Este endpoint retorna todas as recomendações de saúde presentes no sistema. Retorna 200 OK se encontrado, ou 204 No Content caso não haja nenhuma.")
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
            .Produces<PagedResponse<RecomendacaoSaudeResumoDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        recSaude.MapPost("/", async (RecomendacaoSaudePostDto dto, RecomendacaoSaudeService service) => await service.CreateRecomendacaoSaudeAsync(dto))
            .WithSummary("Cria uma recomendação de saúde (V2)")
            .WithDescription("Este endpoint é responsável por criar uma nova recomendação de saúde. Retorna 201 Created caso seja criada, ou erro caso não seja possível.")
            .MapToApiVersion(2, 0)
            .Accepts<RecomendacaoSaudePostDto>("application/json")
            .Produces<ResourceResponse<RecomendacaoSaudeReadDto>>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
        
        recSaude.MapDelete("/{id:int}", async ([Description("Identificador único de Recomendação")] int id, RecomendacaoSaudeService service) => await service.DeleteRecomendacaoSaudeAsync(id))
            .WithSummary("Deleta uma recomendação de saúde (V2)")
            .WithDescription("Este endpoint é responsável por remover uma recomendação de saúde. Retorna 204 No Content se for removida, ou erro caso não seja possível.")
            .MapToApiVersion(2, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}