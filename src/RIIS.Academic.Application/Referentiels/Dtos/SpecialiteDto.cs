namespace RIIS.Academic.Application.Referentiels.Dtos;

public class SpecialiteDto
{
    public long Id { get; set; }
    public long? CycleFormationId { get; set; }
    public long FiliereId { get; set; }
    public string FiliereLibelle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public bool EstActive { get; set; } = true;
}
