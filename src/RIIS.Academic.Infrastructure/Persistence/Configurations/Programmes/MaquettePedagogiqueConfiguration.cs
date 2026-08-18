using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class MaquettePedagogiqueConfiguration : IEntityTypeConfiguration<MaquettePedagogique>
{
    public void Configure(EntityTypeBuilder<MaquettePedagogique> builder)
    {
        builder.ToTable("MaquettesPedagogiques");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Version).HasMaxLength(40).IsRequired();
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.SourceDocument).HasMaxLength(500);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ParcoursAcademiqueId, x.Code, x.Version }).IsUnique();

        builder.HasOne(x => x.ParcoursAcademique)
            .WithMany(x => x.MaquettesPedagogiques)
            .HasForeignKey(x => x.ParcoursAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
