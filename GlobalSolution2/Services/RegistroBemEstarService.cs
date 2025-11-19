using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2.Services;

public class RegistroBemEstarService
{
    private readonly AppDbContext _db;
    private readonly ILogger<RegistroBemEstarService> _logger;

    public RegistroBemEstarService(AppDbContext db, ILogger<RegistroBemEstarService> logger)
    {
        _db = db;
        _logger = logger;
    }

    // retorna todos os registro bem estar com paginação
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

    // retorna todos os registros bem estar de um usuário
    public async Task<IResult> GetRegistrosDeUsuarioAsync(int usuarioId, int pageNumber = 1, int pageSize = 10)
    {

        _logger.LogInformation("Buscando todos os registros de bem estar do usuário com ID {Id}", usuarioId);

        var usuarioExiste = await _db.Usuarios.Where(u => u.UsuarioId == usuarioId).FirstOrDefaultAsync();
        if (usuarioExiste is null)
        {
            _logger.LogWarning("Usuário com ID {Id} não encontrado", usuarioId);
            return Results.NotFound($"Nenhum usuário encontrado com o ID {usuarioId}.");
        } 

        var totalCount = await _db.RegistrosBemEstar
            .Where(r => r.UsuarioId == usuarioId)
            .CountAsync();

        var registros = await _db.RegistrosBemEstar
            .Where(r => r.UsuarioId == usuarioId)
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

    // retorna um Registro bem estar pelo ID
    public async Task<IResult> GetRegistroBemEstarByIdAsync(int id)
    {
        _logger.LogInformation("Buscando Registro de Bem Estar com ID {Id}", id);

        var registro = await _db.RegistrosBemEstar.Include(r => r.Usuario).FirstOrDefaultAsync(r => r.RegistroId == id);

        if (registro is null)
        {
            _logger.LogWarning("Registro de Bem Estar com ID {Id} não encontrado", id);
            return Results.NotFound("Registro de Bem Estar não encontrado com ID informado.");
        }

        _logger.LogInformation("Registro de Bem Estar de ID {Id} encontrado com sucesso", id);

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

    // cria um novo registro bem estar
    public async Task<IResult> CreateRegistroBemEstarAsync(RegistroBemEstarPostDto dto)
    {
        _logger.LogInformation("Iniciando criação de Registro de Bem Estar do Usuário de ID {Id}", dto.UsuarioId);

        var validation = await ValidateRegistroPost(dto);
        if (validation is not null)
        {
            _logger.LogWarning("Falha na validação ao criar um registro para o Usuário de ID {Id}", dto.UsuarioId);
            return validation;
        }

        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == dto.UsuarioId);
        if (usuario is null)
        {
            _logger.LogWarning("Usuário com ID {Id} não encontrado", dto.UsuarioId);
            return Results.NotFound("Usuário com ID fornecido não encontrado.");
        }

        var registro = new RegistroBemEstar
        {
            DataRegistro = dto.DataRegistro,
            HumorRegistro = dto.HumorRegistro,
            HorasSono = dto.HorasSono,
            HorasTrabalho = dto.HorasTrabalho,
            NivelEnergia = dto.NivelEnergia,
            NivelEstresse = dto.NivelEstresse,
            ObservacaoRegistro = dto.ObservacaoRegistro,
            UsuarioId = dto.UsuarioId,
            Usuario = usuario
        };

        _db.RegistrosBemEstar.Add(registro);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Registro de Bem Estar de ID {Id} cadastrado com sucesso", registro.RegistroId);

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

    // atualiza um registro bem estar
    public async Task<IResult> UpdateRegistroBemEstarAsync(int id, RegistroBemEstarPutDto dto)
    {
        _logger.LogInformation("Iniciando atualização de registro de ID {Id}", id);
        var registroExistente = await _db.RegistrosBemEstar
        .Include(r => r.Usuario)
        .FirstOrDefaultAsync(r => r.RegistroId == id);
        if (registroExistente is null)
        {
            _logger.LogInformation("Registro de ID {Id} não encontrado", id);
            return Results.NotFound("Registro não encontrado com ID informado.");
        }

        var validation = await ValidateRegistroPut(dto);
        if (validation is not null)
        {
            _logger.LogWarning("Falha na validação ao atualizar o registro de ID {Id}", id);
            return validation;
        }

        registroExistente.HumorRegistro = dto.HumorRegistro;
        registroExistente.HorasSono = dto.HorasSono;
        registroExistente.HorasTrabalho = dto.HorasTrabalho;
        registroExistente.NivelEnergia = dto.NivelEnergia;
        registroExistente.NivelEstresse = dto.NivelEstresse;
        registroExistente.ObservacaoRegistro = dto.ObservacaoRegistro;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Registro de Bem Estar de ID {Id} atualizado com sucesso", id);

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

    // remove um registro bem estar
    public async Task<IResult> DeleteRegistroBemEstarAsync(int id)
    {
        _logger.LogInformation("Iniciando remoção de registro de ID {Id}", id);
        var registro = await _db.RegistrosBemEstar.FindAsync(id);
        if (registro is null)
        {
            _logger.LogWarning("Registro de Bem Estar de ID {Id} não encontrado", id);
            return Results.NotFound("Registro de Bem Estar não encontrado com ID informado.");
        } 

        _db.RegistrosBemEstar.Remove(registro);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Registro de Bem Estar de ID {Id} removido com sucesso", id);

        return Results.NoContent();
    }

    private async Task<IResult?> ValidateRegistroPost(RegistroBemEstarPostDto dto)
    {
        var humoresValidos = new[] { "Feliz", "Triste", "Estressado", "Bravo", "Calmo" };
        if (!humoresValidos.Contains(dto.HumorRegistro))
            return Results.BadRequest("Humor inválido. Valores aceitos: Feliz, Triste, Estressado, Bravo ou Calmo.");

        if (dto.HorasSono < 0 || dto.HorasSono > 24)
            return Results.BadRequest("As horas de sono devem estar entre 0 e 24.");

        if (dto.HorasTrabalho < 0 || dto.HorasTrabalho > 24)
            return Results.BadRequest("As horas de trabalho devem estar entre 0 e 24.");

        if (dto.NivelEnergia < 1 || dto.NivelEnergia > 10)
            return Results.BadRequest("O nível de energia deve estar entre 1 e 10.");

        if (dto.NivelEstresse < 1 || dto.NivelEstresse > 10)
            return Results.BadRequest("O nível de estresse deve estar entre 1 e 10.");

        if (!string.IsNullOrWhiteSpace(dto.ObservacaoRegistro) && dto.ObservacaoRegistro.Length > 255)
        {
            return Results.BadRequest("O campo 'ObservacaoRegistro' não pode exceder 255 caracteres.");
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