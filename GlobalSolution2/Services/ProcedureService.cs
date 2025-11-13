using GlobalSolution2.Dtos;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System.Data;
using System.Text;
using System.Text.Json;

namespace GlobalSolution2.Services;

public class ProcedureService
{
    private readonly string _connectionString;

    private readonly ILogger<ProcedureService> _logger;

    public ProcedureService(IConfiguration configuration, ILogger<ProcedureService> logger)
    {
        _connectionString = configuration.GetConnectionString("OracleConnection")
            ?? throw new InvalidOperationException("Oracle connection string não configurada");
        _logger = logger;
    }

    // Usuário
    public async Task<IResult> InserirUsuarioAsync(UsuarioPostDto dto)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_usuario.sp_inserir_usuario(
                        :p_nome_usuario, 
                        :p_senha_usuario, 
                        :p_area_atual, 
                        :p_area_interesse,
                        :p_objetivo_carreira, 
                        :p_nivel_experiencia
                    ); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_nome_usuario", OracleDbType.Varchar2, 100).Value = dto.NomeUsuario;
            command.Parameters.Add("p_senha_usuario", OracleDbType.Varchar2, 255).Value = dto.SenhaUsuario;
            command.Parameters.Add("p_area_atual", OracleDbType.Varchar2, 100).Value = dto.AreaAtual;
            command.Parameters.Add("p_area_interesse", OracleDbType.Varchar2, 100).Value = dto.AreaInteresse;
            command.Parameters.Add("p_objetivo_carreira", OracleDbType.Varchar2, 255).Value = dto.ObjetivoCarreira;
            command.Parameters.Add("p_nivel_experiencia", OracleDbType.Varchar2, 50).Value = dto.NivelExperiencia;

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            _logger.LogInformation("Output da procedure: {Output}", output);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta da procedure de inserção de usuário");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("Erro:"))
            {
                _logger.LogError("Erro ao inserir usuário: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            // Extrai o ID do usuário criado
            var match = System.Text.RegularExpressions.Regex.Match(output, @"ID:\s*(\d+)");
            var usuarioId = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return Results.Created($"/api/usuarios/{usuarioId}", new
            {
                success = true,
                message = output,
                usuarioId = usuarioId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao inserir usuário: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao inserir usuário");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // UsuarioCompetencia
    public async Task<IResult> InserirUsuarioCompetenciaAsync(int usuarioId, int competenciaId)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_usuario.sp_inserir_usuario_competencia(:p_usuario_id, :p_competencia_id); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = usuarioId;
            command.Parameters.Add("p_competencia_id", OracleDbType.Int32).Value = competenciaId;

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta ao associar competência");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("Erro:"))
            {
                _logger.LogError("Erro ao associar competência: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            return Results.Ok(new
            {
                success = true,
                message = output,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao associar competência");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao associar competência");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // Competência
    public async Task<IResult> InserirCompetenciaAsync(CompetenciaPostDto dto)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_competencia.sp_inserir_competencia(
                        :p_nome_competencia, 
                        :p_categoria_competencia, 
                        :p_descricao_competencia
                    ); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_nome_competencia", OracleDbType.Varchar2, 100).Value = dto.NomeCompetencia;
            command.Parameters.Add("p_categoria_competencia", OracleDbType.Varchar2, 100).Value = dto.CategoriaCompetencia;
            command.Parameters.Add("p_descricao_competencia", OracleDbType.Varchar2, 500).Value = dto.DescricaoCompetencia;

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            _logger.LogInformation("Output da procedure: {Output}", output);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta da procedure de inserção de competência");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("Erro:"))
            {
                _logger.LogError("Erro ao inserir competência: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            var match = System.Text.RegularExpressions.Regex.Match(output, @"ID:\s*(\d+)");
            var competenciaId = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return Results.Created($"/api/competencias/{competenciaId}", new
            {
                success = true,
                message = output,
                competenciaId = competenciaId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao inserir competência: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao inserir competência");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // Registro de Bem Estar
    public async Task<IResult> InserirRegistroBemEstarAsync(RegistroBemEstarPostDto dto)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_bem_estar.sp_inserir_registro_bem_estar(
                        :p_usuario_id,
                        :p_data_registro,
                        :p_humor,
                        :p_horas_sono,
                        :p_horas_trabalho,
                        :p_nivel_energia,
                        :p_nivel_estresse,
                        :p_observacao
                    ); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = dto.UsuarioId;
            command.Parameters.Add("p_data_registro", OracleDbType.Date).Value = dto.DataRegistro;
            command.Parameters.Add("p_humor", OracleDbType.Varchar2, 50).Value = dto.HumorRegistro;
            command.Parameters.Add("p_horas_sono", OracleDbType.Decimal).Value = dto.HorasSono;
            command.Parameters.Add("p_horas_trabalho", OracleDbType.Decimal).Value = dto.HorasTrabalho;
            command.Parameters.Add("p_nivel_energia", OracleDbType.Int32).Value = dto.NivelEnergia;
            command.Parameters.Add("p_nivel_estresse", OracleDbType.Int32).Value = dto.NivelEstresse;

            if (string.IsNullOrEmpty(dto.ObservacaoRegistro))
            {
                command.Parameters.Add("p_observacao", OracleDbType.Varchar2).Value = DBNull.Value;
            }
            else
            {
                command.Parameters.Add("p_observacao", OracleDbType.Varchar2, 500).Value = dto.ObservacaoRegistro;
            }

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            _logger.LogInformation("Output da procedure: {Output}", output);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta da procedure de registro de bem-estar");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("Erro:"))
            {
                _logger.LogError("Erro ao inserir registro de bem-estar: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            var match = System.Text.RegularExpressions.Regex.Match(output, @"ID:\s*(\d+)");
            var registroId = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return Results.Created($"/api/bem-estar/{registroId}", new
            {
                success = true,
                message = output,
                registroId = registroId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao inserir registro de bem-estar: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao inserir registro de bem-estar");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // RecomendacaoProfissional
    public async Task<IResult> CriarRecomendacaoProfissionalAsync(RecomendacaoProfissionalPostDto dto)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_recomendacao.sp_criar_recomendacao_profissional_completa(
                        :p_usuario_id,
                        :p_titulo,
                        :p_descricao,
                        :p_prompt,
                        :p_categoria,
                        :p_area,
                        :p_fonte
                    ); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = dto.UsuarioId;
            command.Parameters.Add("p_titulo", OracleDbType.Varchar2, 200).Value = dto.TituloRecomendacao;
            command.Parameters.Add("p_descricao", OracleDbType.Varchar2, 1000).Value = dto.DescricaoRecomendacao;
            command.Parameters.Add("p_prompt", OracleDbType.Varchar2, 2000).Value = dto.PromptUsado;
            command.Parameters.Add("p_categoria", OracleDbType.Varchar2, 50).Value = dto.CategoriaRecomendacao;
            command.Parameters.Add("p_area", OracleDbType.Varchar2, 100).Value = dto.AreaRecomendacao;
            command.Parameters.Add("p_fonte", OracleDbType.Varchar2, 200).Value = dto.FonteRecomendacao;

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            _logger.LogInformation("Output da procedure: {Output}", output);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta ao criar recomendação profissional");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("ERRO:"))
            {
                _logger.LogError("Erro ao criar recomendação profissional: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            var match = System.Text.RegularExpressions.Regex.Match(output, @"ID:\s*(\d+)");
            var recomendacaoId = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return Results.Created($"/api/recomendacoes/profissional/{recomendacaoId}", new
            {
                success = true,
                message = output,
                recomendacaoId = recomendacaoId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao criar recomendação profissional: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar recomendação profissional");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // RecomendaçãoSaúde
    public async Task<IResult> CriarRecomendacaoSaudeAsync(RecomendacaoSaudePostDto dto)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_recomendacao.sp_criar_recomendacao_saude_completa(
                        :p_usuario_id,
                        :p_titulo,
                        :p_descricao,
                        :p_prompt,
                        :p_tipo_saude,
                        :p_nivel_alerta,
                        :p_mensagem_saude
                    ); 
                END;",
                CommandType = CommandType.Text
            };

            command.Parameters.Add("p_usuario_id", OracleDbType.Int32).Value = dto.UsuarioId;
            command.Parameters.Add("p_titulo", OracleDbType.Varchar2, 200).Value = dto.TituloRecomendacao;
            command.Parameters.Add("p_descricao", OracleDbType.Varchar2, 1000).Value = dto.DescricaoRecomendacao;
            command.Parameters.Add("p_prompt", OracleDbType.Varchar2, 2000).Value = dto.PromptUsado;
            command.Parameters.Add("p_tipo_saude", OracleDbType.Varchar2, 50).Value = dto.TipoSaude;
            command.Parameters.Add("p_nivel_alerta", OracleDbType.Varchar2, 50).Value = dto.NivelAlerta;
            command.Parameters.Add("p_mensagem_saude", OracleDbType.Varchar2, 500).Value = dto.MensagemSaude;

            await ExecuteWithDbmsOutputAsync(connection, command);
            var output = await GetDbmsOutputAsync(connection);

            _logger.LogInformation("Output da procedure: {Output}", output);

            if (string.IsNullOrEmpty(output))
            {
                _logger.LogWarning("Nenhuma resposta ao criar recomendação de saúde");
                return Results.BadRequest(new
                {
                    success = false,
                    message = "Nenhuma resposta do banco de dados",
                    timestamp = DateTime.UtcNow
                });
            }

            if (output.Contains("ERRO:"))
            {
                _logger.LogError("Erro ao criar recomendação de saúde: {Error}", output);
                return Results.BadRequest(new
                {
                    success = false,
                    error = output,
                    timestamp = DateTime.UtcNow
                });
            }

            var match = System.Text.RegularExpressions.Regex.Match(output, @"ID:\s*(\d+)");
            var recomendacaoId = match.Success ? int.Parse(match.Groups[1].Value) : 0;

            return Results.Created($"/api/recomendacoes/saude/{recomendacaoId}", new
            {
                success = true,
                message = output,
                recomendacaoId = recomendacaoId,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao criar recomendação de saúde: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar recomendação de saúde");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

    // Exportação
    public async Task<IResult> ExportarDatasetUsuariosAsync()
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync();

            using var command = new OracleCommand
            {
                Connection = connection,
                CommandText = @"BEGIN 
                    pkg_export.sp_exportar_dataset_usuarios(:p_arquivo); 
                END;",
                CommandType = CommandType.Text
            };

            var arquivoParam = new OracleParameter("p_arquivo", OracleDbType.Clob)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(arquivoParam);

            // Habilita DBMS_OUTPUT para capturar mensagens de erro
            using var enableCmd = new OracleCommand("BEGIN DBMS_OUTPUT.ENABLE(1000000); END;", connection);
            await enableCmd.ExecuteNonQueryAsync();

            await command.ExecuteNonQueryAsync();

            var mensagensErro = await GetDbmsOutputAsync(connection);

            if (!string.IsNullOrEmpty(mensagensErro) && mensagensErro.Contains("ERRO"))
            {
                _logger.LogError("Erro ao exportar dataset: {Error}", mensagensErro);
                return Results.BadRequest(new
                {
                    success = false,
                    error = mensagensErro,
                    timestamp = DateTime.UtcNow
                });
            }

            var jsonDataset = ((OracleClob)arquivoParam.Value).Value;

            if (string.IsNullOrEmpty(jsonDataset))
            {
                _logger.LogWarning("Nenhum dado encontrado para exportação");
                return Results.NotFound(new
                {
                    success = false,
                    message = "Nenhum usuário encontrado para exportação",
                    timestamp = DateTime.UtcNow
                });
            }

            // Valida se é um JSON válido
            var usuarios = JsonSerializer.Deserialize<List<JsonElement>>(jsonDataset);

            return Results.Ok(new
            {
                success = true,
                totalUsuarios = usuarios?.Count ?? 0,
                data = usuarios,
                timestamp = DateTime.UtcNow
            });
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "Erro Oracle ao exportar dataset de usuários: {Message}", ex.Message);
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro no banco de dados Oracle"
            );
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Erro ao deserializar JSON do dataset");
            return Results.Problem(
                detail: "JSON inválido retornado do banco de dados",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro ao processar dados"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao exportar dataset");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Erro interno no servidor"
            );
        }
    }

private async Task ExecuteWithDbmsOutputAsync(OracleConnection connection, OracleCommand command)
    {
        using var enableCmd = new OracleCommand("BEGIN DBMS_OUTPUT.ENABLE(1000000); END;", connection);
        await enableCmd.ExecuteNonQueryAsync();

        await command.ExecuteNonQueryAsync();
    }

    private async Task<string> GetDbmsOutputAsync(OracleConnection connection)
    {
        var output = new StringBuilder();

        using var getLineCmd = new OracleCommand
        {
            Connection = connection,
            CommandText = "BEGIN DBMS_OUTPUT.GET_LINE(:line, :status); END;",
            CommandType = CommandType.Text
        };

        var lineParam = new OracleParameter("line", OracleDbType.Varchar2, 32767)
        {
            Direction = ParameterDirection.Output
        };
        var statusParam = new OracleParameter("status", OracleDbType.Int32)
        {
            Direction = ParameterDirection.Output
        };

        getLineCmd.Parameters.Add(lineParam);
        getLineCmd.Parameters.Add(statusParam);

        while (true)
        {
            await getLineCmd.ExecuteNonQueryAsync();

            var status = Convert.ToInt32(statusParam.Value.ToString());
            if (status != 0) break;

            var line = lineParam.Value?.ToString();
            if (!string.IsNullOrEmpty(line))
            {
                output.AppendLine(line);
            }
        }

        return output.ToString().Trim();
    }
}




