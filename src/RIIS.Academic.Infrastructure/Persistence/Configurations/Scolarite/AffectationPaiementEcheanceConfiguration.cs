using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class AffectationPaiementEcheanceConfiguration : IEntityTypeConfiguration<AffectationPaiementEcheance>
{
    public void Configure(EntityTypeBuilder<AffectationPaiementEcheance> builder)
    {
        builder.ToTable("AffectationsPaiementsEcheances", table =>
            table.HasCheckConstraint("CK_AffectationsPaiementsEcheances_Montant", "[MontantAffecte] > 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MontantAffecte).HasPrecision(18, 2);
        builder.Property(x => x.AffectePar).HasMaxLength(150);
        builder.HasIndex(x => new { x.PaiementScolariteId, x.EcheanceScolariteId });

        builder.HasOne(x => x.PaiementScolarite)
            .WithMany(x => x.Affectations)
            .HasForeignKey(x => x.PaiementScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.EcheanceScolarite)
            .WithMany(x => x.Affectations)
            .HasForeignKey(x => x.EcheanceScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
