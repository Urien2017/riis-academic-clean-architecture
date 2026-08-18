namespace RIIS.Academic.Domain;

public class Specialite
{
    public long Id { get; set; }
    public long FiliereId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public bool EstActive { get; set; } = true;

    public Filiere Filiere { get; set; } = null!;
    public ICollection<ParcoursAcademique> ParcoursAcademiques { get; set; } = [];
}
