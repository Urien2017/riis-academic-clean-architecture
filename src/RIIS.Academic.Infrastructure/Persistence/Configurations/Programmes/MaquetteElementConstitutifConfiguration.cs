using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class MaquetteElementConstitutifConfiguration : IEntityTypeConfiguration<MaquetteElementConstitutif>
{
    public void Configure(EntityTypeBuilder<MaquetteElementConstitutif> builder)
    {
        builder.ToTable("MaquetteElementsConstitutifs", table =>
            table.HasCheckConstraint("CK_MaquetteElementsConstitutifs_CreditsCoefVolume", "[Credits] >= 0 AND [Coefficient] >= 0 AND [VolumeHoraire] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Credits).HasPrecision(5, 2);
        builder.Property(x => x.Coefficient).HasPrecision(5, 2);
        builder.Property(x => x.Coefficient).HasDefaultValue(1m);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.SemestrePedagogiqueId, x.ElementConstitutifId }).IsUnique();

        builder.HasOne(x => x.SemestrePedagogique)
            .WithMany(x => x.ElementsConstitutifs)
            .HasForeignKey(x => x.SemestrePedagogiqueId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ElementConstitutif)
            .WithMany()
            .HasForeignKey(x => x.ElementConstitutifId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
