using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2.Services;

public class UsuarioService
{
    private readonly AppDbContext _db;

    public UsuarioService(AppDbContext db)
    {
        _db = db;
    }

    // retorna todos os usuários com paginação
    public async Task<IResult> GetAllUsuariosAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.Usuarios.CountAsync();

        var usuarios = await _db.Usuarios
            .Include(u => u.UsuarioCompetencias)
                .ThenInclude(uc => uc.Competencia)
            .OrderBy(u => u.UsuarioId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var usuariosDto = usuarios.Select(UsuarioReadDto.ToDto).ToList();

        var response = new PagedResponse<UsuarioReadDto>
        (
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: usuariosDto,
            Links: new List<LinkDto>
            {
                new("self", $"/usuarios?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("create", "/usuarios", "POST"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize) ? $"/usuarios?pageNumber={pageNumber+1}&pageSize={pageSize}" : string.Empty, "GET"),
                new("prev", pageNumber > 1 ? $"/usuarios?pageNumber={pageNumber-1}&pageSize={pageSize}" : string.Empty, "GET")
            }
        );

        return usuariosDto.Count != 0 ? Results.Ok(response) : Results.NoContent();
    }

    // retorna um usuário por ID
    public async Task<IResult> GetUsuarioByIdAsync(int id)
    {
        var usuario = await _db.Usuarios
            .Include(u => u.UsuarioCompetencias)
                .ThenInclude(uc => uc.Competencia)
            .FirstOrDefaultAsync(u => u.UsuarioId == id);

        if (usuario == null) return Results.NotFound("Usuário não encontrado com ID informado.");

        var usuarioDto = UsuarioReadDto.ToDto(usuario);

        var response = new ResourceResponse<UsuarioReadDto>
        (
            Data: usuarioDto,
            Links: new List<LinkDto>
            {
                new("self", $"/usuarios/{id}", "GET"),
                new("update", $"/usuarios/{id}", "PUT"),
                new("delete", $"/usuarios/{id}", "DELETE"),
                new("list", "/usuarios", "GET")
            }
        );

        return Results.Ok(response);
    }

    public async Task<IResult> CreateUsuarioAsync(UsuarioPostDto dto)
    {
        var validation = await ValidateUsuario(dto);
        if (validation is not null) return validation;

        var usuario = new Usuario
        {
            NomeUsuario = dto.NomeUsuario,
            SenhaUsuario = dto.SenhaUsuario,
            AreaAtual = dto.AreaAtual,
            AreaInteresse = dto.AreaInteresse,
            ObjetivoCarreira = dto.ObjetivoCarreira,
            NivelExperiencia = dto.NivelExperiencia,
        };

        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        var usuarioDto = UsuarioReadDto.ToDto(usuario);

        var response = new ResourceResponse<UsuarioReadDto>(
            Data: usuarioDto,
            Links: new List<LinkDto>
            {
                new("self", $"/usuarios/{usuarioDto.UsuarioId}", "GET"),
                new("update", $"/usuarios/{usuarioDto.UsuarioId}", "PUT"),
                new("delete", $"/usuarios/{usuarioDto.UsuarioId}", "DELETE"),
                new("list", "/usuarios", "GET")
            }
        );

        return Results.Created($"/usuarios/{usuario.UsuarioId}", response);
    }

    // atualiza os dados do usuário
    public async Task<IResult> UpdateUsuarioAsync(int id, UsuarioPostDto dto)
    {
        var usuarioExistente = await _db.Usuarios.FindAsync(id);
        if (usuarioExistente is null) return Results.NotFound("Usuário não encontrado com ID informado.");

        var validation = await ValidateUsuario(dto, id);
        if (validation is not null) return validation;

        usuarioExistente.NomeUsuario = dto.NomeUsuario;
        usuarioExistente.SenhaUsuario = dto.SenhaUsuario;
        usuarioExistente.AreaAtual = dto.AreaAtual;
        usuarioExistente.AreaInteresse = dto.AreaInteresse;
        usuarioExistente.ObjetivoCarreira = dto.ObjetivoCarreira;
        usuarioExistente.NivelExperiencia = dto.NivelExperiencia;

        await _db.SaveChangesAsync();

        var usuarioDto = UsuarioReadDto.ToDto(usuarioExistente);

        var response = new ResourceResponse<UsuarioReadDto>(
            Data: usuarioDto,
            Links: new List<LinkDto>
            {
                new("self", $"/usuarios/{usuarioDto.UsuarioId}", "GET"),
                new("update", $"/usuarios/{id}", "PUT"),
                new("delete", $"/usuarios/{usuarioDto.UsuarioId}", "DELETE"),
                new("list", "/usuarios", "GET")
            }
        );

        return Results.Ok(response);
    }

    // deleta um usuário pelo ID
    public async Task<IResult> DeleteUsuarioAsync(int id)
    {
        var usuario = await _db.Usuarios.FindAsync(id);
        if (usuario is null) return Results.NotFound("Usuário não encontrado com ID informado.");

        _db.Usuarios.Remove(usuario);
        await _db.SaveChangesAsync();
        return Results.NoContent();
    }

    // adicionar associação com Competencia
    public async Task<IResult> AddCompetenciaAoUsuarioAsync(int usuarioId, int competenciaId)
    {
        var usuarioExistente = await _db.Usuarios.FindAsync(usuarioId);
        var competenciaExistente = await _db.Competencias.FindAsync(competenciaId);

        if (usuarioExistente == null || competenciaExistente == null) return Results.NotFound("Usuário e Competência inexistentes.");

        var existeAssociacao = await _db.UsuarioCompetencias
        .AnyAsync(uc => uc.UsuarioId == usuarioId && uc.CompetenciaId == competenciaId);

        if (existeAssociacao) return Results.Conflict("A associação entre Usuário e Competência já existe.");

        var usuarioCompetencia = new UsuarioCompetencia
        {
            UsuarioId = usuarioId,
            CompetenciaId = competenciaId
        };

        _db.UsuarioCompetencias.Add(usuarioCompetencia);
        await _db.SaveChangesAsync();

        return Results.NoContent();
    }

    // remover associação com Competencia
    public async Task<IResult> DeleteCompetenciaDeUsuarioAsync(int usuarioId, int competenciaId)
    {
        var usuarioCompetencia = await _db.UsuarioCompetencias
        .FirstOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.CompetenciaId == competenciaId);

        if (usuarioCompetencia == null) return Results.NotFound("Associação entre Usuário e Competência não encontrada.");

        _db.UsuarioCompetencias.Remove(usuarioCompetencia);
        await _db.SaveChangesAsync();

        return Results.NoContent(); 
    }

