namespace RIIS.Academic.Application.Scolarite.Dtos;

public class TarifScolariteContexteDto
{
    public string AnneeAcademiqueCode { get; set; } = string.Empty;
    public string? CycleCode { get; set; }
    public int? NiveauNumero { get; set; }
    public string? FiliereCode { get; set; }
    public string? SpecialiteCode { get; set; }
    public DateOnly? DateReference { get; set; }
}
