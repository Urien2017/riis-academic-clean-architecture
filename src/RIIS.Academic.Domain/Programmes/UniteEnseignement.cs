namespace RIIS.Academic.Domain;

public class UniteEnseignement
{
    public long Id { get; set; }
    public long SemestrePedagogiqueId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public decimal Credits { get; set; }
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; } = true;

    public SemestrePedagogique SemestrePedagogique { get; set; } = null!;
    public ICollection<ElementConstitutif> ElementsConstitutifs { get; set; } = [];
    public ICollection<ResultatUniteEnseignement> ResultatsUnitesEnseignement { get; set; } = [];
}
