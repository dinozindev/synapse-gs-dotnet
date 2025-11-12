using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Dados de atualização de Registro de Bem Estar")]
public record RegistroBemEstarPutDto(
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
    string? ObservacaoRegistro
);