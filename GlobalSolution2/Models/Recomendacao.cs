using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GlobalSolution2.Models;

[Table("RECOMENDACAO")]
public class Recomendacao
{
    [Key]
    [Column("ID_RECOMENDACAO")]
    public int RecomendacaoId { get; set; }

    [Column("DATA_RECOMENDACAO")]
    public required DateTime DataRecomendacao { get; set; }

    [Column("DESCRICAO_RECOMENDACAO")]
    [StringLength(1000)]
    public required string DescricaoRecomendacao { get; set; }

    [Column("PROMPT_USADO")]
    [StringLength(1000)]
    public required string PromptUsado { get; set; }

    [Column("TITULO_RECOMENDACAO")]
    [StringLength(100)]
    public required string TituloRecomendacao { get; set; }

    [Column("USUARIO_ID_USUARIO")]
    public required int UsuarioId { get; set; }

    [JsonIgnore]
    public required Usuario Usuario { get; set; }
}

