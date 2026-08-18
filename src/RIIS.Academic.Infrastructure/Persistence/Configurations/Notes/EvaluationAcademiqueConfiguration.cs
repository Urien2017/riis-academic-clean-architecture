using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class EvaluationAcademiqueConfiguration : IEntityTypeConfiguration<EvaluationAcademique>
{
    public void Configure(EntityTypeBuilder<EvaluationAcademique> builder)
    {
        builder.ToTable("EvaluationsAcademiques", table =>
            table.HasCheckConstraint(
                "CK_EvaluationsAcademiques_BaremePonderation",
                "[Bareme] > 0 AND [PonderationPourcentage] >= 0 AND [PonderationPourcentage] <= 100"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Bareme).HasPrecision(5, 2).HasDefaultValue(20m);
        builder.Property(x => x.PonderationPourcentage).HasPrecision(5, 2);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.AnneeAcademiqueId, x.MaquetteElementConstitutifId, x.Type, x.Numero }).IsUnique();

        builder.HasOne(x => x.AnneeAcademique)
            .WithMany()
            .HasForeignKey(x => x.AnneeAcademiqueId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MaquetteElementConstitutif)
            .WithMany(x => x.Evaluations)
            .HasForeignKey(x => x.MaquetteElementConstitutifId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.EvaluationRemplacee)
            .WithMany(x => x.EvaluationsDeRattrapage)
            .HasForeignKey(x => x.EvaluationRemplaceeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
