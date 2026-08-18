using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class ParcoursAcademiqueConfiguration : IEntityTypeConfiguration<ParcoursAcademique>
{
    public void Configure(EntityTypeBuilder<ParcoursAcademique> builder)
    {
        builder.ToTable("ParcoursAcademiques");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(60).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.SpecialiteId).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.AnneeAcademiqueId, x.CycleFormationId, x.NiveauEtudeId, x.FiliereId, x.SpecialiteId }).IsUnique();

        builder.HasOne(x => x.AnneeAcademique)
            .WithMany(x => x.ParcoursAcademiques)
            .HasForeignKey(x => x.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CycleFormation)
            .WithMany(x => x.ParcoursAcademiques)
            .HasForeignKey(x => x.CycleFormationId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.NiveauEtude)
            .WithMany(x => x.ParcoursAcademiques)
            .HasForeignKey(x => x.NiveauEtudeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Filiere)
            .WithMany(x => x.ParcoursAcademiques)
            .HasForeignKey(x => x.FiliereId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Specialite)
            .WithMany(x => x.ParcoursAcademiques)
            .HasForeignKey(x => x.SpecialiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
