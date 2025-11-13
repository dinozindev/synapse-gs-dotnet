using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using GlobalSolution2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GlobalSolution2.Tests.Unit
{
    public class RecomendacaoProfissionalServiceTests
    {
        private readonly Mock<ILogger<RecomendacaoProfissionalService>> _mockLogger;
        private readonly AppDbContext _db;
        private readonly RecomendacaoProfissionalService _service;

        public RecomendacaoProfissionalServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<RecomendacaoProfissionalService>>();
            _service = new RecomendacaoProfissionalService(_db, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllRecomendacoesAsync_DeveRetornarPaginado()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario1", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            _db.RecomendacoesProfissionais.AddRange(
                new RecomendacaoProfissional
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario = usuario,
                    DataRecomendacao = DateTime.UtcNow,
                    TituloRecomendacao = "Rec 1",
                    DescricaoRecomendacao = "Desc 1",
                    PromptUsado = "Prompt",
                    CategoriaRecomendacao = "Curso",
                    AreaRecomendacao = "TI",
                    FonteRecomendacao = "Fonte A"
                },
                new RecomendacaoProfissional
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario = usuario,
                    DataRecomendacao = DateTime.UtcNow.AddDays(-1),
                    TituloRecomendacao = "Rec 2",
                    DescricaoRecomendacao = "Desc 2",
                    PromptUsado = "Prompt",
                    CategoriaRecomendacao = "Vaga",
                    AreaRecomendacao = "TI",
                    FonteRecomendacao = "Fonte B"
                },
                new RecomendacaoProfissional
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario = usuario,
                    DataRecomendacao = DateTime.UtcNow.AddDays(-2),
                    TituloRecomendacao = "Rec 3",
                    DescricaoRecomendacao = "Desc 3",
                    PromptUsado = "Prompt",
                    CategoriaRecomendacao = "Curso",
                    AreaRecomendacao = "RH",
                    FonteRecomendacao = "Fonte C"
                }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetAllRecomendacoesAsync(pageNumber: 1, pageSize: 2);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<RecomendacaoProfissionalReadDto>>>(result);
            Assert.Equal(3, ok.Value?.TotalCount);
            Assert.Equal(2, ok.Value?.Data.Count());
        }

        [Fact]
        public async Task GetAllRecomendacoesAsync_DeveRetornarNoContent_QuandoNaoExistirem()
        {
            // Act
            var result = await _service.GetAllRecomendacoesAsync();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
        }

        [Fact]
        public async Task GetRecomendacaoByIdAsync_DeveRetornarRecomendacao_QuandoExistir()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario2", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var rec = new RecomendacaoProfissional
            {
                UsuarioId = usuario.UsuarioId,
                Usuario = usuario,
                DataRecomendacao = DateTime.UtcNow,
                TituloRecomendacao = "Recomendacao X",
                DescricaoRecomendacao = "Descricao X",
                PromptUsado = "PromptX",
                CategoriaRecomendacao = "Curso",
                AreaRecomendacao = "Marketing",
                FonteRecomendacao = "Blog"
            };
            _db.RecomendacoesProfissionais.Add(rec);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacaoByIdAsync(rec.RecomendacaoId);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<RecomendacaoProfissionalReadDto>>>(result);
            Assert.Equal("Recomendacao X", ok.Value?.Data.TituloRecomendacao);
        }

        [Fact]
        public async Task GetRecomendacaoByIdAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Act
            var result = await _service.GetRecomendacaoByIdAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarPaginado_QuandoExistirem()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario3", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            _db.RecomendacoesProfissionais.AddRange(
                new RecomendacaoProfissional
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario = usuario,
                    DataRecomendacao = DateTime.UtcNow,
                    TituloRecomendacao = "R1",
                    DescricaoRecomendacao = "D1",
                    PromptUsado = "P",
                    CategoriaRecomendacao = "Curso",
                    AreaRecomendacao = "TI",
                    FonteRecomendacao = "Fonte1"
                },
                new RecomendacaoProfissional
                {
                    UsuarioId = usuario.UsuarioId,
                    Usuario = usuario,
                    DataRecomendacao = DateTime.UtcNow.AddDays(-1),
                    TituloRecomendacao = "R2",
                    DescricaoRecomendacao = "D2",
                    PromptUsado = "P",
                    CategoriaRecomendacao = "Vaga",
                    AreaRecomendacao = "TI",
                    FonteRecomendacao = "Fonte2"
                }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(usuario.UsuarioId);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<RecomendacaoProfissionalResumoDto>>>(result);
            Assert.Equal(2, ok.Value?.TotalCount);
            Assert.Equal(2, ok.Value?.Data.Count());
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarNoContent_QuandoUsuarioSemRecomendacoes()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario4", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(usuario.UsuarioId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
        }

        [Fact]
        public async Task GetRecomendacoesByUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Act
            var result = await _service.GetRecomendacoesByUsuarioAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task CreateRecomendacaoProfissionalAsync_DeveCriarComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario5", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var dto = new RecomendacaoProfissionalPostDto(
                TituloRecomendacao: "Titulo Teste",
                DescricaoRecomendacao: "Descricao Teste",
                PromptUsado: "Prompt Teste",
                CategoriaRecomendacao: "Curso",
                AreaRecomendacao: "Desenvolvimento",
                FonteRecomendacao: "Fonte X",
                UsuarioId: usuario.UsuarioId
            );

            // Act
            var result = await _service.CreateRecomendacaoProfissionalAsync(dto);

            // Assert
            var created = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Created<ResourceResponse<RecomendacaoProfissionalReadDto>>>(result);
            Assert.Equal("Titulo Teste", created.Value?.Data.TituloRecomendacao);
            Assert.Equal(1, await _db.RecomendacoesProfissionais.CountAsync());
        }

        [Fact]
        public async Task CreateRecomendacaoProfissionalAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var dto = new RecomendacaoProfissionalPostDto(
                TituloRecomendacao: "Titulo Teste",
                DescricaoRecomendacao: "Descricao Teste",
                PromptUsado: "Prompt Teste",
                CategoriaRecomendacao: "Curso",
                AreaRecomendacao: "Desenvolvimento",
                FonteRecomendacao: "Fonte X",
                UsuarioId: 999
            );

            // Act
            var result = await _service.CreateRecomendacaoProfissionalAsync(dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteRecomendacaoProfissionalAsync_DeveRemoverComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario6", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var rec = new RecomendacaoProfissional
            {
                UsuarioId = usuario.UsuarioId,
                Usuario = usuario,
                DataRecomendacao = DateTime.UtcNow,
                TituloRecomendacao = "ParaRemover",
                DescricaoRecomendacao = "Desc",
                PromptUsado = "P",
                CategoriaRecomendacao = "Curso",
                AreaRecomendacao = "TI",
                FonteRecomendacao = "Fonte"
            };
            _db.RecomendacoesProfissionais.Add(rec);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.DeleteRecomendacaoProfissionalAsync(rec.RecomendacaoId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(0, await _db.RecomendacoesProfissionais.CountAsync());
        }

        [Fact]
        public async Task DeleteRecomendacaoProfissionalAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Act
            var result = await _service.DeleteRecomendacaoProfissionalAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }
    }
}
