namespace RIIS.Academic.Domain;

public class CycleFormation
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<ParcoursAcademique> ParcoursAcademiques { get; set; } = [];
}
