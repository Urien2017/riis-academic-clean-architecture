using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class UniteEnseignementConfiguration : IEntityTypeConfiguration<UniteEnseignement>
{
    public void Configure(EntityTypeBuilder<UniteEnseignement> builder)
    {
        builder.ToTable("UnitesEnseignement", table =>
            table.HasCheckConstraint("CK_UnitesEnseignement_CreditsVolume", "[Credits] >= 0 AND [VolumeHoraire] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Credits).HasPrecision(5, 2);
        builder.HasIndex(x => new { x.SemestrePedagogiqueId, x.Code }).IsUnique();

        builder.HasOne(x => x.SemestrePedagogique)
            .WithMany(x => x.UnitesEnseignement)
            .HasForeignKey(x => x.SemestrePedagogiqueId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
