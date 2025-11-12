using System.ComponentModel;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de Registro de Bem Estar")]
public record RegistroBemEstarReadDto(
    [property: Description("Identificador único do Registro")]
    int RegistroId,
    [property: Description("Data do Registro")]
    DateTime DataRegistro,
    [property: Description("Humor do usuário")]
    string HumorRegistro,
    [property: Description("Horas de sono do usuário")]
    int HorasSono,
    [property: Description("Horas de trabalho do usuário")]
    int HorasTrabalho,
    [property: Description("Nível de energia do usuário (1 a 10)")]
    int NivelEnergia,
    [property: Description("Nível de estresse do usuário (1 a 10)")]
    int NivelEstresse,
    [property: Description("Observação adicional sobre o bem estar do usuário")]
    string? ObservacaoRegistro,
    [property: Description("Usuário que realizou o Registro de Bem Estar")]
    UsuarioResumoDto Usuario
)
{
    public static RegistroBemEstarReadDto ToDto(RegistroBemEstar rbe) =>
        new(
            rbe.RegistroId,
            rbe.DataRegistro,
            rbe.HumorRegistro,
            rbe.HorasSono,
            rbe.HorasTrabalho,
            rbe.NivelEnergia,
            rbe.NivelEstresse,
            rbe.ObservacaoRegistro,
            UsuarioResumoDto.ToDto(rbe.Usuario)
        );
};