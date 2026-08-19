using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class TarifScolariteConfiguration : IEntityTypeConfiguration<TarifScolarite>
{
    public void Configure(EntityTypeBuilder<TarifScolarite> builder)
    {
        builder.ToTable("TarifsScolarite", table =>
            table.HasCheckConstraint("CK_TarifsScolarite_Montant", "[Montant] >= 0"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Montant).HasPrecision(18, 2);
        builder.Property(x => x.Devise).HasMaxLength(10);
        builder.HasIndex(x => x.Code);
        builder.HasIndex(x => new { x.TypeElementScolariteId, x.ParcoursAcademiqueId });

        builder.HasOne(x => x.TypeElementScolarite)
            .WithMany(x => x.Tarifs)
            .HasForeignKey(x => x.TypeElementScolariteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ParcoursAcademique)
            .WithMany()
            .HasForeignKey(x => x.ParcoursAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
