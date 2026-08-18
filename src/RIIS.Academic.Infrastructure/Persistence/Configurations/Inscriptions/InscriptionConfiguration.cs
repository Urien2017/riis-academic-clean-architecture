using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> builder)
    {
        builder.ToTable("Inscriptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.MentionSpeciale).HasMaxLength(500);
        builder.Property(x => x.TutelleAcademique).HasMaxLength(200);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.Property(x => x.CodeAdministration).HasMaxLength(50);
        builder.Property(x => x.Version).IsRowVersion();
        builder.HasIndex(x => new { x.AnneeAcademiqueId, x.EtudiantId }).IsUnique();
        builder.HasIndex(x => x.CodeAdministration)
            .IsUnique()
            .HasFilter("[CodeAdministration] IS NOT NULL");

        builder.HasOne(x => x.AnneeAcademique).WithMany(x => x.Inscriptions)
            .HasForeignKey(x => x.AnneeAcademiqueId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Etudiant).WithMany(x => x.Inscriptions)
            .HasForeignKey(x => x.EtudiantId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ParcoursAcademique).WithMany(x => x.Inscriptions)
            .HasForeignKey(x => x.ParcoursAcademiqueId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.NiveauEtude).WithMany(x => x.Inscriptions)
            .HasForeignKey(x => x.NiveauEtudeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.MaquettePedagogique).WithMany(x => x.Inscriptions)
            .HasForeignKey(x => x.MaquettePedagogiqueId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ClassePedagogique).WithMany()
            .HasForeignKey(x => x.ClassePedagogiqueId).OnDelete(DeleteBehavior.Restrict);
    }
}
