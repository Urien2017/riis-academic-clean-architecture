using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ClassePedagogiqueConfiguration : IEntityTypeConfiguration<ClassePedagogique>
{
    public void Configure(EntityTypeBuilder<ClassePedagogique> builder)
    {
        builder.ToTable("ClassesPedagogiques");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => new { x.ParcoursAcademiqueId, x.Libelle }).IsUnique();
    }
}
