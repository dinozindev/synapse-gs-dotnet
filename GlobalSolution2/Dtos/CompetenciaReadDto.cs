using System.ComponentModel;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de uma Competência")]
public record CompetenciaReadDto
(
    [property: Description("Identificador único da competência")]
    int CompetenciaId,
    [property: Description("Nome da competência")]
    string NomeCompetencia,
    [property: Description("Categoria da competência")]
    string CategoriaCompetencia,
    [property: Description("Descrição detalhada da competência")]
    string DescricaoCompetencia
)
{
    public static CompetenciaReadDto ToDto(Competencia c) => 
        new (
            c.CompetenciaId,
            c.NomeCompetencia,
            c.CategoriaCompetencia,
            c.DescricaoCompetencia
        );
};