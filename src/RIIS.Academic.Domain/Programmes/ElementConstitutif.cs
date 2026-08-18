namespace RIIS.Academic.Domain;

public class ElementConstitutif
{
    public long Id { get; set; }
    public long UniteEnseignementId { get; set; }
    public string? Code { get; set; }
    public required string Libelle { get; set; }
    public TypeElementConstitutif Type { get; set; } = TypeElementConstitutif.Cours;
    public decimal Credits { get; set; }
    public decimal Coefficient { get; set; } = 1m;
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public string? Observation { get; set; }

    public UniteEnseignement UniteEnseignement { get; set; } = null!;
    public ICollection<EvaluationAcademique> Evaluations { get; set; } = [];
    public ICollection<ResultatElementConstitutif> ResultatsElementsConstitutifs { get; set; } = [];
}
