using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using GlobalSolution2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GlobalSolution2.Tests.Unit
{
    public class UsuarioServiceTests
    {
        private readonly Mock<ILogger<UsuarioService>> _mockLogger;
        private readonly AppDbContext _db;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<UsuarioService>>();
            _service = new UsuarioService(_db, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllUsuariosAsync_DeveRetornarUsuariosPaginados()
        {
            // Arrange
            _db.Usuarios.AddRange(
                new Usuario { NomeUsuario = "Usuario1", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" },
                new Usuario { NomeUsuario = "Usuario2", SenhaUsuario = "senha223", AreaAtual = "RH", AreaInteresse = "Area Interesse 2", ObjetivoCarreira = "Objetivo Carreira 2", NivelExperiencia = "Nenhuma" },
                new Usuario { NomeUsuario = "Usuario3", SenhaUsuario = "senha323", AreaAtual = "Vendas", AreaInteresse = "Area Interesse 3", ObjetivoCarreira = "Objetivo Carreira 3", NivelExperiencia = "Nenhuma" }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetAllUsuariosAsync(pageNumber: 1, pageSize: 2);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<UsuarioReadDto>>>(result);
            Assert.NotNull(okResult.Value);
            Assert.Equal(3, okResult.Value.TotalCount);
            Assert.Equal(2, okResult.Value.Data.Count());
        }

        [Fact]
        public async Task GetAllUsuariosAsync_DeveRetornarNoContent_QuandoNaoHaUsuarios()
        {
            // Act
            var result = await _service.GetAllUsuariosAsync();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_DeveRetornarUsuario_QuandoExistir()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "Teste", 
                SenhaUsuario = "senha123", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma"
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetUsuarioByIdAsync(usuario.UsuarioId);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<UsuarioReadDto>>>(result);
            Assert.Equal("Teste", okResult.Value?.Data.NomeUsuario);
        }

        [Fact]
        public async Task GetUsuarioByIdAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Act
            var result = await _service.GetUsuarioByIdAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task CreateUsuarioAsync_DeveCriarUsuarioComSucesso()
        {
            // Arrange
            var dto = new UsuarioPostDto
            (
                NomeUsuario : "NovoUsuario",
                SenhaUsuario : "senha123",
                AreaAtual : "TI",
                AreaInteresse : "Desenvolvimento",
                ObjetivoCarreira : "Crescer na carreira",
                NivelExperiencia : "Júnior"
            );

            // Act
            var result = await _service.CreateUsuarioAsync(dto);

            // Assert
            var createdResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Created<ResourceResponse<UsuarioReadDto>>>(result);
            Assert.NotNull(createdResult.Value);
            Assert.Equal("NovoUsuario", createdResult.Value.Data.NomeUsuario);
            Assert.Equal(1, await _db.Usuarios.CountAsync());
        }

        [Fact]
        public async Task UpdateUsuarioAsync_DeveAtualizarUsuarioComSucesso()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "Original", 
                SenhaUsuario = "senha123", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma" 
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var dto = new UsuarioPostDto
            (
                NomeUsuario : "Atualizado",
                SenhaUsuario : "novaSenha",
                AreaAtual : "RH",
                AreaInteresse : "Gestão",
                ObjetivoCarreira : "Gerente",
                NivelExperiencia : "Sênior"
            );

            // Act
            var result = await _service.UpdateUsuarioAsync(usuario.UsuarioId, dto);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<UsuarioReadDto>>>(result);
            Assert.Equal("Atualizado", okResult.Value?.Data.NomeUsuario);
            
            var usuarioAtualizado = await _db.Usuarios.FindAsync(usuario.UsuarioId);
            Assert.Equal("Atualizado", usuarioAtualizado?.NomeUsuario);
            Assert.Equal("RH", usuarioAtualizado?.AreaAtual);
        }

        [Fact]
        public async Task UpdateUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var dto = new UsuarioPostDto
            (
                NomeUsuario : "Teste",
                SenhaUsuario : "senha123",
                AreaAtual : "TI",
                AreaInteresse : "Area Interesse 1", 
                ObjetivoCarreira : "Objetivo Carreira 1", 
                NivelExperiencia : "Nenhuma" 
            );

            // Act
            var result = await _service.UpdateUsuarioAsync(999, dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DeveRemoverUsuarioComSucesso()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "ParaRemover", 
                SenhaUsuario = "senha123", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma" 
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.DeleteUsuarioAsync(usuario.UsuarioId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(0, await _db.Usuarios.CountAsync());
        }

        [Fact]
        public async Task DeleteUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Act
            var result = await _service.DeleteUsuarioAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task AddCompetenciaAoUsuarioAsync_DeveAssociarComSucesso()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "Usuario", 
                SenhaUsuario = "senha123", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma" 
            };
            var competencia = new Competencia 
            { 
                NomeCompetencia = "C#",
                CategoriaCompetencia = "Back-end", 
                DescricaoCompetencia = "Linguagem de programação" 
            };
            
            _db.Usuarios.Add(usuario);
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.AddCompetenciaAoUsuarioAsync(usuario.UsuarioId, competencia.CompetenciaId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(1, await _db.UsuarioCompetencias.CountAsync());
        }

        [Fact]
        public async Task AddCompetenciaAoUsuarioAsync_DeveRetornarConflict_QuandoAssociacaoJaExistir()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "Usuario", 
                SenhaUsuario = "senha", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma" 
            };
            var competencia = new Competencia 
            { 
                NomeCompetencia = "C#",
                CategoriaCompetencia = "Back-end", 
                DescricaoCompetencia = "Linguagem de programação" 
            };
            
            _db.Usuarios.Add(usuario);
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            var associacao = new UsuarioCompetencia
            {
                UsuarioId = usuario.UsuarioId,
                CompetenciaId = competencia.CompetenciaId
            };
            _db.UsuarioCompetencias.Add(associacao);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.AddCompetenciaAoUsuarioAsync(usuario.UsuarioId, competencia.CompetenciaId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Conflict<string>>(result);
        }

        [Fact]
        public async Task AddCompetenciaAoUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioOuCompetenciaNaoExistir()
        {
            // Act
            var result = await _service.AddCompetenciaAoUsuarioAsync(999, 999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteCompetenciaDeUsuarioAsync_DeveRemoverAssociacaoComSucesso()
        {
            // Arrange
            var usuario = new Usuario 
            { 
                NomeUsuario = "Usuario", 
                SenhaUsuario = "senha", 
                AreaAtual = "TI",
                AreaInteresse = "Area Interesse 1", 
                ObjetivoCarreira = "Objetivo Carreira 1", 
                NivelExperiencia = "Nenhuma"
            };
            var competencia = new Competencia 
            { 
                NomeCompetencia = "C#",
                CategoriaCompetencia = "Back-end", 
                DescricaoCompetencia = "Linguagem de programação" 
            };
            
            _db.Usuarios.Add(usuario);
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            var associacao = new UsuarioCompetencia
            {
                UsuarioId = usuario.UsuarioId,
                CompetenciaId = competencia.CompetenciaId
            };
            _db.UsuarioCompetencias.Add(associacao);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.DeleteCompetenciaDeUsuarioAsync(usuario.UsuarioId, competencia.CompetenciaId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(0, await _db.UsuarioCompetencias.CountAsync());
        }

        [Fact]
        public async Task DeleteCompetenciaDeUsuarioAsync_DeveRetornarNotFound_QuandoAssociacaoNaoExistir()
        {
            // Act
            var result = await _service.DeleteCompetenciaDeUsuarioAsync(999, 999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }
    }
}