using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class DocumentElementScolariteConfiguration : IEntityTypeConfiguration<DocumentElementScolarite>
{
    public void Configure(EntityTypeBuilder<DocumentElementScolarite> builder)
    {
        builder.ToTable("DocumentsElementsScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NomDocument).HasMaxLength(200);
        builder.Property(x => x.UrlFichier).HasMaxLength(500);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.VerifiePar).HasMaxLength(150);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => new { x.ElementScolariteEtudiantId, x.NomDocument });

        builder.HasOne(x => x.ElementScolariteEtudiant)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.ElementScolariteEtudiantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
