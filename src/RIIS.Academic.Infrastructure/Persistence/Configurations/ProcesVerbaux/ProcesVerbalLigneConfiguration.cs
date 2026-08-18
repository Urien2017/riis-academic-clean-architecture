using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ProcesVerbalLigneConfiguration : IEntityTypeConfiguration<ProcesVerbalLigne>
{
    public void Configure(EntityTypeBuilder<ProcesVerbalLigne> builder)
    {
        builder.ToTable("ProcesVerbauxLignes");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MatriculeSnapshot).HasMaxLength(30).IsRequired();
        builder.Property(x => x.NomCompletSnapshot).HasMaxLength(250).IsRequired();
        builder.Property(x => x.MoyenneGenerale).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAcquis).HasPrecision(5, 2);
        builder.Property(x => x.DecisionJury).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.ProcesVerbalId, x.InscriptionId }).IsUnique();

        builder.HasOne(x => x.ProcesVerbal)
            .WithMany(x => x.Lignes)
            .HasForeignKey(x => x.ProcesVerbalId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.LignesProcesVerbaux)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
