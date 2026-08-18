using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class SemestrePedagogiqueConfiguration : IEntityTypeConfiguration<SemestrePedagogique>
{
    public void Configure(EntityTypeBuilder<SemestrePedagogique> builder)
    {
        builder.ToTable("SemestresPedagogiques", table =>
            table.HasCheckConstraint("CK_SemestresPedagogiques_Numero", "[Numero] BETWEEN 1 AND 10"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CreditsAttendus).HasPrecision(5, 2);
        builder.HasIndex(x => new { x.MaquettePedagogiqueId, x.Numero }).IsUnique();

        builder.HasOne(x => x.MaquettePedagogique)
            .WithMany(x => x.Semestres)
            .HasForeignKey(x => x.MaquettePedagogiqueId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.NiveauEtude)
            .WithMany(x => x.SemestresPedagogiques)
            .HasForeignKey(x => x.NiveauEtudeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
