using Microsoft.EntityFrameworkCore;
using GlobalSolution2.Dtos;
using GlobalSolution2.Models;

namespace GlobalSolution2.Services;

public class RecomendacaoSaudeService
{
    private readonly AppDbContext _db;

    public RecomendacaoSaudeService(AppDbContext db)
    {
        _db = db;
    }

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

        var recomendacoesDto = recomendacoes.Select(RecomendacaoSaudeResumoDto.ToDto).ToList();

        var response = new PagedResponse<RecomendacaoSaudeResumoDto>(
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

    public async Task<IResult> CreateRecomendacaoSaudeAsync(RecomendacaoSaudePostDto dto)
    {
        var validation = await ValidateRecomendacaoSaude(dto);
        if (validation is not null) return validation;

        var usuario = await _db.Usuarios.FindAsync(dto.UsuarioId);
        if (usuario is null) return Results.NotFound("Nenhum usuário encontrado com o ID informado.");

        var recomendacao = new RecomendacaoSaude
        {
            DataRecomendacao = DateTime.UtcNow,
            TituloRecomendacao = dto.TituloRecomendacao,
            DescricaoRecomendacao = dto.DescricaoRecomendacao,
            PromptUsado = dto.PromptUsado,
            TipoSaude = dto.TipoSaude,
            NivelAlerta = dto.NivelAlerta,
            MensagemSaude = dto.MensagemSaude,
            UsuarioId = dto.UsuarioId,
            Usuario = usuario
        };

        _db.RecomendacoesSaude.Add(recomendacao);
        await _db.SaveChangesAsync();

        var recomendacaoDto = RecomendacaoSaudeReadDto.ToDto(recomendacao);


        var response = new ResourceResponse<RecomendacaoSaudeReadDto>(
            Data: recomendacaoDto,
            Links: new List<LinkDto>
            {
                new("self", $"/recomendacoes-saude/{recomendacao.RecomendacaoId}", "GET"),
                new("list", "/recomendacoes-saude", "GET")
            }
        );

        return Results.Created($"/recomendacoes-saude/{recomendacao.RecomendacaoId}", response);
    }

    public async Task<IResult> DeleteRecomendacaoSaudeAsync(int id) 
    {
        var recomendacao = await _db.RecomendacoesSaude.FindAsync(id);
        if (recomendacao is null) return Results.NotFound("Recomendação de Saúde não encontrada com ID informado.");

        _db.RecomendacoesSaude.Remove(recomendacao);
        await _db.SaveChangesAsync();
        return Results.NoContent();
    }

    private async Task<IResult?> ValidateRecomendacaoSaude(RecomendacaoSaudePostDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.TituloRecomendacao))
            return Results.BadRequest("O título da recomendação é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.DescricaoRecomendacao))
            return Results.BadRequest("A descrição da recomendação é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.PromptUsado))
            return Results.BadRequest("O prompt utilizado é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.TipoSaude))
            return Results.BadRequest("O tipo de saúde é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.NivelAlerta))
            return Results.BadRequest("O nível de alerta é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.MensagemSaude))
            return Results.BadRequest("A mensagem de saúde é obrigatória.");

        var tiposValidos = new[] { "Sono", "Produtividade", "Saúde Mental" };
        var niveisValidos = new[] { "Baixo", "Moderado", "Alto" };

        if (!tiposValidos.Contains(dto.TipoSaude))
            return Results.BadRequest($"Tipo de saúde inválido. Valores aceitos: {string.Join(", ", tiposValidos)}.");

        if (!niveisValidos.Contains(dto.NivelAlerta))
            return Results.BadRequest($"Nível de alerta inválido. Valores aceitos: {string.Join(", ", niveisValidos)}.");

        return null;
    }

}
