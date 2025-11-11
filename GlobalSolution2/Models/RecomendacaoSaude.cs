using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution2.Models;

[Table("RECOMENDACAO_SAUDE")]
public class RecomendacaoSaude
{
    [Column("ID_RECOMENDACAO")]
    [ForeignKey("RECOMENDACAO")]
    public int RecomendacaoId { get; set; }

    [Column("TIPO_SAUDE")]
    [StringLength(50)]
    public required string TipoSaude { get; set; }

    [Column("NIVEL_ALERTA")]
    [StringLength(50)]
    public required string NivelAlerta { get; set; }

    [Column("MENSAGEM_SAUDE")]
    [StringLength(1000)]
    public required string MensagemSaude { get; set; }

    public required Recomendacao Recomendacao { get; set; }

}