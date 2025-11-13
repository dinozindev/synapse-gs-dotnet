using Xunit;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using GlobalSolution2.Models;
using GlobalSolution2.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using GlobalSolution2.Dtos;

namespace GlobalSolution2.Tests.Services
{
    public class RecomendacaoSaudeServiceTests
    {
        private readonly AppDbContext _context;
        private readonly RecomendacaoSaudeService _service;
        private readonly Mock<ILogger<RecomendacaoSaudeService>> _loggerMock;

        public RecomendacaoSaudeServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _loggerMock = new Mock<ILogger<RecomendacaoSaudeService>>();
            _service = new RecomendacaoSaudeService(_context, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllRecomendacoesAsync_DeveRetornarOk_QuandoExistiremRecomendacoes()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario1", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _context.Usuarios.Add(usuario);
            _context.RecomendacoesSaude.Add(new RecomendacaoSaude
            {
                RecomendacaoId = 1,
                TituloRecomendacao = "Beba água",
                DescricaoRecomendacao = "Hidrate-se bem",
                PromptUsado = "P",
                TipoSaude = "Hidratação",
                NivelAlerta = "Baixo",
                MensagemSaude = "Importante se manter hidratado",
                Usuario = usuario,
                UsuarioId = usuario.UsuarioId,
                DataRecomendacao = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllRecomendacoesAsync();

            // Assert
            var okResult = Assert.IsType<Ok<PagedResponse<RecomendacaoSaudeResumoDto>>>(result);
            Assert.NotEmpty(okResult.Value?.Data);
        }

        [Fact]
        public async Task GetAllRecomendacoesAsync_DeveRetornarNoContent_QuandoNaoExistiremRecomendacoes()
        {
            // Act
            var result = await _service.GetAllRecomendacoesAsync();

            // Assert
            Assert.IsType<NoContent>(result);
        }

        [Fact]
        public async Task GetRecomendacaoByIdAsync_DeveRetornarOk_QuandoEncontrada()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario2", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            var recomendacao = new RecomendacaoSaude
            {
                RecomendacaoId = 2,
                TituloRecomendacao = "Durma bem",
                DescricaoRecomendacao = "Durma ao menos 8 horas por noite",
                PromptUsado = "",
                TipoSaude = "Sono",
                NivelAlerta = "Moderado",
                MensagemSaude = "A falta de sono prejudica a saúde",
                Usuario = usuario,
                UsuarioId = usuario.UsuarioId,
                DataRecomendacao = DateTime.UtcNow
            };
            _context.Usuarios.Add(usuario);
            _context.RecomendacoesSaude.Add(recomendacao);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacaoByIdAsync(2);

            // Assert
            var okResult = Assert.IsType<Ok<ResourceResponse<RecomendacaoSaudeReadDto>>>(result);
            Assert.Equal("Durma bem", okResult.Value?.Data.TituloRecomendacao);
        }

        [Fact]
        public async Task GetRecomendacaoByIdAsync_DeveRetornarNotFound_QuandoNaoEncontrada()
        {
            // Act
            var result = await _service.GetRecomendacaoByIdAsync(99);

            // Assert
            Assert.IsType<NotFound<string>>(result);
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarOk_QuandoUsuarioExistirComRecomendacoes()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario3", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _context.Usuarios.Add(usuario);
            _context.RecomendacoesSaude.AddRange(
                new RecomendacaoSaude
                {
                    RecomendacaoId = 3,
                    TituloRecomendacao = "Faça exercícios",
                    DescricaoRecomendacao = "30 minutos por dia",
                    PromptUsado = "PPPPP",
                    TipoSaude = "Sono",
                    NivelAlerta = "Baixo",
                    MensagemSaude = "Manter o corpo ativo é essencial",
                    Usuario = usuario,
                    UsuarioId = usuario.UsuarioId,
                    DataRecomendacao = DateTime.UtcNow
                });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(usuario.UsuarioId);

            // Assert
            var okResult = Assert.IsType<Ok<PagedResponse<RecomendacaoSaudeReadDto>>>(result);
            Assert.Single(okResult.Value?.Data);
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(99);

            // Assert
            Assert.IsType<NotFound<string>>(result);
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarNoContent_QuandoNaoHouverRecomendacoes()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario4", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(usuario.UsuarioId);

            // Assert
            Assert.IsType<NoContent>(result);
        }

        [Fact]
        public async Task CreateRecomendacaoSaudeAsync_DeveCriarRecomendacaoComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario5", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var dto = new RecomendacaoSaudePostDto
            (
                UsuarioId : usuario.UsuarioId,
                TituloRecomendacao : "Evite café à noite",
                DescricaoRecomendacao : "A cafeína pode atrapalhar o sono",
                PromptUsado : "Sono e cafeína",
                TipoSaude : "Sono",
                NivelAlerta : "Moderado",
                MensagemSaude : "Evite bebidas com cafeína após as 18h"
            );

            // Act
            var result = await _service.CreateRecomendacaoSaudeAsync(dto);

            // Assert
            var createdResult = Assert.IsType<Created<ResourceResponse<RecomendacaoSaudeReadDto>>>(result);
            Assert.Equal("Evite café à noite", createdResult.Value?.Data.TituloRecomendacao);
        }

        [Fact]
        public async Task CreateRecomendacaoSaudeAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var dto = new RecomendacaoSaudePostDto
            (
                UsuarioId : 999,
                TituloRecomendacao : "Evite álcool em excesso",
                DescricaoRecomendacao : "O consumo excessivo afeta o fígado",
                PromptUsado : "P",
                TipoSaude : "Sono",
                NivelAlerta : "Alto",
                MensagemSaude : "Evite exageros para manter o equilíbrio"
            );

            // Act
            var result = await _service.CreateRecomendacaoSaudeAsync(dto);

            // Assert
            Assert.IsType<NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteRecomendacaoSaudeAsync_DeveRemoverComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario6", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            var recomendacao = new RecomendacaoSaude
            {
                RecomendacaoId = 6,
                DataRecomendacao = DateTime.UtcNow,
                TituloRecomendacao = "Reduza o açúcar",
                DescricaoRecomendacao = "Evite bebidas açucaradas",
                PromptUsado = "P",
                TipoSaude = "Sono",
                NivelAlerta = "Alto",
                MensagemSaude = "O consumo de açúcar deve ser moderado",
                Usuario = usuario,
                UsuarioId = usuario.UsuarioId
            };
            _context.Usuarios.Add(usuario);
            _context.RecomendacoesSaude.Add(recomendacao);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteRecomendacaoSaudeAsync(6);

            // Assert
            Assert.IsType<NoContent>(result);
            Assert.Empty(_context.RecomendacoesSaude.ToList());
        }

        [Fact]
        public async Task DeleteRecomendacaoSaudeAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Act
            var result = await _service.DeleteRecomendacaoSaudeAsync(999);

            // Assert
            Assert.IsType<NotFound<string>>(result);
        }
    }
}
