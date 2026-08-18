using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class CycleFormationConfiguration : IEntityTypeConfiguration<CycleFormation>
{
    public void Configure(EntityTypeBuilder<CycleFormation> builder)
    {
        builder.ToTable("CyclesFormation");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(100).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasData(
            new CycleFormation { Id = 1, Code = "PREPA", Libelle = "Préparatoire", OrdreAffichage = 1 },
            new CycleFormation { Id = 2, Code = "BTS", Libelle = "Brevet de technicien supérieur", OrdreAffichage = 2 },
            new CycleFormation { Id = 3, Code = "LICENCE", Libelle = "Licence", OrdreAffichage = 3 },
            new CycleFormation { Id = 4, Code = "MASTER", Libelle = "Master", OrdreAffichage = 4 });
    }
}
