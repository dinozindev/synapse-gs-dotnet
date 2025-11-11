
using GlobalSolution2.Models;
using Microsoft.EntityFrameworkCore;

namespace GlobalSolution2;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Competencia> Competencias { get; set; }
    public DbSet<RegistroBemEstar> RegistrosBemEstar { get; set; }
    public DbSet<Recomendacao> Recomendacoes { get; set; }
    public DbSet<RecomendacaoSaude> RecomendacoesSaude { get; set; }
    public DbSet<RecomendacaoProfissional> RecomendacoesProfissionais { get; set; }
    public DbSet<UsuarioCompetencia> UsuarioCompetencias { get; set; }
    public string? DbPath { get; set; }  

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioCompetencia>()
            .HasKey(uc => new { uc.UsuarioId, uc.CompetenciaId });

        modelBuilder.Entity<UsuarioCompetencia>()
            .HasOne(uc => uc.Usuario)
            .WithMany(u => u.UsuarioCompetencias)
            .HasForeignKey(uc => uc.UsuarioId);

        modelBuilder.Entity<UsuarioCompetencia>()
            .HasOne(uc => uc.Competencia)
            .WithMany(c => c.UsuarioCompetencias)
            .HasForeignKey(uc => uc.CompetenciaId);

        // Usuários
        modelBuilder.Entity<Usuario>()
            .HasIndex(u => u.NomeUsuario)
            .IsUnique();

        modelBuilder.Entity<Usuario>()
            .Property(u => u.NivelExperiencia)
            .HasConversion<string>();

        // Competencia
        modelBuilder.Entity<Competencia>()
            .Property(c => c.CategoriaCompetencia)
            .HasConversion<string>();

        // Recomendacao
        modelBuilder.Entity<Recomendacao>()
            .HasKey(r => r.RecomendacaoId);

        modelBuilder.Entity<Recomendacao>()
            .HasOne<Usuario>()
            .WithMany(u => u.Recomendacoes)
            .HasForeignKey(r => r.UsuarioId);
        
        // RecomendacaoProfissional
        modelBuilder.Entity<RecomendacaoProfissional>()
            .HasKey(rp => rp.RecomendacaoId);

        modelBuilder.Entity<RecomendacaoProfissional>()
            .Property(rp => rp.CategoriaRecomendacao)
            .HasConversion<string>();

        modelBuilder.Entity<RecomendacaoProfissional>()
            .Property(rp => rp.AreaRecomendacao)
            .HasConversion<string>();

        modelBuilder.Entity<RecomendacaoProfissional>()
            .HasOne<Recomendacao>()
            .WithOne(r => r.RecomendacaoProfissional)
            .HasForeignKey<RecomendacaoProfissional>(rp => rp.RecomendacaoId);

        // RecomendacaoSaude
        modelBuilder.Entity<RecomendacaoSaude>()
            .HasKey(rs => rs.RecomendacaoId);

        modelBuilder.Entity<RecomendacaoSaude>()
            .Property(rs => rs.TipoSaude)
            .HasConversion<string>();

        modelBuilder.Entity<RecomendacaoSaude>()
            .Property(rs => rs.NivelAlerta)
            .HasConversion<string>();

        modelBuilder.Entity<RecomendacaoSaude>()
            .HasOne<Recomendacao>()
            .WithOne(r => r.RecomendacaoSaude)
            .HasForeignKey<RecomendacaoSaude>(rs => rs.RecomendacaoId);


        // RegistroBemEstar
        modelBuilder.Entity<RegistroBemEstar>()
            .HasKey(rb => rb.RegistroId);

        modelBuilder.Entity<RegistroBemEstar>()
            .Property(rb => rb.HorasSono)
            .HasPrecision(2, 0); 

        modelBuilder.Entity<RegistroBemEstar>()
            .Property(rb => rb.HorasTrabalho)
            .HasPrecision(2, 0);

        modelBuilder.Entity<RegistroBemEstar>()
            .Property(rb => rb.NivelEnergia)
            .HasPrecision(2, 0);

        modelBuilder.Entity<RegistroBemEstar>()
            .Property(rb => rb.NivelEstresse)
            .HasPrecision(2, 0);

        modelBuilder.Entity<RegistroBemEstar>()
            .HasOne<Usuario>()
            .WithMany(u => u.RegistrosBemEstar)
            .HasForeignKey(rb => rb.UsuarioId);
        }
}