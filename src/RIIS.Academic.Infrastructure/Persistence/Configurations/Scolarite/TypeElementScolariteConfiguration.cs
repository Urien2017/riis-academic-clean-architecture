using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class TypeElementScolariteConfiguration : IEntityTypeConfiguration<TypeElementScolarite>
{
    public void Configure(EntityTypeBuilder<TypeElementScolarite> builder)
    {
        builder.ToTable("TypesElementsScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(50);
        builder.Property(x => x.Libelle).HasMaxLength(200);
        builder.Property(x => x.Categorie).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
