using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GlobalSolution2.Models;

    [Table("REGISTRO_BEM_ESTAR")]
    public class RegistroBemEstar
    {
    [Column("ID_REGISTRO")]
    public int RegistroId { get; set; }
        
    [Column("DATA_REGISTRO")]
    public required DateTime DataRegistro { get; set; }

    [Column("HUMOR_REGISTRO")]
    [StringLength(20)]
    public required string HumorRegistro { get; set; }

    [Column("HORAS_SONO")]
    [Range(0, 24)]
    public required int HorasSono { get; set; }

    [Column("HORAS_TRABALHO")]
    [Range(0, 24)]
    public required int HorasTrabalho { get; set; }

    [Column("NIVEL_ENERGIA")]
    [Range(1, 10)]
    public required int NivelEnergia { get; set; }

    [Column("NIVEL_ESTRESSE")]
    [Range(1, 10)]
    public required int NivelEstresse { get; set; }

    [Column("OBSERVACAO_REGISTRO")]
    [StringLength(255)]
    public string? ObservacaoRegistro { get; set; }

    [Column("USUARIO_ID_USUARIO")]
    public required int UsuarioId { get; set; }

    [JsonIgnore]
    public required Usuario Usuario { get; set; }
    }