    // valida as informações do usuário para POST e PUT
    private async Task<IResult?> ValidateUsuario(UsuarioPostDto dto, int? ignoreId = null)
    {
        // valida nome 
        if (string.IsNullOrWhiteSpace(dto.NomeUsuario))
            return Results.BadRequest("O nome de usuário é obrigatório.");

        // valida senha (mínimo de 6 caracteres por segurança)
        if (string.IsNullOrWhiteSpace(dto.SenhaUsuario) || dto.SenhaUsuario.Length < 6)
            return Results.BadRequest("A senha deve conter pelo menos 6 caracteres.");

        // valida área atual
        if (string.IsNullOrWhiteSpace(dto.AreaAtual))
            return Results.BadRequest("A área atual do usuário é obrigatória.");

        // valida área de interesse
        if (string.IsNullOrWhiteSpace(dto.AreaInteresse))
            return Results.BadRequest("A área de interesse é obrigatória.");

        // valida objetivo de carreira
        if (string.IsNullOrWhiteSpace(dto.ObjetivoCarreira))
            return Results.BadRequest("O objetivo de carreira é obrigatório.");

        // valida nível de experiência
        string[] niveisValidos = { "Nenhuma", "Estagiário", "Júnior", "Pleno", "Sênior" };

            if (string.IsNullOrWhiteSpace(dto.NivelExperiencia) ||
                !niveisValidos.Contains(dto.NivelExperiencia, StringComparer.OrdinalIgnoreCase))
            {
                return Results.BadRequest(
                    $"Nível de experiência inválido. Os valores válidos são: {string.Join(", ", niveisValidos)}."
                );
            }

        // verifica se o nome de usuário já existe
        var nomeExistente = await _db.Usuarios
            .Where(u => u.NomeUsuario == dto.NomeUsuario
                        && (!ignoreId.HasValue || u.UsuarioId != ignoreId.Value))
            .AnyAsync();

        if (nomeExistente)
            return Results.Conflict("Já existe um usuário com esse nome de login.");

        return null; 
    }
    }