using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ProcesVerbalConfiguration : IEntityTypeConfiguration<ProcesVerbal>
{
    public void Configure(EntityTypeBuilder<ProcesVerbal> builder)
    {
        builder.ToTable("ProcesVerbaux");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.CodeSession).HasMaxLength(20);
        builder.Property(x => x.Titre).HasMaxLength(250).IsRequired();
        builder.Property(x => x.CheminFichier).HasMaxLength(500);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ParcoursAcademiqueId, x.ClassePedagogiqueId, x.SemestrePedagogiqueId, x.Type, x.CodeSession });

        builder.HasOne(x => x.AnneeAcademique)
            .WithMany()
            .HasForeignKey(x => x.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ParcoursAcademique)
            .WithMany(x => x.ProcesVerbaux)
            .HasForeignKey(x => x.ParcoursAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ClassePedagogique)
            .WithMany()
            .HasForeignKey(x => x.ClassePedagogiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.SemestrePedagogique)
            .WithMany(x => x.ProcesVerbaux)
            .HasForeignKey(x => x.SemestrePedagogiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
