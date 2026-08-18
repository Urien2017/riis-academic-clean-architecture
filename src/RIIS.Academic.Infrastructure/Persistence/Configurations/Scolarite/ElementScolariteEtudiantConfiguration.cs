using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class ElementScolariteEtudiantConfiguration : IEntityTypeConfiguration<ElementScolariteEtudiant>
{
    public void Configure(EntityTypeBuilder<ElementScolariteEtudiant> builder)
    {
        builder.ToTable("ElementsScolariteEtudiants", table =>
            table.HasCheckConstraint(
                "CK_ElementsScolariteEtudiants_Montants",
                "[MontantAttendu] >= 0 AND [MontantAffecte] >= 0 AND [MontantRestant] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(80);
        builder.Property(x => x.Libelle).HasMaxLength(200);
        builder.Property(x => x.MontantAttendu).HasPrecision(18, 2);
        builder.Property(x => x.MontantAffecte).HasPrecision(18, 2);
        builder.Property(x => x.MontantRestant).HasPrecision(18, 2);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.DossierScolariteId, x.TypeElementScolariteId }).IsUnique();
        builder.HasIndex(x => new { x.DossierScolariteId, x.Statut });

        builder.HasOne(x => x.DossierScolarite)
            .WithMany(x => x.ElementsScolarite)
            .HasForeignKey(x => x.DossierScolariteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.TypeElementScolarite)
            .WithMany(x => x.ElementsEtudiants)
            .HasForeignKey(x => x.TypeElementScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
