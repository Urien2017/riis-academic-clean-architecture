using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ResultatSemestreConfiguration : IEntityTypeConfiguration<ResultatSemestre>
{
    public void Configure(EntityTypeBuilder<ResultatSemestre> builder)
    {
        builder.ToTable("ResultatsSemestres");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MoyenneControleContinu).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneControleConnaissance).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneSessionNormale).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneSessionRattrapage).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneSemestrielle).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAcquis).HasPrecision(5, 2);
        builder.Property(x => x.CreditsRequis).HasPrecision(5, 2).HasDefaultValue(30m);
        builder.Property(x => x.StatutValidation).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.DecisionJury).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.InscriptionId, x.SemestrePedagogiqueId }).IsUnique();

        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.ResultatsSemestres)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.SemestrePedagogique)
            .WithMany(x => x.ResultatsSemestres)
            .HasForeignKey(x => x.SemestrePedagogiqueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
