
using System.ComponentModel;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

namespace GlobalSolution2.Endpoints;

public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var usuarios = app.MapGroup("/api/v{version:apiVersion}/usuarios")
            .WithApiVersionSet(versionSet)
            .WithTags("Usuarios");

        usuarios.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, UsuarioService service) => await service.GetAllUsuariosAsync(pageNumber, pageSize))
        .WithSummary("Retorna todos os usuários cadastrados (paginação) (V1)")
        .WithDescription("Este endpoint retorna a lista de todos os usuários, paginados de acordo com os parâmetros **pageNumber** e **pageSize**, incluindo também as competências associadas a cada usuário (se existir).")
        .MapToApiVersion(1, 0)
        .Produces<PagedResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapGet("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioService service) => await service.GetUsuarioByIdAsync(id))
        .WithSummary("Retorna um usuário pelo ID (V1)")
        .WithDescription("Este endpoint retorna os dados de um usuário pelo ID, incluindo também as competências associadas a ele (se existir). Retorna 200 OK se o usuário for encontrado, ou 404 Not Found se não for achado.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapPost("/", async (UsuarioPostDto dto, UsuarioService service) => await service.CreateUsuarioAsync(dto))
        .Accepts<UsuarioPostDto>("application/json")
        .WithSummary("Cria um Usuário (V1)")
        .WithDescription("Este endpoint cria um usuário a partir do nome, senha, área atual, área de interesse, objetivo de carreira e nível de experiência. Retorna 201 Created se o usuário for criado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapPut("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioPostDto dto, UsuarioService service) => await service.UpdateUsuarioAsync(id, dto))
            .WithSummary("Atualiza um Usuário (V1)")
            .WithDescription("Este endpoint é responsável por atualizar os dados existentes de um Usuário a partir de seu ID. Retorna 200 OK se o Usuário for atualizado com sucesso, ou erro caso não seja possível.")
            .MapToApiVersion(1, 0)
            .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapDelete("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioService service) => await service.DeleteUsuarioAsync(id))
            .WithSummary("Deleta um usuário pelo ID (V1)")
            .WithDescription("Este endpoint é responsável por deletar um Usuário pelo ID informado. Retorna 204 No Content se o Usuário for deletado com sucesso, ou erro se não for achado.")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapPost("/{usuarioId:int}/adicionar-competencia/{competenciaId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("Identificador único de Competência")] int competenciaId, UsuarioService service) => await service.AddCompetenciaAoUsuarioAsync(usuarioId, competenciaId))
            .WithSummary("Adiciona uma competência ao Usuário (V1)")
            .WithDescription("Este endpoint é responsável por adicionar uma associação de Competência a um Usuário. Retorna 204 No Content se possível, ou erro se não for possível.")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapDelete("/{usuarioId:int}/remover-competencia/{competenciaId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("Identificador único de Competência")] int competenciaId, UsuarioService service) => await service.DeleteCompetenciaDeUsuarioAsync(usuarioId, competenciaId))
            .WithSummary("Remove uma Competência do Usuário (V1)")
            .WithDescription("Este endpoint é responsável por remover uma associação de Competência de um Usuário. Retorna 204 No Content se possível, ou erro se não for achada.")
            .MapToApiVersion(1, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        usuarios.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, UsuarioService service) => await service.GetAllUsuariosAsync(pageNumber, pageSize))
        .WithSummary("Retorna todos os usuários cadastrados (paginação) (V2)")
        .WithDescription("Este endpoint retorna a lista de todos os usuários, paginados de acordo com os parâmetros **pageNumber** e **pageSize**, incluindo também as competências associadas a cada usuário (se existir).")
        .MapToApiVersion(2, 0)
        .Produces<PagedResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        usuarios.MapGet("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioService service) => await service.GetUsuarioByIdAsync(id))
        .WithSummary("Retorna um usuário pelo ID (V2)")
        .WithDescription("Este endpoint retorna os dados de um usuário pelo ID, incluindo também as competências associadas a ele (se existir). Retorna 200 OK se o usuário for encontrado, ou 404 Not Found se não for achado.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        usuarios.MapPost("/", async (UsuarioPostDto dto, UsuarioService service) => await service.CreateUsuarioAsync(dto))
        .Accepts<UsuarioPostDto>("application/json")
        .WithSummary("Cria um Usuário (V2)")
        .WithDescription("Este endpoint cria um usuário a partir do nome, senha, área atual, área de interesse, objetivo de carreira e nível de experiência. Retorna 201 Created se o usuário for criado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        usuarios.MapPut("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioPostDto dto, UsuarioService service) => await service.UpdateUsuarioAsync(id, dto))
            .WithSummary("Atualiza um Usuário (V2)")
            .WithDescription("Este endpoint é responsável por atualizar os dados existentes de um Usuário a partir de seu ID. Retorna 200 OK se o Usuário for atualizado com sucesso, ou erro caso não seja possível.")
            .MapToApiVersion(2, 0)
            .Produces<ResourceResponse<UsuarioReadDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        usuarios.MapDelete("/{id:int}", async ([Description("Identificador único de Usuário")] int id, UsuarioService service) => await service.DeleteUsuarioAsync(id))
            .WithSummary("Deleta um usuário pelo ID (V2)")
            .WithDescription("Este endpoint é responsável por deletar um Usuário pelo ID informado. Retorna 204 No Content se o Usuário for deletado com sucesso, ou erro se não for achado.")
            .MapToApiVersion(2, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        usuarios.MapPost("/{usuarioId:int}/adicionar-competencia/{competenciaId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("Identificador único de Competência")] int competenciaId, UsuarioService service) => await service.AddCompetenciaAoUsuarioAsync(usuarioId, competenciaId))
            .WithSummary("Adiciona uma competência ao Usuário (V2)")
            .WithDescription("Este endpoint é responsável por adicionar uma associação de Competência a um Usuário. Retorna 204 No Content se possível, ou erro se não for possível.")
            .MapToApiVersion(2, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();

        usuarios.MapDelete("/{usuarioId:int}/remover-competencia/{competenciaId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, [Description("Identificador único de Competência")] int competenciaId, UsuarioService service) => await service.DeleteCompetenciaDeUsuarioAsync(usuarioId, competenciaId))
            .WithSummary("Remove uma Competência do Usuário (V2)")
            .WithDescription("Este endpoint é responsável por remover uma associação de Competência de um Usuário. Retorna 204 No Content se possível, ou erro se não for achada.")
            .MapToApiVersion(2, 0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }
}