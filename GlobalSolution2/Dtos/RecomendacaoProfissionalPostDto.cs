using System.ComponentModel;

namespace GlobalSolution2.Dtos;

[Description("Dados de POST de Recomendação Profissional")]
public record RecomendacaoProfissionalPostDto(
    [property: Description("Título da Recomendação")]
    string TituloRecomendacao,
    [property: Description("Descrição da Recomendação")]
    string DescricaoRecomendacao,
    [property: Description("Prompt utilizado para obter resposta da IA")]
    string PromptUsado,
    [property: Description("Categoria da Recomendação (curso ou vaga)")]
    string CategoriaRecomendacao,
    [property: Description("Área de Trabalho da Recomendação")]
    string AreaRecomendacao,
    [property: Description("Fonte de onde a Recomendação foi feita")]
    string FonteRecomendacao,
    [property: Description("Identificador único do Usuário")]
    int UsuarioId
);