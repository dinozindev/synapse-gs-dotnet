using System.ComponentModel;
using GlobalSolution2.Models;

namespace GlobalSolution2.Dtos;

[Description("Dados de leitura de Recomendação de Saúde")]
public record RecomendacaoSaudeResumoDto(
    [property: Description("Identificador único de Recomendação")]
    int RecomendacaoId,
    [property: Description("Data de criação da Recomendação")]
    DateTime DataRecomendacao,
    [property: Description("Título da Recomendação")]
    string TituloRecomendacao,
    [property: Description("Descrição da Recomendação")]
    string DescricaoRecomendacao,
    [property: Description("Tipo de problema de saúde")]
    string TipoSaude,
    [property: Description("Nível de alerta")]
    string NivelAlerta,
    [property: Description("Mensagem sobre o que fazer para melhorar o problema")]
    string MensagemSaude
)
{
    public static RecomendacaoSaudeResumoDto ToDto(RecomendacaoSaude r) =>
        new(
            r.RecomendacaoId,
            r.DataRecomendacao,
            r.TituloRecomendacao,
            r.DescricaoRecomendacao,
            r.TipoSaude,
            r.NivelAlerta,
            r.MensagemSaude
        );
}