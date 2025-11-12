using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2.Services;

public class RegistroBemEstarService
{
    private readonly AppDbContext _db;

    public RegistroBemEstarService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IResult> GetAllRegistrosBemEstarAsync(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = await _db.RegistrosBemEstar.CountAsync();

        var registros = await _db.RegistrosBemEstar
            .Include(r => r.Usuario)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var registrosDto = registros.Select(RegistroBemEstarReadDto.ToDto).ToList();

        var response = new PagedResponse<RegistroBemEstarReadDto>
        (
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: registrosDto,
            Links: new List<LinkDto>
            {
                new("self", $"/registros-bem-estar?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
                new("create", "/registros-bem-estar", "POST"),
                new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize) ? $"/registros-bem-estar?pageNumber={pageNumber+1}&pageSize={pageSize}" : string.Empty, "GET"),
                new("prev", pageNumber > 1 ? $"/registros-bem-estar?pageNumber={pageNumber-1}&pageSize={pageSize}" : string.Empty, "GET")
            }
        );

        return registrosDto.Count != 0 ? Results.Ok(response) : Results.NoContent();
    }

    public async Task<IResult> GetRegistrosDeUsuarioAsync(int usuarioId, int pageNumber = 1, int pageSize = 10)
    {
        var usuarioExiste = await _db.Usuarios.AnyAsync(u => u.UsuarioId == usuarioId);
        if (!usuarioExiste) return Results.NotFound($"Nenhum usuário encontrado com o ID {usuarioId}.");

        var totalCount = await _db.RegistrosBemEstar
            .Where(r => r.UsuarioId == usuarioId)
            .CountAsync();

        var registros = await _db.RegistrosBemEstar
            .Where(r => r.UsuarioId == usuarioId)
            .Include(r => r.Usuario)
            .OrderByDescending(r => r.DataRegistro)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var registrosDto = registros.Select(RegistroBemEstarResumoDto.ToDto).ToList();

        var response = new PagedResponse<RegistroBemEstarResumoDto>
        (
            TotalCount: totalCount,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalPages: (int)Math.Ceiling(totalCount / (double)pageSize),
            Data: registrosDto,
            Links: new List<LinkDto>
            {
            new("self", $"/usuarios/{usuarioId}/registros-bem-estar?pageNumber={pageNumber}&pageSize={pageSize}", "GET"),
            new("create", $"/usuarios/{usuarioId}/registros-bem-estar", "POST"),
            new("next", pageNumber < (int)Math.Ceiling(totalCount / (double)pageSize) ? $"/usuarios/{usuarioId}/registros-bem-estar?pageNumber={pageNumber + 1}&pageSize={pageSize}" : string.Empty, "GET"),
            new("prev", pageNumber > 1 ? $"/usuarios/{usuarioId}/registros-bem-estar?pageNumber={pageNumber - 1}&pageSize={pageSize}" : string.Empty, "GET")
            }
        );

        return registrosDto.Count != 0 ? Results.Ok(response) : Results.NoContent();
    }

    public async Task<IResult> GetRegistroBemEstarByIdAsync(int id)
    {
        var registro = await _db.RegistrosBemEstar.Include(r => r.Usuario).FirstOrDefaultAsync(r => r.RegistroId == id);
        if (registro is null) return Results.NotFound("Registro de Bem Estar não encontrado com ID informado.");

        var registroDto = RegistroBemEstarReadDto.ToDto(registro);

        var response = new ResourceResponse<RegistroBemEstarReadDto>(
            Data: registroDto,
            Links: new List<LinkDto>
            {
                new("self", $"/registros-bem-estar/{id}", "GET"),
                new("update", $"/registros-bem-estar/{id}", "PUT"),
                new("delete", $"/registros-bem-estar/{id}", "DELETE"),
                new("list", "/registros-bem-estar", "GET")
            }
        );

        return Results.Ok(response);
    }

    public async Task<IResult> CreateRegistroBemEstarAsync(RegistroBemEstarPostDto dto)
    {
        var validation = await ValidateRegistroPost(dto);
        if (validation is not null) return validation;

        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == dto.UsuarioId);
        if (usuario is null) return Results.NotFound("Usuário com ID fornecido não encontrado.");

        var registro = new RegistroBemEstar
        {
            DataRegistro = dto.DataRegistro,
            HumorRegistro = dto.HumorRegistro,
            HorasSono = dto.HorasSono,
            HorasTrabalho = dto.HorasTrabalho,
            NivelEnergia = dto.NivelEnergia,
            NivelEstresse = dto.NivelEstresse,
            UsuarioId = dto.UsuarioId,
            Usuario = usuario
        };

        _db.RegistrosBemEstar.Add(registro);
        await _db.SaveChangesAsync();

        var registroDto = RegistroBemEstarReadDto.ToDto(registro);

        var response = new ResourceResponse<RegistroBemEstarReadDto>(
            Data: registroDto,
            Links: new List<LinkDto>
            {
                new("self", $"/registros-bem-estar/{registroDto.RegistroId}", "GET"),
                new("update", $"/registros-bem-estar/{registroDto.RegistroId}", "PUT"),
                new("delete", $"/registros-bem-estar/{registroDto.RegistroId}", "DELETE"),
                new("list", "/registros-bem-estar", "GET")
            }
        );
        return Results.Created($"/registros-bem-estar/{registroDto.RegistroId}", response);
    }

    public async Task<IResult> UpdateRegistroBemEstarAsync(int id, RegistroBemEstarPutDto dto)
    {
        var registroExistente = await _db.RegistrosBemEstar.FindAsync(id);
        if (registroExistente is null) return Results.NotFound("Registro não encontrado com ID informado.");

        var validation = await ValidateRegistroPut(dto);
        if (validation is not null) return validation;

        registroExistente.HumorRegistro = dto.HumorRegistro;
        registroExistente.HorasSono = dto.HorasSono;
        registroExistente.HorasTrabalho = dto.HorasTrabalho;
        registroExistente.NivelEnergia = dto.NivelEnergia;
        registroExistente.NivelEstresse = dto.NivelEstresse;

        await _db.SaveChangesAsync();

        var registroDto = RegistroBemEstarReadDto.ToDto(registroExistente);

        var response = new ResourceResponse<RegistroBemEstarReadDto>(
            Data: registroDto,
            Links: new List<LinkDto>
            {
                new("self", $"/registros-bem-estar/{registroDto.RegistroId}", "GET"),
                new("update", $"/registros-bem-estar/{registroDto.RegistroId}", "PUT"),
                new("delete", $"/registros-bem-estar/{registroDto.RegistroId}", "DELETE"),
                new("list", "/registros-bem-estar", "GET")
            });

        return Results.Ok(response);
    }

    public async Task<IResult> DeleteRegistroBemEstarAsync(int id)
    {
        var registro = await _db.RegistrosBemEstar.FindAsync(id);
        if (registro is null) return Results.NotFound("Registro de Bem Estar não encontrado com ID informado.");

        _db.RegistrosBemEstar.Remove(registro);
        await _db.SaveChangesAsync();
        return Results.NoContent();
    }

    // valida as informações do RegistroBemEstar para POST
    private async Task<IResult?> ValidateRegistroPost(RegistroBemEstarPostDto dto, int? ignoreId = null)
    {

        var humoresValidos = new[] { "Feliz", "Triste", "Estressado", "Bravo", "Calmo" };
        if (!humoresValidos.Contains(dto.HumorRegistro))
        {
            return Results.BadRequest("Humor inválido. Valores aceitos: Feliz, Triste, Estressado, Bravo ou Calmo.");
        }

        if (dto.DataRegistro > DateTime.Now)
        {
            return Results.BadRequest("A data do registro não pode ser no futuro.");
        }

        if (dto.HorasSono < 0 || dto.HorasSono > 24)
        {
            return Results.BadRequest("As horas de sono devem estar entre 0 e 24.");
        }

        if (dto.HorasTrabalho < 0 || dto.HorasTrabalho > 24)
        {
            return Results.BadRequest("As horas de trabalho devem estar entre 0 e 24.");
        }

        if (dto.NivelEnergia < 1 || dto.NivelEnergia > 10)
        {
            return Results.BadRequest("O nível de energia deve estar entre 1 e 10.");
        }

        if (dto.NivelEstresse < 1 || dto.NivelEstresse > 10)
        {
            return Results.BadRequest("O nível de estresse deve estar entre 1 e 10.");
        }

        var registroExistente = await _db.RegistrosBemEstar
            .AnyAsync(r => r.UsuarioId == dto.UsuarioId &&
                           r.DataRegistro.Date == dto.DataRegistro.Date &&
                           (!ignoreId.HasValue || r.RegistroId != ignoreId.Value));

        if (registroExistente)
        {
            return Results.Conflict("Já existe um registro de bem-estar para esse usuário nesta data.");
        }

        return null;
    }

    private async Task<IResult?> ValidateRegistroPut(RegistroBemEstarPutDto dto)
    {
        var humoresValidos = new[] { "Feliz", "Triste", "Estressado", "Bravo", "Calmo" };
        if (!humoresValidos.Contains(dto.HumorRegistro, StringComparer.OrdinalIgnoreCase))
        {
            return Results.BadRequest("O humor informado é inválido. Valores aceitos: Feliz, Triste, Estressado, Bravo ou Calmo.");
        }

        if (dto.HorasSono < 0 || dto.HorasSono > 24)
        {
            return Results.BadRequest("O campo 'HorasSono' deve estar entre 0 e 24.");
        }

        if (dto.HorasTrabalho < 0 || dto.HorasTrabalho > 24)
        {
            return Results.BadRequest("O campo 'HorasTrabalho' deve estar entre 0 e 24.");
        }

        if (dto.NivelEnergia < 1 || dto.NivelEnergia > 10)
        {
            return Results.BadRequest("O campo 'NivelEnergia' deve estar entre 1 e 10.");
        }

        if (dto.NivelEstresse < 1 || dto.NivelEstresse > 10)
        {
            return Results.BadRequest("O campo 'NivelEstresse' deve estar entre 1 e 10.");
        }

        if (!string.IsNullOrWhiteSpace(dto.ObservacaoRegistro) && dto.ObservacaoRegistro.Length > 255)
        {
            return Results.BadRequest("O campo 'ObservacaoRegistro' não pode exceder 255 caracteres.");
        }

        return null;
    }



}