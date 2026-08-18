using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Programmes.Dtos;

public class ElementConstitutifHierarchyDto
{
    public long Id { get; set; }
    public long MaquetteElementConstitutifId { get; set; }
    public string? Code { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public TypeElementConstitutif Type { get; set; }
    public decimal Credits { get; set; }
    public decimal Coefficient { get; set; }
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; }
    public string? Observation { get; set; }
}
