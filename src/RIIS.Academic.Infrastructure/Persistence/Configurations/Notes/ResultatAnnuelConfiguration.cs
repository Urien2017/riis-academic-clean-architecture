using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ResultatAnnuelConfiguration : IEntityTypeConfiguration<ResultatAnnuel>
{
    public void Configure(EntityTypeBuilder<ResultatAnnuel> builder)
    {
        builder.ToTable("ResultatsAnnuels");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MoyenneAnnuelle).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAcquis).HasPrecision(5, 2);
        builder.Property(x => x.CreditsRequis).HasPrecision(5, 2).HasDefaultValue(60m);
        builder.Property(x => x.StatutValidation).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.DecisionJury).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.InscriptionId, x.AnneeAcademiqueId, x.NiveauEtudeId }).IsUnique();

        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.ResultatsAnnuels)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.AnneeAcademique)
            .WithMany()
            .HasForeignKey(x => x.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.NiveauEtude)
            .WithMany()
            .HasForeignKey(x => x.NiveauEtudeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
