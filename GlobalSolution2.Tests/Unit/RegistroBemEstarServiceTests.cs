using GlobalSolution2.Dtos;
using GlobalSolution2.Models;
using GlobalSolution2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace GlobalSolution2.Tests.Unit
{
    public class RegistroBemEstarServiceTests
    {
        private readonly Mock<ILogger<RegistroBemEstarService>> _mockLogger;
        private readonly AppDbContext _db;
        private readonly RegistroBemEstarService _service;

        public RegistroBemEstarServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _db = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<RegistroBemEstarService>>();
            _service = new RegistroBemEstarService(_db, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllRegistrosBemEstarAsync_DeveRetornarPaginado()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Usuario1", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            _db.RegistrosBemEstar.AddRange(
                new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now, HumorRegistro = "Feliz", HorasSono = 8, HorasTrabalho = 6, NivelEnergia = 7, NivelEstresse = 4, ObservacaoRegistro = "Observação Registro 1"  },
                new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now.AddDays(-1), HumorRegistro = "Cansado", HorasSono = 9, HorasTrabalho = 6, NivelEnergia = 8, NivelEstresse = 3, ObservacaoRegistro = "Observação Registro 2" },
                new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now.AddDays(-2), HumorRegistro = "Animado", HorasSono = 4, HorasTrabalho = 9, NivelEnergia = 3, NivelEstresse = 9, ObservacaoRegistro = "Observação Registro 3" }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetAllRegistrosBemEstarAsync(pageNumber: 1, pageSize: 2);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<RegistroBemEstarReadDto>>>(result);
            Assert.Equal(3, ok.Value?.TotalCount);
            Assert.Equal(2, ok.Value?.Data.Count());
        }

        [Fact]
        public async Task GetAllRegistrosBemEstarAsync_DeveRetornarNoContent_QuandoNaoExistirem()
        {
            var result = await _service.GetAllRegistrosBemEstarAsync();
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
        }

        [Fact]
        public async Task GetRegistrosDeUsuarioAsync_DeveRetornarPaginado()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "CarlosMarcos", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            _db.RegistrosBemEstar.AddRange(
                 new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now, HumorRegistro = "Feliz", HorasSono = 8, HorasTrabalho = 6, NivelEnergia = 7, NivelEstresse = 4, ObservacaoRegistro = "Observação Registro 1"  },
                    new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now.AddDays(-1), HumorRegistro = "Cansado", HorasSono = 9, HorasTrabalho = 6, NivelEnergia = 8, NivelEstresse = 3, ObservacaoRegistro = "Observação Registro 2" }
            );
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetRegistrosDeUsuarioAsync(usuario.UsuarioId);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<PagedResponse<RegistroBemEstarResumoDto>>>(result);
            Assert.Equal(2, ok.Value?.TotalCount);
        }

        [Fact]
        public async Task GetRegistrosDeUsuarioAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            var result = await _service.GetRegistrosDeUsuarioAsync(999);
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task GetRegistroBemEstarByIdAsync_DeveRetornarRegistro()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "PedroDosSantos", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var registro = new RegistroBemEstar { UsuarioId = usuario.UsuarioId, Usuario = usuario, DataRegistro = DateTime.Now, HumorRegistro = "Feliz", HorasSono = 8, HorasTrabalho = 6, NivelEnergia = 7, NivelEstresse = 4, ObservacaoRegistro = "Observação Registro 1"  };
            _db.RegistrosBemEstar.Add(registro);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.GetRegistroBemEstarByIdAsync(registro.RegistroId);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<RegistroBemEstarReadDto>>>(result);
            Assert.Equal("Feliz", ok.Value?.Data.HumorRegistro);
        }

        [Fact]
        public async Task GetRegistroBemEstarByIdAsync_DeveRetornarNotFound()
        {
            var result = await _service.GetRegistroBemEstarByIdAsync(999);
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task CreateRegistroBemEstarAsync_DeveCriarComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "LucasJunior", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var dto = new RegistroBemEstarPostDto
            (
                UsuarioId : usuario.UsuarioId,
                HumorRegistro : "Feliz",
                DataRegistro : DateTime.Now,
                HorasSono : 8,
                HorasTrabalho : 6,
                NivelEnergia : 9,
                NivelEstresse : 3,
                ObservacaoRegistro : ""
            );

            // Act
            var result = await _service.CreateRegistroBemEstarAsync(dto);

            // Assert
            var created = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Created<ResourceResponse<RegistroBemEstarReadDto>>>(result);
            Assert.Equal("Feliz", created.Value?.Data.HumorRegistro);
            Assert.Equal(1, await _db.RegistrosBemEstar.CountAsync());
        }

        [Fact]
        public async Task CreateRegistroBemEstarAsync_DeveRetornarNotFound_QuandoUsuarioNaoExistir()
        {
            var dto = new RegistroBemEstarPostDto
            (
                UsuarioId : 999,
                HumorRegistro : "Triste",
                DataRegistro : DateTime.Now,
                HorasSono : 5,
                HorasTrabalho : 9,
                NivelEnergia : 4,
                NivelEstresse : 7,
                ObservacaoRegistro : ""
            );

            var result = await _service.CreateRegistroBemEstarAsync(dto);
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task UpdateRegistroBemEstarAsync_DeveAtualizarComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "LucasJunior", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var registro = new RegistroBemEstar
            {
                UsuarioId = usuario.UsuarioId,
                Usuario = usuario,
                HumorRegistro = "Feliz",
                DataRegistro = DateTime.Now,
                HorasSono = 8,
                HorasTrabalho = 6,
                NivelEnergia = 9,
                NivelEstresse = 3,
                ObservacaoRegistro = ""
            };
            _db.RegistrosBemEstar.Add(registro);
            await _db.SaveChangesAsync();

            var dto = new RegistroBemEstarPutDto
            (
                HumorRegistro : "Calmo",
                HorasSono : 8,
                HorasTrabalho : 6,
                NivelEnergia : 9,
                NivelEstresse : 2,
                ObservacaoRegistro : ""
            );

            // Act
            var result = await _service.UpdateRegistroBemEstarAsync(registro.RegistroId, dto);

            // Assert
            var ok = Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.Ok<ResourceResponse<RegistroBemEstarReadDto>>>(result);
            Assert.Equal("Calmo", ok.Value?.Data.HumorRegistro);
        }

        [Fact]
        public async Task UpdateRegistroBemEstarAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            var dto = new RegistroBemEstarPutDto
            (
                HumorRegistro : "Calmo",
                HorasSono : 7,
                HorasTrabalho : 8,
                NivelEnergia : 6,
                NivelEstresse : 5,
                ObservacaoRegistro : ""
            );

            var result = await _service.UpdateRegistroBemEstarAsync(999, dto);
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }

        [Fact]
        public async Task DeleteRegistroBemEstarAsync_DeveRemoverComSucesso()
        {
            // Arrange
            var usuario = new Usuario { NomeUsuario = "Marco Antonio", SenhaUsuario = "senha123", AreaAtual = "TI", AreaInteresse = "Area Interesse 1", ObjetivoCarreira = "Objetivo Carreira 1", NivelExperiencia = "Nenhuma" };
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();

            var registro = new RegistroBemEstar
            {
                UsuarioId = usuario.UsuarioId,
                Usuario = usuario,
                HumorRegistro = "Feliz",
                DataRegistro = DateTime.Now,
                HorasSono = 8,
                HorasTrabalho = 6,
                NivelEnergia = 9,
                NivelEstresse = 3,
                ObservacaoRegistro = ""
            };
            _db.RegistrosBemEstar.Add(registro);
            await _db.SaveChangesAsync();

            // Act
            var result = await _service.DeleteRegistroBemEstarAsync(registro.RegistroId);

            // Assert
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NoContent>(result);
            Assert.Equal(0, await _db.RegistrosBemEstar.CountAsync());
        }

        [Fact]
        public async Task DeleteRegistroBemEstarAsync_DeveRetornarNotFound_QuandoNaoExistir()
        {
            var result = await _service.DeleteRegistroBemEstarAsync(999);
            Assert.IsType<Microsoft.AspNetCore.Http.HttpResults.NotFound<string>>(result);
        }
    }
}
