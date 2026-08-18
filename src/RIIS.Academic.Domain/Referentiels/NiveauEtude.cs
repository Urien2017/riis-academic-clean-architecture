namespace RIIS.Academic.Domain;

public class NiveauEtude
{
    public long Id { get; set; }
    public byte Numero { get; set; }
    public required string Libelle { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<ParcoursAcademique> ParcoursAcademiques { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
    public ICollection<SemestrePedagogique> SemestresPedagogiques { get; set; } = [];
}
