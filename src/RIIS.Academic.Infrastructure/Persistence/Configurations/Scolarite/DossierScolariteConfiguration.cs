using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class DossierScolariteConfiguration : IEntityTypeConfiguration<DossierScolarite>
{
    public void Configure(EntityTypeBuilder<DossierScolarite> builder)
    {
        builder.ToTable("DossiersScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.AnneeAcademiqueCode).HasMaxLength(30);
        builder.Property(x => x.AnneeAcademiqueLibelle).HasMaxLength(100);
        builder.Property(x => x.CycleCode).HasMaxLength(30);
        builder.Property(x => x.CycleLibelle).HasMaxLength(150);
        builder.Property(x => x.FiliereCode).HasMaxLength(50);
        builder.Property(x => x.FiliereLibelle).HasMaxLength(150);
        builder.Property(x => x.SpecialiteCode).HasMaxLength(50);
        builder.Property(x => x.SpecialiteLibelle).HasMaxLength(150);
        builder.Property(x => x.NiveauLibelle).HasMaxLength(100);
        builder.Property(x => x.StatutAdministratif).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.StatutFinancier).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.StatutGlobal).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => x.InscriptionId).IsUnique();
        builder.HasIndex(x => new { x.AnneeAcademiqueCode, x.CycleCode, x.FiliereCode, x.SpecialiteCode, x.NiveauNumero });

        builder.HasOne(x => x.Inscription)
            .WithOne(x => x.DossierScolarite)
            .HasForeignKey<DossierScolarite>(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
