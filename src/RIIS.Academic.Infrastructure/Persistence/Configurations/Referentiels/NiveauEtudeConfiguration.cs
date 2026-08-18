using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class NiveauEtudeConfiguration : IEntityTypeConfiguration<NiveauEtude>
{
    public void Configure(EntityTypeBuilder<NiveauEtude> builder)
    {
        builder.ToTable("NiveauxEtude", table =>
            table.HasCheckConstraint("CK_NiveauxEtude_Numero", "[Numero] >= 1"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => x.Numero).IsUnique();
        builder.HasData(
            new NiveauEtude { Id = 1, Numero = 1, Libelle = "Niveau 1" },
            new NiveauEtude { Id = 2, Numero = 2, Libelle = "Niveau 2" },
            new NiveauEtude { Id = 3, Numero = 3, Libelle = "Niveau 3" },
            new NiveauEtude { Id = 4, Numero = 4, Libelle = "Niveau 4" },
            new NiveauEtude { Id = 5, Numero = 5, Libelle = "Niveau 5" });
    }
}
