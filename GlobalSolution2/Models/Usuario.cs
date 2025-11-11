using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution2.Models;

[Table("USUARIO")]
public class Usuario
{
    [Column("ID_USUARIO")]
    public int UsuarioId { get; set; }

    [Column("NOME_USUARIO")]
    [StringLength(100)]
    public required string NomeUsuario { get; set; }

    [Column("SENHA_USUARIO")]
    [StringLength(255)]
    public required string SenhaUsuario { get; set; }

    [Column("AREA_ATUAL")]
    [StringLength(100)]
    public required string AreaAtual { get; set; }

    [Column("AREA_INTERESSE")]
    [StringLength(100)]
    public required string AreaInteresse { get; set; }

    [Column("OBJETIVO_CARREIRA")]
    [StringLength(255)]
    public required string ObjetivoCarreira { get; set; }

    [Column("NIVEL_EXPERIENCIA")]
    [StringLength(50)]
    public required string NivelExperiencia { get; set; }

    public ICollection<UsuarioCompetencia> UsuarioCompetencias { get; set; } = new List<UsuarioCompetencia>();

}
    
