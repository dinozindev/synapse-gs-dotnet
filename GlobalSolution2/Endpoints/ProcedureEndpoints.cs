using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

public static class ProcedureEndpoints
{
    public static void MapProcedureEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var procedures = app.MapGroup("/api/v{version:apiVersion}/procedures")
                            .WithApiVersionSet(versionSet)
                            .WithTags("Procedures (Somente para disciplina de Database)");

        procedures.MapPost("/usuarios", async (UsuarioPostDto dto, ProcedureService service) =>
            await service.InserirUsuarioAsync(dto))
        .Accepts<UsuarioPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_usuario (V1)")
        .WithDescription("Cria um novo usuário usando a procedure Oracle sp_inserir_usuario.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/usuarios/{usuarioId:int}/competencias/{competenciaId:int}", async (
            int usuarioId, int competenciaId, ProcedureService service) =>
            await service.InserirUsuarioCompetenciaAsync(usuarioId, competenciaId))
        .WithSummary("Chama a procedure sp_inserir_usuario_competencia (V1)")
        .WithDescription("Associa uma competência a um usuário usando a procedure Oracle sp_inserir_usuario_competencia.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/competencias", async (CompetenciaPostDto dto, ProcedureService service) =>
            await service.InserirCompetenciaAsync(dto))
        .Accepts<CompetenciaPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_competencia (V1)")
        .WithDescription("Cria uma nova competência usando a procedure Oracle sp_inserir_competencia.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/bem-estar", async (RegistroBemEstarPostDto dto, ProcedureService service) =>
            await service.InserirRegistroBemEstarAsync(dto))
        .Accepts<RegistroBemEstarPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_registro_bem_estar (V1)")
        .WithDescription("Cria um registro de bem-estar usando a procedure Oracle sp_inserir_registro_bem_estar.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/recomendacoes/profissional", async (RecomendacaoProfissionalPostDto dto, ProcedureService service) =>
            await service.CriarRecomendacaoProfissionalAsync(dto))
        .Accepts<RecomendacaoProfissionalPostDto>("application/json")
        .WithSummary("Chama a procedure sp_criar_recomendacao_profissional_completa (V1)")
        .WithDescription("Cria uma recomendação profissional completa usando a procedure Oracle sp_criar_recomendacao_profissional_completa.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/recomendacoes/saude", async (RecomendacaoSaudePostDto dto, ProcedureService service) =>
            await service.CriarRecomendacaoSaudeAsync(dto))
        .Accepts<RecomendacaoSaudePostDto>("application/json")
        .WithSummary("Chama a procedure sp_criar_recomendacao_saude_completa (V1)")
        .WithDescription("Cria uma recomendação de saúde completa usando a procedure Oracle sp_criar_recomendacao_saude_completa.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapGet("/exportar/usuarios", async (ProcedureService service) =>
            {
                var arquivo = await service.ExportarDatasetUsuariosAsync();
                return Results.Ok(arquivo);
            })
        .WithSummary("Chama a procedure sp_exportar_dataset_usuarios (V1)")
        .WithDescription("Exporta o dataset de usuários em JSON usando a procedure Oracle sp_exportar_dataset_usuarios.")
        .MapToApiVersion(1, 0)
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status500InternalServerError);

        procedures.MapPost("/usuarios", async (UsuarioPostDto dto, ProcedureService service) =>
            await service.InserirUsuarioAsync(dto))
        .Accepts<UsuarioPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_usuario (V2)")
        .WithDescription("Cria um novo usuário usando a procedure Oracle sp_inserir_usuario.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapPost("/usuarios/{usuarioId:int}/competencias/{competenciaId:int}", async (
            int usuarioId, int competenciaId, ProcedureService service) =>
            await service.InserirUsuarioCompetenciaAsync(usuarioId, competenciaId))
        .WithSummary("Chama a procedure sp_inserir_usuario_competencia (V2)")
        .WithDescription("Associa uma competência a um usuário usando a procedure Oracle sp_inserir_usuario_competencia.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapPost("/competencias", async (CompetenciaPostDto dto, ProcedureService service) =>
            await service.InserirCompetenciaAsync(dto))
        .Accepts<CompetenciaPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_competencia (V2)")
        .WithDescription("Cria uma nova competência usando a procedure Oracle sp_inserir_competencia.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapPost("/bem-estar", async (RegistroBemEstarPostDto dto, ProcedureService service) =>
            await service.InserirRegistroBemEstarAsync(dto))
        .Accepts<RegistroBemEstarPostDto>("application/json")
        .WithSummary("Chama a procedure sp_inserir_registro_bem_estar (V2)")
        .WithDescription("Cria um registro de bem-estar usando a procedure Oracle sp_inserir_registro_bem_estar.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapPost("/recomendacoes/profissional", async (RecomendacaoProfissionalPostDto dto, ProcedureService service) =>
            await service.CriarRecomendacaoProfissionalAsync(dto))
        .Accepts<RecomendacaoProfissionalPostDto>("application/json")
        .WithSummary("Chama a procedure sp_criar_recomendacao_profissional_completa (V2)")
        .WithDescription("Cria uma recomendação profissional completa usando a procedure Oracle sp_criar_recomendacao_profissional_completa.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapPost("/recomendacoes/saude", async (RecomendacaoSaudePostDto dto, ProcedureService service) =>
            await service.CriarRecomendacaoSaudeAsync(dto))
        .Accepts<RecomendacaoSaudePostDto>("application/json")
        .WithSummary("Chama a procedure sp_criar_recomendacao_saude_completa (V2)")
        .WithDescription("Cria uma recomendação de saúde completa usando a procedure Oracle sp_criar_recomendacao_saude_completa.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        procedures.MapGet("/exportar/usuarios", async (ProcedureService service) =>
            {
                var arquivo = await service.ExportarDatasetUsuariosAsync();
                return Results.Ok(arquivo);
            })
        .WithSummary("Chama a procedure sp_exportar_dataset_usuarios (V2)")
        .WithDescription("Exporta o dataset de usuários em JSON usando a procedure Oracle sp_exportar_dataset_usuarios.")
        .MapToApiVersion(2, 0)
        .Produces<string>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
    }
}