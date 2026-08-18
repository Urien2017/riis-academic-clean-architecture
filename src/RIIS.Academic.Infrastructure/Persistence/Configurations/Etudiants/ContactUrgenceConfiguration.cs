using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class ContactUrgenceConfiguration : IEntityTypeConfiguration<ContactUrgence>
{
    public void Configure(EntityTypeBuilder<ContactUrgence> builder)
    {
        builder.ToTable("ContactsUrgence");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NomComplet).HasMaxLength(150).IsRequired();
        builder.Property(x => x.LienParente).HasMaxLength(50);
        builder.Property(x => x.TelephonePrincipal).HasMaxLength(20).IsRequired();
        builder.Property(x => x.TelephoneSecondaire).HasMaxLength(20);
        builder.HasOne(x => x.Etudiant)
            .WithMany(x => x.ContactsUrgence)
            .HasForeignKey(x => x.EtudiantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
