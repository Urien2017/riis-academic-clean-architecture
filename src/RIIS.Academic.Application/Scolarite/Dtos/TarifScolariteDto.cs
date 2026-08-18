namespace RIIS.Academic.Application.Scolarite.Dtos;

public class TarifScolariteDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public long TypeElementScolariteId { get; set; }
    public string TypeElementScolariteCode { get; set; } = string.Empty;
    public string TypeElementScolariteLibelle { get; set; } = string.Empty;
    public string AnneeAcademiqueCode { get; set; } = string.Empty;
    public string? CycleCode { get; set; }
    public int? NiveauNumero { get; set; }
    public string? FiliereCode { get; set; }
    public string? SpecialiteCode { get; set; }
    public decimal Montant { get; set; }
    public string Devise { get; set; } = "XOF";
    public DateOnly DateDebutValidite { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public DateOnly? DateFinValidite { get; set; }
    public int Priorite { get; set; }
    public bool EstActif { get; set; } = true;
    public string ContexteLibelle { get; set; } = string.Empty;
}
