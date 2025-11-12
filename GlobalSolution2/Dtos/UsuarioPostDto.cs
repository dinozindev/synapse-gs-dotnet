using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Dados de criação de um Usuário")]
public record UsuarioPostDto(
    [property: Description("Nome de Usuário")]
    string NomeUsuario,
    [property: Description("Senha do Usuário")]
    string SenhaUsuario,
    [property: Description("Área de atuação atual do usuário")]
    string AreaAtual,
    [property: Description("Área de interesse do usuário")]
    string AreaInteresse,
    [property: Description("Objetivo de carreira profissional do usuário")]
    string ObjetivoCarreira,
    [property: Description("Nível de experiência do usuário")]
    string NivelExperiencia
);