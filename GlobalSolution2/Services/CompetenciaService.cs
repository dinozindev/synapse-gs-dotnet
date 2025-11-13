
using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2.Services;

public class CompetenciaService
{
    private readonly AppDbContext _db;
    private readonly ILogger<CompetenciaService> _logger;

    public CompetenciaService(AppDbContext db, ILogger<CompetenciaService> logger)
    {
        _db = db;
        _logger = logger;
    }

    // retorna todas as competencias com paginação
    public async Task<IResult> GetAllCompetenciasAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.Competencias.CountAsync();

        var competencias = await _db.Competencias
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var competenciasDto = competencias.Select(CompetenciaReadDto.ToDto).ToList();

        var response = new PagedResponse<CompetenciaReadDto>(
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: competenciasDto,
            Links: new List<LinkDto>
            {
                new("self", $"/competencias?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize) ? $"/competencias?pageNumber={pageNumber+1}&pageSize={pageSize}" : string.Empty, "GET"),
                new("prev", pageNumber > 1 ? $"/competencias?pageNumber={pageNumber-1}&pageSize={pageSize}" : string.Empty, "GET")
            }
        );
        return competenciasDto.Count != 0 ? Results.Ok(response) : Results.NoContent();
    }

    // retorna uma competência por ID
    public async Task<IResult> GetCompetenciaByIdAsync(int id)
    {
        _logger.LogInformation("Buscando competência com ID {Id}", id);

        var competencia = await _db.Competencias.FindAsync(id);

        if (competencia is null)
        {
            _logger.LogWarning("Competência com ID {Id} não encontrada", id);
            return Results.NotFound("Nenhuma Competência encontrada com ID informado.");
        }

        _logger.LogInformation("Competência de ID {Id} encontrada com sucesso", id);

        var competenciaDto = CompetenciaReadDto.ToDto(competencia);

        var response = new ResourceResponse<CompetenciaReadDto>(
            Data: competenciaDto,
            Links: new List<LinkDto>
            {
                new("self", $"/competencias/{id}", "GET"),
                new("update", $"/competencias/{id}", "PUT"),
                new("delete", $"/competencias/{id}", "DELETE"),
                new("list", "/competencias", "GET")
            }
        );
        return Results.Ok(response);
    }

    // Cria uma competência para um usuário
    public async Task<IResult> CreateCompetenciaParaUsuarioAsync(int usuarioId, CompetenciaPostDto dto)
    {
        _logger.LogInformation("Iniciando criação de competência: {NomeCompetencia}", dto.NomeCompetencia);

        var usuario = await _db.Usuarios
        .Include(u => u.UsuarioCompetencias)
        .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario is null)
        {
            _logger.LogWarning("Nenhum usuário encontrado com o ID {UsuarioId}", usuarioId);
            return Results.NotFound("Nenhum usuário encontrado com ID informado.");
        } 

        var validation = await ValidateCompetencia(dto);
        if (validation is not null)
        {
            _logger.LogWarning("Falha na validação ao criar competência {NomeCompetencia}", dto.NomeCompetencia);
            return validation;
        } 

        var competencia = new Competencia
        {
            NomeCompetencia = dto.NomeCompetencia,
            CategoriaCompetencia = dto.CategoriaCompetencia,
            DescricaoCompetencia = dto.DescricaoCompetencia
        };

        _db.Competencias.Add(competencia);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Competência criada com sucesso: ID {Id}", competencia.CompetenciaId);

        var associacao = new UsuarioCompetencia
        {
            UsuarioId = usuario.UsuarioId,
            CompetenciaId = competencia.CompetenciaId
        };

        _db.UsuarioCompetencias.Add(associacao);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Associação entre Competência de ID {CompetenciaId} e Usuário de ID {UsuarioId} realizada com sucesso", competencia.CompetenciaId, usuarioId);

        var competenciaDto = CompetenciaReadDto.ToDto(competencia);

        var response = new ResourceResponse<CompetenciaReadDto>(
            Data: competenciaDto,
            Links: new List<LinkDto>
            {
                new("self", $"/competencias/{competenciaDto.CompetenciaId}", "GET"),
                new("update", $"/competencias/{competenciaDto.CompetenciaId}", "PUT"),
                new("delete", $"/competencias/{competenciaDto.CompetenciaId}", "DELETE"),
                new("list", "/competencias", "GET")
            }
        );

        return Results.Created($"/competencias/{competencia.CompetenciaId}", response);
    }

    // Atualiza uma competência
    public async Task<IResult> UpdateCompetenciaAsync(int id, CompetenciaPostDto dto)
    {
        _logger.LogInformation("Iniciando atualização de competência com ID {Id}", id);

        var competenciaExistente = await _db.Competencias.FindAsync(id);
        if (competenciaExistente is null)
        {
            _logger.LogWarning("Competência com ID {Id} não encontrada para atualização", id);
            return Results.NotFound("Competência não encontrada com ID informado.");
        } 

        var validation = await ValidateCompetencia(dto);
        if (validation is not null)
        {
            _logger.LogWarning("Falha na validação ao atualizar competência {NomeCompetencia}", dto.NomeCompetencia);
            return validation;
        } 

        competenciaExistente.NomeCompetencia = dto.NomeCompetencia;
        competenciaExistente.CategoriaCompetencia = dto.CategoriaCompetencia;
        competenciaExistente.DescricaoCompetencia = dto.DescricaoCompetencia;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Competência de ID {Id} atualizada com sucesso", id);

        var competenciaDto = CompetenciaReadDto.ToDto(competenciaExistente);

        var response = new ResourceResponse<CompetenciaReadDto>(
            Data: competenciaDto,
            Links: new List<LinkDto>
            {
                new("self", $"/competencias/{competenciaDto.CompetenciaId}", "GET"),
                new("update", $"/competencias/{id}", "PUT"),
                new("delete", $"/competencias/{competenciaDto.CompetenciaId}", "DELETE"),
                new("list", "/competencias", "GET")
            }
        );
        return Results.Ok(response);
    }

    // deleta uma competência
    public async Task<IResult> DeleteCompetenciaAsync(int id)
    {

        _logger.LogInformation("Iniciando remoção de competência de ID {Id}", id);

        var competencia = await _db.Competencias.FindAsync(id);
        if (competencia is null)
        {
            _logger.LogWarning("Competência com ID {Id} não encontrada para remoção", id);
            return Results.NotFound("Competência não encontrada com ID informado.");
        }

        _db.Competencias.Remove(competencia);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Competência de ID {Id} excluída com sucesso", id);

        return Results.NoContent();
    }
    
    // Validação de POST e PUT
    private async Task<IResult?> ValidateCompetencia(CompetenciaPostDto dto, int? ignoreId = null)
    {
        if (string.IsNullOrWhiteSpace(dto.NomeCompetencia))
            return Results.BadRequest("O nome da competência é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.CategoriaCompetencia))
            return Results.BadRequest("A categoria da competência é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.DescricaoCompetencia))
            return Results.BadRequest("A descrição da competência é obrigatória.");

        return null;
    }
}