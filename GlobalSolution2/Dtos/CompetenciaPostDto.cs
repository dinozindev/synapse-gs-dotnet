using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de uma Competência")]
public record CompetenciaPostDto
(
    [property: Description("Nome da competência")]
    string NomeCompetencia,
    [property: Description("Categoria da competência")]
    string CategoriaCompetencia,
    [property: Description("Descrição detalhada da competência")]
    string DescricaoCompetencia
){}