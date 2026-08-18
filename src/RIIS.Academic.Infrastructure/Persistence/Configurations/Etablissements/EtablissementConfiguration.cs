using RIIS.Academic.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RIIS.Academic.Infrastructure.Persistence.Configurations;
public class EtablissementConfiguration : IEntityTypeConfiguration<Etablissement>
{
    public void Configure(EntityTypeBuilder<Etablissement> builder)
    {
        builder.ToTable("Etablissements");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NomOfficiel).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Sigle).HasMaxLength(30);
        builder.Property(x => x.NumeroAutorisation).HasMaxLength(150);
        builder.Property(x => x.NumeroRegistreCommerce).HasMaxLength(80);
        builder.Property(x => x.Banque).HasMaxLength(80);
        builder.Property(x => x.NumeroCompteBancaire).HasMaxLength(80);
        builder.Property(x => x.TelephonePrincipal).HasMaxLength(20);
        builder.Property(x => x.TelephoneSecondaire).HasMaxLength(20);
        builder.Property(x => x.BoitePostale).HasMaxLength(50);
        builder.Property(x => x.Ville).HasMaxLength(80);
        builder.Property(x => x.Pays).HasMaxLength(80);
        builder.Property(x => x.Email).HasMaxLength(254);
        builder.Property(x => x.Facebook).HasMaxLength(100);
        builder.Property(x => x.Instagram).HasMaxLength(100);
        builder.Property(x => x.Adresse).HasMaxLength(300);
        builder.Property(x => x.LogoUrl).HasMaxLength(500);
        builder.HasIndex(x => x.Sigle).IsUnique().HasFilter("[Sigle] IS NOT NULL");

        builder.HasData(new Etablissement
        {
            Id = 1,
            NomOfficiel = "RAPUS INTERNATIONAL INSTITUTE SCHOOL",
            Sigle = "RIIS",
            NumeroAutorisation = "23-02048/L/MINESUP/SG/DDES/ESUP/SDA/AOSB",
            NumeroRegistreCommerce = "RC/YAO/2022/B/1986",
            Banque = "UBA",
            NumeroCompteBancaire = "10033 05206 06011000466 49",
            TelephonePrincipal = "+237 699 51 17 76",
            TelephoneSecondaire = "+237 651 61 35 98",
            BoitePostale = "BP 1463",
            Ville = "Yaoundé",
            Pays = "Cameroun",
            Email = "contact@riis.institut",
            Facebook = "riis",
            Instagram = "riis_officiel",
            Adresse = "Nouvelle route Bastos, entrée derrière la Banque Mondiale"
        });
    }
}
