using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Programmes.Dtos;

public class MaquettePedagogiqueHierarchyDto
{
    public long Id { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public string ParcoursAcademiqueLibelle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public StatutMaquettePedagogique Statut { get; set; }
    public DateOnly? DateDebutValidite { get; set; }
    public DateOnly? DateFinValidite { get; set; }
    public string? SourceDocument { get; set; }
    public string? Observation { get; set; }
    public List<SemestrePedagogiqueHierarchyDto> Semestres { get; set; } = [];
}
