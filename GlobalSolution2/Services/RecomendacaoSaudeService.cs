using Microsoft.EntityFrameworkCore;
using GlobalSolution2.Dtos;

namespace GlobalSolution2.Services;

public class RecomendacaoSaudeService
{
    private readonly AppDbContext _db;

    public RecomendacaoSaudeService(AppDbContext db)
    {
        _db = db;
    }

    // GET - Todas as recomendações de saúde (paginadas)
    public async Task<IResult> GetAllRecomendacoesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.RecomendacoesSaude.CountAsync();

        var recomendacoes = await _db.RecomendacoesSaude
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.DataRecomendacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!recomendacoes.Any()) return Results.NoContent();

        var recomendacoesDto = recomendacoes.Select(RecomendacaoSaudeReadDto.ToDto).ToList();

        var response = new PagedResponse<RecomendacaoSaudeReadDto>(
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: recomendacoesDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/saude?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
                    ? $"/recomendacoes/saude?pageNumber={pageNumber + 1}&pageSize={pageSize}"
                    : string.Empty, "GET"),
                new("prev", pageNumber > 1
                    ? $"/recomendacoes/saude?pageNumber={pageNumber - 1}&pageSize={pageSize}"
                    : string.Empty, "GET")
            }
        );

        return Results.Ok(response);
    }

    // GET - Recomendação de saúde por ID
    public async Task<IResult> GetRecomendacaoByIdAsync(int id)
    {
        var recomendacao = await _db.RecomendacoesSaude
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.RecomendacaoId == id);

        if (recomendacao is null)
            return Results.NotFound("Nenhuma recomendação de saúde encontrada com o ID informado.");

        var dto = RecomendacaoSaudeReadDto.ToDto(recomendacao);

        var response = new ResourceResponse<RecomendacaoSaudeReadDto>(
            Data: dto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/saude/{id}", "GET"),
                new("list", "/recomendacoes/saude", "GET")
            }
        );

        return Results.Ok(response);
    }

    // GET - Todas as recomendações de saúde de um usuário específico
    public async Task<IResult> GetRecomendacoesByUsuarioAsync(int usuarioId, int pageNumber = 1, int pageSize = 10)
    {
        var usuario = await _db.Usuarios.FindAsync(usuarioId);
        if (usuario is null)
            return Results.NotFound("Usuário não encontrado.");

        var recomendacoes = await _db.RecomendacoesSaude
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.DataRecomendacao)
            .ToListAsync();
        
        var totalCount = recomendacoes.Count;

        if (!recomendacoes.Any())
            return Results.NoContent();

        var recomendacoesDto = recomendacoes.Select(RecomendacaoSaudeReadDto.ToDto).ToList();

        var response = new PagedResponse<RecomendacaoSaudeReadDto>(
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: recomendacoesDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/saude?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
                    ? $"/recomendacoes/saude?pageNumber={pageNumber + 1}&pageSize={pageSize}"
                    : string.Empty, "GET"),
                new("prev", pageNumber > 1
                    ? $"/recomendacoes/saude?pageNumber={pageNumber - 1}&pageSize={pageSize}"
                    : string.Empty, "GET")
            }
        );

        return Results.Ok(response);
    }
}
