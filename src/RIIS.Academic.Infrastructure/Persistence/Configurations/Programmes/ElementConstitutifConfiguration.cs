using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ElementConstitutifConfiguration : IEntityTypeConfiguration<ElementConstitutif>
{
    public void Configure(EntityTypeBuilder<ElementConstitutif> builder)
    {
        builder.ToTable("ElementsConstitutifs", table =>
            table.HasCheckConstraint("CK_ElementsConstitutifs_CreditsCoefVolume", "[Credits] >= 0 AND [Coefficient] >= 0 AND [VolumeHoraire] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30);
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.Credits).HasPrecision(5, 2);
        builder.Property(x => x.Coefficient).HasPrecision(5, 2);
        builder.Property(x => x.Coefficient).HasDefaultValue(1m);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.UniteEnseignementId, x.OrdreAffichage }).IsUnique();
        builder.HasIndex(x => new { x.UniteEnseignementId, x.Code })
            .IsUnique()
            .HasFilter("[Code] IS NOT NULL");

        builder.HasOne(x => x.UniteEnseignement)
            .WithMany(x => x.ElementsConstitutifs)
            .HasForeignKey(x => x.UniteEnseignementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
