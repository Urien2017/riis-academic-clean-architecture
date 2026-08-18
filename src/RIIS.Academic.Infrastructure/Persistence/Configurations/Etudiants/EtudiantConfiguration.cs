using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class EtudiantConfiguration : IEntityTypeConfiguration<Etudiant>
{
    public void Configure(EntityTypeBuilder<Etudiant> builder)
    {
        builder.ToTable("Etudiants");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Matricule).HasMaxLength(30);
        builder.HasIndex(x => x.Matricule)
            .IsUnique()
            .HasFilter("[Matricule] IS NOT NULL");
        builder.Property(x => x.Nom).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Prenoms).HasMaxLength(150).IsRequired();
        builder.Property(x => x.LieuNaissance).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Sexe).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.AptitudeMedicale).HasConversion<string>().HasMaxLength(20);
        builder.Property(x => x.Nationalite).HasMaxLength(80).IsRequired();
        builder.Property(x => x.RegionOrigine).HasMaxLength(100);
        builder.Property(x => x.TelephonePrincipal).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TelephoneSecondaire).HasMaxLength(20);
        builder.Property(x => x.Email).HasMaxLength(254);
        builder.Property(x => x.NomPere).HasMaxLength(150);
        builder.Property(x => x.NomMere).HasMaxLength(150);
        builder.Property(x => x.LieuResidence).HasMaxLength(250);
        builder.Property(x => x.PhotoUrl).HasMaxLength(500);
        builder.Property(x => x.Version).IsRowVersion();
        builder.HasIndex(x => x.Nom);
    }
}
