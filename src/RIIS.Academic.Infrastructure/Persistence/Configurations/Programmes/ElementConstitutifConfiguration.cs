using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ElementConstitutifConfiguration : IEntityTypeConfiguration<ElementConstitutif>
{
    public void Configure(EntityTypeBuilder<ElementConstitutif> builder)
    {
        builder.ToTable("ElementsConstitutifs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30);
        builder.Property(x => x.Libelle).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(20);
        builder.HasIndex(x => new { x.UniteEnseignementId, x.Code })
            .IsUnique()
            .HasFilter("[Code] IS NOT NULL");

        builder.HasOne(x => x.UniteEnseignement)
            .WithMany(x => x.ElementsConstitutifs)
            .HasForeignKey(x => x.UniteEnseignementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
