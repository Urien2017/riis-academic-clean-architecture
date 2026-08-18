using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class PaiementScolariteConfiguration : IEntityTypeConfiguration<PaiementScolarite>
{
    public void Configure(EntityTypeBuilder<PaiementScolarite> builder)
    {
        builder.ToTable("PaiementsScolarite", table =>
            table.HasCheckConstraint(
                "CK_PaiementsScolarite_Montants",
                "[Montant] >= 0 AND [MontantAffecte] >= 0 AND [MontantNonAffecte] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Montant).HasPrecision(18, 2);
        builder.Property(x => x.ModePaiement).HasMaxLength(50);
        builder.Property(x => x.ReferencePaiement).HasMaxLength(100);
        builder.Property(x => x.EncaissePar).HasMaxLength(150);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.Property(x => x.MontantAffecte).HasPrecision(18, 2);
        builder.Property(x => x.MontantNonAffecte).HasPrecision(18, 2);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.DossierScolariteId, x.DatePaiement });
        builder.HasIndex(x => new { x.DossierScolariteId, x.TypeElementScolariteId });
        builder.HasIndex(x => x.ModePaiementScolariteId);
        builder.HasIndex(x => x.ReferencePaiement);

        builder.HasOne(x => x.DossierScolarite)
            .WithMany(x => x.Paiements)
            .HasForeignKey(x => x.DossierScolariteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ElementScolariteEtudiant)
            .WithMany(x => x.Paiements)
            .HasForeignKey(x => x.ElementScolariteEtudiantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TypeElementScolarite)
            .WithMany()
            .HasForeignKey(x => x.TypeElementScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TarifScolarite)
            .WithMany()
            .HasForeignKey(x => x.TarifScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ModePaiementScolarite)
            .WithMany(x => x.Paiements)
            .HasForeignKey(x => x.ModePaiementScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
