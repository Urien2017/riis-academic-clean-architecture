using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class NoteEvaluationConfiguration : IEntityTypeConfiguration<NoteEvaluation>
{
    public void Configure(EntityTypeBuilder<NoteEvaluation> builder)
    {
        builder.ToTable("NotesEvaluations", table =>
            table.HasCheckConstraint("CK_NotesEvaluations_Valeur", "[Valeur] IS NULL OR ([Valeur] >= 0 AND [Valeur] <= 20)"));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Valeur).HasPrecision(5, 2);
        builder.Property(x => x.StatutPresence).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.Property(x => x.SaisiePar).HasMaxLength(150);
        builder.HasIndex(x => new { x.EvaluationAcademiqueId, x.InscriptionId }).IsUnique();

        builder.HasOne(x => x.EvaluationAcademique)
            .WithMany(x => x.Notes)
            .HasForeignKey(x => x.EvaluationAcademiqueId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.NotesEvaluations)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
