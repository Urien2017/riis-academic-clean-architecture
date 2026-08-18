namespace RIIS.Academic.Domain;

public class Filiere
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public bool EstActive { get; set; } = true;

    public ICollection<Specialite> Specialites { get; set; } = [];
    public ICollection<ParcoursAcademique> ParcoursAcademiques { get; set; } = [];
}
