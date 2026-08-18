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
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => new { x.AnneeAcademiqueId, x.ParcoursAcademiqueId, x.Code }).IsUnique();

        builder.HasOne(x => x.AnneeAcademique)
            .WithMany(x => x.ClassesPedagogiques)
            .HasForeignKey(x => x.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ParcoursAcademique)
            .WithMany(x => x.ClassesPedagogiques)
            .HasForeignKey(x => x.ParcoursAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
