using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class ModePaiementScolariteConfiguration : IEntityTypeConfiguration<ModePaiementScolarite>
{
    public void Configure(EntityTypeBuilder<ModePaiementScolarite> builder)
    {
        builder.ToTable("ModesPaiementScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(50);
        builder.Property(x => x.Libelle).HasMaxLength(150);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.EstActif, x.OrdreAffichage });
    }
}
