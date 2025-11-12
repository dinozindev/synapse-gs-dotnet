using Microsoft.EntityFrameworkCore;
using GlobalSolution2.Dtos;
using GlobalSolution2.Models;

namespace GlobalSolution2.Services;

public class RecomendacaoProfissionalService
{
    private readonly AppDbContext _db;

    public RecomendacaoProfissionalService(AppDbContext db)
    {
        _db = db;
    }

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

    public async Task<IResult> CreateRecomendacaoProfissionalAsync(RecomendacaoProfissionalPostDto dto)
    {
        var validation = await ValidateRecomendacaoProfissional(dto);
        if (validation is not null) return validation;

        var usuario = await _db.Usuarios.FindAsync(dto.UsuarioId);
        if (usuario is null) return Results.NotFound("Nenhum usuário encontrado com o ID informado.");

        var recomendacao = new RecomendacaoProfissional
        {
            DataRecomendacao = DateTime.UtcNow,
            TituloRecomendacao = dto.TituloRecomendacao,
            DescricaoRecomendacao = dto.DescricaoRecomendacao,
            PromptUsado = dto.PromptUsado,
            CategoriaRecomendacao = dto.CategoriaRecomendacao,
            AreaRecomendacao = dto.AreaRecomendacao,
            FonteRecomendacao = dto.FonteRecomendacao,
            UsuarioId = dto.UsuarioId,
            Usuario = usuario
        };

        _db.RecomendacoesProfissionais.Add(recomendacao);
        await _db.SaveChangesAsync();

        var recomendacaoDto = RecomendacaoProfissionalReadDto.ToDto(recomendacao);


        var response = new ResourceResponse<RecomendacaoProfissionalReadDto>(
            Data: recomendacaoDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes-profissionais/{recomendacao.RecomendacaoId}", "GET"),
                new("list", "/recomendacoes-profissionais", "GET")
            }
        );

        return Results.Created($"/recomendacoes-profissionais/{recomendacao.RecomendacaoId}", response);
    }

    private async Task<IResult?> ValidateRecomendacaoProfissional(RecomendacaoProfissionalPostDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TituloRecomendacao))
            return Results.BadRequest("O título da recomendação é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.DescricaoRecomendacao))
            return Results.BadRequest("A descrição da recomendação é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.PromptUsado))
            return Results.BadRequest("O prompt utilizado é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.CategoriaRecomendacao))
            return Results.BadRequest("A categoria da recomendação é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.AreaRecomendacao))
            return Results.BadRequest("A área da recomendação é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.FonteRecomendacao))
            return Results.BadRequest("A fonte da recomendação é obrigatória.");

        return null;
    }
}
