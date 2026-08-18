using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class ValidationElementScolariteConfiguration : IEntityTypeConfiguration<ValidationElementScolarite>
{
    public void Configure(EntityTypeBuilder<ValidationElementScolarite> builder)
    {
        builder.ToTable("ValidationsElementsScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.ValidePar).HasMaxLength(150);
        builder.Property(x => x.MotifRejet).HasMaxLength(500);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ElementScolariteEtudiantId, x.Statut });

        builder.HasOne(x => x.ElementScolariteEtudiant)
            .WithMany(x => x.Validations)
            .HasForeignKey(x => x.ElementScolariteEtudiantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
