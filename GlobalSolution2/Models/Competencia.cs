using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution2.Models;

[Table("COMPETENCIA")]
public class Competencia
{
    [Column("ID_COMPETENCIA")]
    public int CompetenciaId { get; set; }

    [Column("NOME_COMPETENCIA")]
    [StringLength(100)]
    public required string NomeCompetencia { get; set; }

    [Column("CATEGORIA_COMPETENCIA")]
    [StringLength(50)]
    public required string CategoriaCompetencia { get; set; }

    [Column("DESCRICAO_COMPETENCIA")]
    [StringLength(255)]
    public required string DescricaoCompetencia { get; set; }

    public ICollection<UsuarioCompetencia> UsuarioCompetencias { get; set; } = new List<UsuarioCompetencia>();

}