namespace RIIS.Academic.Application.Referentiels.Dtos;

public class ParcoursFormationDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public long CycleFormationId { get; set; }
    public string CycleCode { get; set; } = string.Empty;
    public string CycleLibelle { get; set; } = string.Empty;
    public long FiliereId { get; set; }
    public string FiliereCode { get; set; } = string.Empty;
    public string FiliereLibelle { get; set; } = string.Empty;
    public long SpecialiteId { get; set; }
    public string SpecialiteCode { get; set; } = string.Empty;
    public string SpecialiteLibelle { get; set; } = string.Empty;
    public bool EstActive { get; set; } = true;
}
