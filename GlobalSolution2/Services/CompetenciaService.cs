
using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2.Services;

public class CompetenciaService
{
    private readonly AppDbContext _db;

    public CompetenciaService(AppDbContext db)
    {
        _db = db;
    }

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

    public async Task<IResult> GetCompetenciaByIdAsync(int id)
    {
        var competencia = await _db.Competencias.FindAsync(id);
        if (competencia is null) return Results.NotFound("Nenhuma Competência encontrada com ID informado.");

        var competenciaDto = CompetenciaReadDto.ToDto(competencia);

        var response = new ResourceResponse<CompetenciaReadDto>(
            Data: competenciaDto,
            Links: new List<LinkDto>
            {
                new("self", $"competencias/{id}", "GET"),
                new("list", "/competencias", "GET")
            }
        );
        return Results.Ok(response);
    }

    public async Task<IResult> CreateCompetenciaParaUsuarioAsync(int usuarioId, CompetenciaPostDto dto)
    {
        var usuario = await _db.Usuarios
        .Include(u => u.UsuarioCompetencias)
        .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        var validation = await ValidateCompetencia(dto);
        if (validation is not null) return validation;

        if (usuario is null) return Results.NotFound("Nenhum usuário encontrado com ID informado.");

        var competencia = new Competencia
        {
            NomeCompetencia = dto.NomeCompetencia,
            CategoriaCompetencia = dto.CategoriaCompetencia,
            DescricaoCompetencia = dto.DescricaoCompetencia
        };

        _db.Competencias.Add(competencia);
        await _db.SaveChangesAsync();

        var associacao = new UsuarioCompetencia
        {
            UsuarioId = usuario.UsuarioId,
            CompetenciaId = competencia.CompetenciaId
        };

        _db.UsuarioCompetencias.Add(associacao);
        await _db.SaveChangesAsync();

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

    public async Task<IResult> UpdateCompetenciaAsync(int id, CompetenciaPostDto dto)
    {
        var competenciaExistente = await _db.Competencias.FindAsync(id);
        if (competenciaExistente is null) return Results.NotFound("Competência não encontrada com ID informado.");

        var validation = await ValidateCompetencia(dto);
        if (validation is not null) return validation;

        competenciaExistente.NomeCompetencia = dto.NomeCompetencia;
        competenciaExistente.CategoriaCompetencia = dto.CategoriaCompetencia;
        competenciaExistente.DescricaoCompetencia = dto.DescricaoCompetencia;

        await _db.SaveChangesAsync();

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

    public async Task<IResult> DeleteCompetenciaAsync(int id)
    {
        var competencia = await _db.Competencias.FindAsync(id);
        if (competencia is null) return Results.NotFound("Competência não encontrada com ID informado.");

        _db.Competencias.Remove(competencia);
        await _db.SaveChangesAsync();
        return Results.NoContent();
    }
    
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