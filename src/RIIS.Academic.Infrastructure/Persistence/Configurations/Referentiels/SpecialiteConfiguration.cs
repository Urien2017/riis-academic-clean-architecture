using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class SpecialiteConfiguration : IEntityTypeConfiguration<Specialite>
{
    public void Configure(EntityTypeBuilder<Specialite> builder)
    {
        builder.ToTable("Specialites");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Libelle).HasMaxLength(150).IsRequired();
        builder.HasIndex(x => new { x.FiliereId, x.Code }).IsUnique();
        builder.HasOne(x => x.Filiere)
            .WithMany(x => x.Specialites)
            .HasForeignKey(x => x.FiliereId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
