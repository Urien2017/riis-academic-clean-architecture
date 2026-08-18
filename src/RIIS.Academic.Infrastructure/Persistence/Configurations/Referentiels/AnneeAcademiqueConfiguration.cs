using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class AnneeAcademiqueConfiguration : IEntityTypeConfiguration<AnneeAcademique>
{
    public void Configure(EntityTypeBuilder<AnneeAcademique> builder)
    {
        builder.ToTable("AnneesAcademiques", table =>
            table.HasCheckConstraint("CK_AnneesAcademiques_Periode", "[AnneeFin] = [AnneeDebut] + 1"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => x.Libelle).IsUnique();
        builder.HasIndex(x => new { x.AnneeDebut, x.AnneeFin }).IsUnique();
    }
}
