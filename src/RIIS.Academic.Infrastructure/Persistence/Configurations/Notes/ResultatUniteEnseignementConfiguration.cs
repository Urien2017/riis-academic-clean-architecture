using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ResultatUniteEnseignementConfiguration : IEntityTypeConfiguration<ResultatUniteEnseignement>
{
    public void Configure(EntityTypeBuilder<ResultatUniteEnseignement> builder)
    {
        builder.ToTable("ResultatsUnitesEnseignement");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Moyenne).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAcquis).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAttendus).HasPrecision(5, 2);
        builder.Property(x => x.StatutValidation).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.InscriptionId, x.SemestrePedagogiqueId }).IsUnique();

        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.ResultatsUnitesEnseignement)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.SemestrePedagogique)
            .WithMany()
            .HasForeignKey(x => x.SemestrePedagogiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
