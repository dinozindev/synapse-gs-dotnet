
using System.ComponentModel;
using Asp.Versioning.Builder;
using GlobalSolution2.Dtos;
using GlobalSolution2.Services;

namespace GlobalSolution2.Endpoints;

public static class RegistroBemEstarEndpoints {
    public static void MapRegistroBemEstarEndpoints(this IEndpointRouteBuilder app, ApiVersionSet versionSet)
    {
        var registros = app.MapGroup("/api/v{version:apiVersion}/registros-bem-estar")
        .WithApiVersionSet(versionSet)
        .WithTags("RegistrosBemEstar");

        registros.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RegistroBemEstarService service) => await service.GetAllRegistrosBemEstarAsync(pageNumber, pageSize))
        .WithSummary("Retorna todos os registros de bem estar cadastrados (paginação) (V1)")
        .WithDescription("Este endpoint retorna a lista de todos os registros de bem estar, paginados de acordo com os parâmetros **pageNumber** e **pageSize**, incluindo também o Usuário.")
        .MapToApiVersion(1, 0)
        .Produces<PagedResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapGet("/registros-usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, RegistroBemEstarService service) => await service.GetRegistrosDeUsuarioAsync(usuarioId))
        .WithSummary("Retorna todos os registros de bem estar de um usuário pelo ID (paginação) (V1)")
        .WithDescription("Este endpoint retorna a lista de todos os registros de bem estar de um usuário específico, paginados de acordo com os parâmetros **pageNumber** e **pageSize**.")
        .MapToApiVersion(1, 0)
        .Produces<PagedResponse<RegistroBemEstarResumoDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapGet("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarService service) => await service.GetRegistroBemEstarByIdAsync(id))
        .WithSummary("Retorna um registro de bem estar a partir do ID (V1)")
        .WithDescription("Este endpoint retorna um registro de bem estar a partir do ID informado. Caso exista retorna 200 OK, caso contrário retorna erro.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapPost("/", async (RegistroBemEstarPostDto dto, RegistroBemEstarService service) => await service.CreateRegistroBemEstarAsync(dto))
        .Accepts<RegistroBemEstarPostDto>("application/json")
        .WithSummary("Cria um registro de bem estar (V1)")
        .WithDescription("Este endpoint cria um registro de bem estar a partir da data do registro, horário de sono e trabalho, humor, nível de energia e estresse, observações e o Usuário. Retorna 201 Created se o registro for criado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapPut("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarPutDto dto, RegistroBemEstarService service) => await service.UpdateRegistroBemEstarAsync(id, dto))
        .WithSummary("Atualiza um registro de bem estar (V1)")
        .WithDescription("Este endpoint atualiza os dados existentes de um registro de bem estar. Retorna 200 OK se o registro for atualizado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(1, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapDelete("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarService service) => await service.DeleteRegistroBemEstarAsync(id))
        .WithSummary("Remove um registro de bem estar pelo ID (V1)")
        .WithDescription("Este endpoint é responsável por deletar um registro de bem estar pelo ID informado. Retorna 204 No Content se o cliente for deletado com sucesso, ou erro se não for possível.")
        .MapToApiVersion(1, 0)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        registros.MapGet("/", async ([Description("O número da página atual (ex: 1)")] int pageNumber, [Description("A quantidade de registros por página (ex: 10)")] int pageSize, RegistroBemEstarService service) => await service.GetAllRegistrosBemEstarAsync(pageNumber, pageSize))
        .WithSummary("Retorna todos os registros de bem estar cadastrados (paginação) (V2)")
        .WithDescription("Este endpoint retorna a lista de todos os registros de bem estar, paginados de acordo com os parâmetros **pageNumber** e **pageSize**, incluindo também o Usuário.")
        .MapToApiVersion(2, 0)
        .Produces<PagedResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        registros.MapGet("/registros-usuario/{usuarioId:int}", async ([Description("Identificador único de Usuário")] int usuarioId, RegistroBemEstarService service) => await service.GetRegistrosDeUsuarioAsync(usuarioId))
        .WithSummary("Retorna todos os registros de bem estar de um usuário pelo ID (paginação) (V2)")
        .WithDescription("Este endpoint retorna a lista de todos os registros de bem estar de um usuário específico, paginados de acordo com os parâmetros **pageNumber** e **pageSize**.")
        .MapToApiVersion(2, 0)
        .Produces<PagedResponse<RegistroBemEstarResumoDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        registros.MapGet("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarService service) => await service.GetRegistroBemEstarByIdAsync(id))
        .WithSummary("Retorna um registro de bem estar a partir do ID (V2)")
        .WithDescription("Este endpoint retorna um registro de bem estar a partir do ID informado. Caso exista retorna 200 OK, caso contrário retorna erro.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        registros.MapPost("/", async (RegistroBemEstarPostDto dto, RegistroBemEstarService service) => await service.CreateRegistroBemEstarAsync(dto))
        .Accepts<RegistroBemEstarPostDto>("application/json")
        .WithSummary("Cria um registro de bem estar (V2)")
        .WithDescription("Este endpoint cria um registro de bem estar a partir da data do registro, horário de sono e trabalho, humor, nível de energia e estresse, observações e o Usuário. Retorna 201 Created se o registro for criado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        registros.MapPut("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarPutDto dto, RegistroBemEstarService service) => await service.UpdateRegistroBemEstarAsync(id, dto))
        .WithSummary("Atualiza um registro de bem estar (V2)")
        .WithDescription("Este endpoint atualiza os dados existentes de um registro de bem estar. Retorna 200 OK se o registro for atualizado com sucesso, ou erro caso não seja possível.")
        .MapToApiVersion(2, 0)
        .Produces<ResourceResponse<RegistroBemEstarReadDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();

        registros.MapDelete("/{id:int}", async ([Description("Identificador único de Registro de Bem Estar")] int id, RegistroBemEstarService service) => await service.DeleteRegistroBemEstarAsync(id))
        .WithSummary("Remove um registro de bem estar pelo ID (V2)")
        .WithDescription("Este endpoint é responsável por deletar um registro de bem estar pelo ID informado. Retorna 204 No Content se o cliente for deletado com sucesso, ou erro se não for possível.")
        .MapToApiVersion(2, 0)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization();
    }
}