using System.ComponentModel;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de um Usuário")]
public record UsuarioResumoDto(
    [property: Description("Identificador único do usuário")]
    int UsuarioId,
    [property: Description("Nome do usuário")]
    string NomeUsuario,
    [property: Description("Área de atuação atual do usuário")]
    string AreaAtual,
    [property: Description("Área de interesse do usuário")]
    string AreaInteresse,
    [property: Description("Objetivo de carreira profissional do usuário")]
    string ObjetivoCarreira,
    [property: Description("Nível de experiência do usuário")]
    string NivelExperiencia
)
{
    public static UsuarioResumoDto ToDto(Usuario u) =>
        new(
            u.UsuarioId,
            u.NomeUsuario,
            u.AreaAtual,
            u.AreaInteresse,
            u.ObjetivoCarreira,
            u.NivelExperiencia
        );
};