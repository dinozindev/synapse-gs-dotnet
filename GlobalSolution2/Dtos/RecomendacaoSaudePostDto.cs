using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Dados de POST de Recomendação de Saúde")]
public record RecomendacaoSaudePostDto(
    [property: Description("Título da Recomendação")]
    string TituloRecomendacao,
    [property: Description("Descrição da Recomendação")]
    string DescricaoRecomendacao,
    [property: Description("Prompt utilizado para obter resposta da IA")]
    string PromptUsado,
    [property: Description("Tipo de problema de saúde")]
    string TipoSaude,
    [property: Description("Nível de alerta")]
    string NivelAlerta,
    [property: Description("Mensagem sobre o que fazer para melhorar o problema")]
    string MensagemSaude,
    [property: Description("Identificador único do Usuário")]
    int UsuarioId
);