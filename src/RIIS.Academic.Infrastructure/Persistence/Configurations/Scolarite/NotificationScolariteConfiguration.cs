using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;

public class NotificationScolariteConfiguration : IEntityTypeConfiguration<NotificationScolarite>
{
    public void Configure(EntityTypeBuilder<NotificationScolarite> builder)
    {
        builder.ToTable("NotificationsScolarite");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(40);
        builder.Property(x => x.Canal).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.Titre).HasMaxLength(200);
        builder.Property(x => x.Message).HasMaxLength(2000);
        builder.Property(x => x.Statut).HasConversion<string>().HasMaxLength(30);
        builder.HasIndex(x => new { x.DossierScolariteId, x.Statut, x.DateGenerationUtc });
        builder.HasIndex(x => new { x.EcheanceScolariteId, x.Type });

        builder.HasOne(x => x.DossierScolarite)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.DossierScolariteId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ElementScolariteEtudiant)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.ElementScolariteEtudiantId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.EcheanceScolarite)
            .WithMany(x => x.Notifications)
            .HasForeignKey(x => x.EcheanceScolariteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
