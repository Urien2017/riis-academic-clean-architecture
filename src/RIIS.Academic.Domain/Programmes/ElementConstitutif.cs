namespace RIIS.Academic.Domain;

public class ElementConstitutif
{
    public long Id { get; set; }
    public long UniteEnseignementId { get; set; }
    public string? Code { get; set; }
    public required string Libelle { get; set; }
    public TypeElementConstitutif Type { get; set; } = TypeElementConstitutif.CoursMagistraux;

    public UniteEnseignement UniteEnseignement { get; set; } = null!;
}
