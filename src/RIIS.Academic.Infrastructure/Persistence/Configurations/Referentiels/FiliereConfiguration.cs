using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class FiliereConfiguration : IEntityTypeConfiguration<Filiere>
{
    public void Configure(EntityTypeBuilder<Filiere> builder)
    {
        builder.ToTable("Filieres");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
