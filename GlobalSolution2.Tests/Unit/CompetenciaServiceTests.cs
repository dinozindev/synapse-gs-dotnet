using GlobalSolution2;
using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using GlobalSolution2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services
{
    public class CompetenciaServiceTests
    {
        private readonly Mock<ILogger<CompetenciaService>> _mockLogger;
        private readonly AppDbContext _db;
        private readonly CompetenciaService _service;

        public CompetenciaServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<CompetenciaService>>();
            _service = new CompetenciaService(_db, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllCompetenciasAsync_DeveRetornarCompetenciasPaginadas()
        {
            // Arrange
            _db.Competencias.AddRange(
                new Competencia 
                { 
                    NomeCompetencia = "C#", 
                    CategoriaCompetencia = "Linguagem",
                    DescricaoCompetencia = "Linguagem de programação"
                },
                new Competencia 
                { 
                    NomeCompetencia = "Java", 
                    CategoriaCompetencia = "Linguagem",
                    DescricaoCompetencia = "Linguagem de programação"
                },
                new Competencia 
                { 
                    NomeCompetencia = "SQL", 
                    CategoriaCompetencia = "Banco de Dados",
                    DescricaoCompetencia = "Linguagem de consulta"
                }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetAllCompetenciasAsync(pageNumber: 1, pageSize: 2);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<CompetenciaReadDto>>>(result);
            Assert.NotNull(okResult.Value);
            Assert.Equal(3, okResult.Value.TotalCount);
            Assert.Equal(2, okResult.Value.Data.Count());
        }

        [Fact]
        public async Task GetAllCompetenciasAsync_DeveRetornarNoContent_QuandoNaoHaCompetencias()
        {
            // Act
            var result = await _service.GetAllCompetenciasAsync();

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
        }

        [Fact]
        public async Task GetCompetenciaByIdAsync_DeveRetornarCompetencia_QuandoExistir()
        {
            // Arrange
            var competencia = new Competencia 
            { 
                NomeCompetencia = "Python", 
                CategoriaCompetencia = "Linguagem",
                DescricaoCompetencia = "Linguagem de programação"
            };
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetCompetenciaByIdAsync(competencia.CompetenciaId);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<CompetenciaReadDto>>>(result);
            Assert.Equal("Python", okResult.Value?.Data.NomeCompetencia);
        }

        [Fact]
        public async Task GetCompetenciaByIdAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            // Act
            var result = await _service.GetCompetenciaByIdAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task CreateCompetenciaParaUsuarioAsync_DeveCriarCompetenciaEAssociarComSucesso()
        {
            // Arrange
            var usuario = new Usuario
            {
                NomeUsuario = "TestUser",
                SenhaUsuario = "senha123",
                AreaAtual = "TI",
                AreaInteresse = "Desenvolvimento",
                ObjetivoCarreira = "Crescer",
                NivelExperiencia = "Júnior"
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "JavaScript",
                CategoriaCompetencia : "Linguagem",
                DescricaoCompetencia : "Linguagem de programação web"
            );

            // Act
            var result = await _service.CreateCompetenciaParaUsuarioAsync(usuario.UsuarioId, dto);

            // Assert
            var createdResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Created<ResourceResponse<CompetenciaReadDto>>>(result);
            Assert.NotNull(createdResult.Value);
            Assert.Equal("JavaScript", createdResult.Value.Data.NomeCompetencia);
            Assert.Equal(1, await _db.Competencias.CountAsync());
            Assert.Equal(1, await _db.UsuarioCompetencias.CountAsync());
        }

        [Fact]
        public async Task CreateCompetenciaParaUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            // Arrange
            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "React",
                CategoriaCompetencia : "Framework",
                DescricaoCompetencia : "Biblioteca JavaScript"
            );

            // Act
            var result = await _service.CreateCompetenciaParaUsuarioAsync(999, dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task UpdateCompetenciaAsync_DeveAtualizarCompetenciaComSucesso()
        {
            // Arrange
            var competencia = new Competencia
            {
                NomeCompetencia = "Original",
                CategoriaCompetencia = "Categoria Original",
                DescricaoCompetencia = "Descrição Original"
            };
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "Atualizado",
                CategoriaCompetencia : "Categoria Atualizada",
                DescricaoCompetencia : "Descrição Atualizada"
            );

            // Act
            var result = await _service.UpdateCompetenciaAsync(competencia.CompetenciaId, dto);

            // Assert
            var okResult = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<CompetenciaReadDto>>>(result);
            Assert.Equal("Atualizado", okResult.Value?.Data.NomeCompetencia);
            
            var competenciaAtualizada = await _db.Competencias.FindAsync(competencia.CompetenciaId);
            Assert.Equal("Atualizado", competenciaAtualizada?.NomeCompetencia);
            Assert.Equal("Categoria Atualizada", competenciaAtualizada?.CategoriaCompetencia);
        }

        [Fact]
        public async Task UpdateCompetenciaAsync_DeveRetornarNotFound_QuandoCompetenciaNaoExistir()
        {
            // Arrange
            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "Teste",
                CategoriaCompetencia : "Categoria",
                DescricaoCompetencia : "Descrição"
            );

            // Act
            var result = await _service.UpdateCompetenciaAsync(999, dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteCompetenciaAsync_DeveRemoverCompetenciaComSucesso()
        {
            // Arrange
            var competencia = new Competencia
            {
                NomeCompetencia = "ParaRemover",
                CategoriaCompetencia = "Categoria",
                DescricaoCompetencia = "Descrição"
            };
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.DeleteCompetenciaAsync(competencia.CompetenciaId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(0, await _db.Competencias.CountAsync());
        }

        [Fact]
        public async Task DeleteCompetenciaAsync_DeveRetornarNotFound_QuandoCompetenciaNaoExistir()
        {
            // Act
            var result = await _service.DeleteCompetenciaAsync(999);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task CreateCompetenciaParaUsuarioAsync_DeveValidarCamposObrigatorios()
        {
            // Arrange
            var usuario = new Usuario
            {
                NomeUsuario = "TestUser",
                SenhaUsuario = "senha123",
                AreaAtual = "TI",
                AreaInteresse = "Desenvolvimento",
                ObjetivoCarreira = "Crescer",
                NivelExperiencia = "Júnior"
            };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "", 
                CategoriaCompetencia : "Categoria",
                DescricaoCompetencia : "Descrição"
            );

            // Act
            var result = await _service.CreateCompetenciaParaUsuarioAsync(usuario.UsuarioId, dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>>(result);
        }

        [Fact]
        public async Task UpdateCompetenciaAsync_DeveValidarCamposObrigatorios()
        {
            // Arrange
            var competencia = new Competencia
            {
                NomeCompetencia = "Original",
                CategoriaCompetencia = "Categoria",
                DescricaoCompetencia = "Descrição"
            };
            _db.Competencias.Add(competencia);
            await _db.SaveChangesAsync();

            var dto = new CompetenciaPostDto
            (
                NomeCompetencia : "", 
                CategoriaCompetencia : "Categoria",
                DescricaoCompetencia : "Descrição"
            );

            // Act
            var result = await _service.UpdateCompetenciaAsync(competencia.CompetenciaId, dto);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.BadRequest<string>>(result);
        }
    }
}