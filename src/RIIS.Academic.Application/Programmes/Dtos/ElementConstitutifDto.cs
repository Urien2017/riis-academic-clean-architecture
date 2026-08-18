using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Programmes.Dtos;

public class ElementConstitutifDto
{
    public long Id { get; set; }
    public long UniteEnseignementId { get; set; }
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public TypeElementConstitutif Type { get; set; } = TypeElementConstitutif.Cours;
    public decimal Credits { get; set; }
    public decimal Coefficient { get; set; } = 1m;
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public string? Observation { get; set; }
}
