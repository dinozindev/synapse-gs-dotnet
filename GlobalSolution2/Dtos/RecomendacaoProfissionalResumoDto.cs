using System.ComponentModel;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de Recomendação Profissional")]
public record RecomendacaoProfissionalResumoDto(
    [property: Description("Identificador único de Recomendação")]
    int RecomendacaoId,
    [property: Description("Data de criação da Recomendação")]
    DateTime DataRecomendacao,
    [property: Description("Título da Recomendação")]
    string TituloRecomendacao,
    [property: Description("Descrição da Recomendação")]
    string DescricaoRecomendacao,
    [property: Description("Categoria da Recomendação (curso ou vaga)")]
    string CategoriaRecomendacao,
    [property: Description("Área de Trabalho da Recomendação")]
    string AreaRecomendacao,
    [property: Description("Fonte de onde a Recomendação foi feita")]
    string FonteRecomendacao
)
{
    public static RecomendacaoProfissionalResumoDto ToDto(RecomendacaoProfissional r) =>
        new(
            r.RecomendacaoId,
            r.DataRecomendacao,
            r.TituloRecomendacao,
            r.DescricaoRecomendacao,
            r.CategoriaRecomendacao,
            r.AreaRecomendacao,
            r.FonteRecomendacao
        );
}