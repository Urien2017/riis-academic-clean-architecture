namespace RIIS.Academic.Domain;

public class MaquetteElementConstitutif
{
    public long Id { get; set; }
    public long SemestrePedagogiqueId { get; set; }
    public long ElementConstitutifId { get; set; }
    public decimal Credits { get; set; }
    public decimal Coefficient { get; set; } = 1m;
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public string? Observation { get; set; }

    public SemestrePedagogique SemestrePedagogique { get; set; } = null!;
    public ElementConstitutif ElementConstitutif { get; set; } = null!;
    public ICollection<EvaluationAcademique> Evaluations { get; set; } = [];
    public ICollection<ResultatElementConstitutif> ResultatsElementsConstitutifs { get; set; } = [];
}
