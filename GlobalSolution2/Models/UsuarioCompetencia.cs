using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalSolution2.Models;

[Table("USUARIO_COMPETENCIA")]
public class UsuarioCompetencia
{
    [Key]
    [ForeignKey("Usuario")]
    [Column("USUARIO_ID_USUARIO", Order = 0)]
    public int UsuarioId { get; set; }

    [Key]
    [ForeignKey("Competencia")]
    [Column("COMPETENCIA_ID_COMPETENCIA", Order = 1)]
    public int CompetenciaId { get; set; }

    public Usuario Usuario { get; set; } = null!;
    public Competencia Competencia { get; set; } = null!;

}