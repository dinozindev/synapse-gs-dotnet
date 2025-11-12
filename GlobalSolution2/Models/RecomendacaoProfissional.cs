using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution2.Models;

[Table("RECOMENDACAO_PROFISSIONAL")]
public class RecomendacaoProfissional : Recomendacao
{
    [Column("CATEGORIA_RECOMENDACAO")]
    [StringLength(50)]
    public required string CategoriaRecomendacao { get; set; }

    [Column("AREA_RECOMENDACAO")]
    [StringLength(100)]
    public required string AreaRecomendacao { get; set; }

    [Column("FONTE_RECOMENDACAO")]
    [StringLength(100)]
    public required string FonteRecomendacao { get; set; }
}
