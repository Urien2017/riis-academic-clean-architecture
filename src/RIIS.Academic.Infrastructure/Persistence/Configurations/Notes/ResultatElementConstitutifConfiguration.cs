using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ResultatElementConstitutifConfiguration : IEntityTypeConfiguration<ResultatElementConstitutif>
{
    public void Configure(EntityTypeBuilder<ResultatElementConstitutif> builder)
    {
        builder.ToTable("ResultatsElementsConstitutifs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.MoyenneControleContinu).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneControleConnaissance).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneSessionNormale).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneSessionRattrapage).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneAvantRattrapage).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneApresRattrapage).HasPrecision(5, 2);
        builder.Property(x => x.MoyenneRetenue).HasPrecision(5, 2);
        builder.Property(x => x.CreditsAcquis).HasPrecision(5, 2);
        builder.Property(x => x.StatutValidation).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.InscriptionId, x.ElementConstitutifId }).IsUnique();

        builder.HasOne(x => x.Inscription)
            .WithMany(x => x.ResultatsElementsConstitutifs)
            .HasForeignKey(x => x.InscriptionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ElementConstitutif)
            .WithMany(x => x.ResultatsElementsConstitutifs)
            .HasForeignKey(x => x.ElementConstitutifId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
