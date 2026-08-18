using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ValidationInscriptionConfiguration : IEntityTypeConfiguration<ValidationInscription>
{
    public void Configure(EntityTypeBuilder<ValidationInscription> builder)
    {
        builder.ToTable("ValidationsInscriptions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.LieuSignature).HasMaxLength(100);
        builder.Property(x => x.NomSignataireEtudiant).HasMaxLength(150);
        builder.Property(x => x.SignatureEtudiantUrl).HasMaxLength(500);
        builder.Property(x => x.NomSignataireAdministration).HasMaxLength(150);
        builder.Property(x => x.SignatureAdministrationUrl).HasMaxLength(500);
        builder.Property(x => x.Observation).HasMaxLength(1000);
        builder.HasIndex(x => x.InscriptionId).IsUnique();

        builder.HasOne(x => x.Inscription)
            .WithOne(x => x.ValidationInscription)
            .HasForeignKey<ValidationInscription>(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
