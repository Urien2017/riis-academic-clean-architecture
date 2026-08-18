using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class SemestrePedagogiqueConfiguration : IEntityTypeConfiguration<SemestrePedagogique>
{
    public void Configure(EntityTypeBuilder<SemestrePedagogique> builder)
    {
        builder.ToTable("SemestresPedagogiques", table =>
            table.HasCheckConstraint("CK_SemestresPedagogiques_NumeroSemestre", "[NumeroSemestre] BETWEEN 1 AND 10"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Libelle).HasMaxLength(50).IsRequired();
        builder.HasIndex(x => new { x.MaquettePedagogiqueId, x.NumeroSemestre, x.UniteEnseignementId }).IsUnique();

        builder.HasOne(x => x.MaquettePedagogique)
            .WithMany(x => x.Semestres)
            .HasForeignKey(x => x.MaquettePedagogiqueId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.UniteEnseignement)
            .WithMany()
            .HasForeignKey(x => x.UniteEnseignementId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
