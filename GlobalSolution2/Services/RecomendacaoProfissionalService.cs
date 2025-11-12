using Microsoft.EntityFrameworkCore;
using GlobalSolution2.Dtos;

namespace GlobalSolution2.Services;

public class RecomendacaoProfissionalService
{
    private readonly AppDbContext _db;

    public RecomendacaoProfissionalService(AppDbContext db)
    {
        _db = db;
    }

    // GET - Todas as recomendações (com paginação)
    public async Task<IResult> GetAllRecomendacoesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.RecomendacoesProfissionais.CountAsync();

        var recomendacoes = await _db.RecomendacoesProfissionais
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.DataRecomendacao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (!recomendacoes.Any()) return Results.NoContent();

        var recomendacoesDto = recomendacoes.Select(RecomendacaoProfissionalReadDto.ToDto).ToList();

        var response = new PagedResponse<RecomendacaoProfissionalReadDto>(
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: recomendacoesDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/profissional?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
                    ? $"/recomendacoes/profissional?pageNumber={pageNumber + 1}&pageSize={pageSize}"
                    : string.Empty, "GET"),
                new("prev", pageNumber > 1
                    ? $"/recomendacoes/profissional?pageNumber={pageNumber - 1}&pageSize={pageSize}"
                    : string.Empty, "GET")
            }
        );

        return Results.Ok(response);
    }

    // GET - Recomendação por ID
    public async Task<IResult> GetRecomendacaoByIdAsync(int id)
    {
        var recomendacao = await _db.RecomendacoesProfissionais
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.RecomendacaoId == id);

        if (recomendacao is null)
            return Results.NotFound("Nenhuma recomendação profissional encontrada com o ID informado.");

        var dto = RecomendacaoProfissionalReadDto.ToDto(recomendacao);

        var response = new ResourceResponse<RecomendacaoProfissionalReadDto>(
            Data: dto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/profissional/{id}", "GET"),
                new("list", "/recomendacoes/profissional", "GET")
            }
        );

        return Results.Ok(response);
    }

    // GET - Todas as recomendações de um usuário específico
    public async Task<IResult> GetRecomendacoesByUsuarioAsync(int usuarioId, int pageNumber = 1, int pageSize = 10)
    {
        var usuario = await _db.Usuarios.FindAsync(usuarioId);
        if (usuario is null)
            return Results.NotFound("Usuário não encontrado.");

        var recomendacoes = await _db.RecomendacoesProfissionais
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.DataRecomendacao)
            .ToListAsync();

        var totalCount = recomendacoes.Count;

        if (!recomendacoes.Any())
            return Results.NoContent();

        var recomendacoesDto = recomendacoes.Select(RecomendacaoProfissionalReadDto.ToDto).ToList();

        var response = new PagedResponse<RecomendacaoProfissionalReadDto>(
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: recomendacoesDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes/profissional?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize)
                    ? $"/recomendacoes/profissional?pageNumber={pageNumber + 1}&pageSize={pageSize}"
                    : string.Empty, "GET"),
                new("prev", pageNumber > 1
                    ? $"/recomendacoes/profissional?pageNumber={pageNumber - 1}&pageSize={pageSize}"
                    : string.Empty, "GET")
            }
        );

        return Results.Ok(response);
    }
}
