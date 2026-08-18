namespace RIIS.Academic.Domain;

public class AnneeAcademique
{
    public long Id { get; set; }
    public required string Libelle { get; set; }
    public short AnneeDebut { get; set; }
    public short AnneeFin { get; set; }
    public bool EstActive { get; set; }

    public ICollection<ParcoursAcademique> ParcoursAcademiques { get; set; } = [];
    public ICollection<ClassePedagogique> ClassesPedagogiques { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
}
