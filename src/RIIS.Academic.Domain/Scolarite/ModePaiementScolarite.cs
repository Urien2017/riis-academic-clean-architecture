namespace RIIS.Academic.Domain;

public class ModePaiementScolarite
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public int OrdreAffichage { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<PaiementScolarite> Paiements { get; set; } = [];
}
