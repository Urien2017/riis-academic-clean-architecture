using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class UniteEnseignementConfiguration : IEntityTypeConfiguration<UniteEnseignement>
{
    public void Configure(EntityTypeBuilder<UniteEnseignement> builder)
    {
        builder.ToTable("UnitesEnseignement");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
