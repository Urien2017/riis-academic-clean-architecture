using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class EcheanceScolariteConfiguration : IEntityTypeConfiguration<EcheanceScolarite>
{
    public void Configure(EntityTypeBuilder<EcheanceScolarite> builder)
    {
        builder.ToTable("EcheancesScolarite", table =>
            table.HasCheckConstraint(
                "CK_EcheancesScolarite_Montants",
                "[MontantAttendu] >= 0 AND [MontantAffecte] >= 0 AND [MontantRestant] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(200);
        builder.Property(x => x.MontantAttendu).HasPrecision(18, 2);
        builder.Property(x => x.MontantAffecte).HasPrecision(18, 2);
        builder.Property(x => x.MontantRestant).HasPrecision(18, 2);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.ElementScolariteEtudiantId, x.Numero }).IsUnique();
        builder.HasIndex(x => new { x.DateExigibilite, x.Statut });

        builder.HasOne(x => x.ElementScolariteEtudiant)
            .WithMany(x => x.Echeances)
            .HasForeignKey(x => x.ElementScolariteEtudiantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
