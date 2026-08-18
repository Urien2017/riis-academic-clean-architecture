namespace RIIS.Academic.Domain;

public class TarifScolarite
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public long TypeElementScolariteId { get; set; }
    public required string AnneeAcademiqueCode { get; set; }
    public string? CycleCode { get; set; }
    public int? NiveauNumero { get; set; }
    public string? FiliereCode { get; set; }
    public string? SpecialiteCode { get; set; }
    public decimal Montant { get; set; }
    public string Devise { get; set; } = "XOF";
    public DateOnly DateDebutValidite { get; set; }
    public DateOnly? DateFinValidite { get; set; }
    public int Priorite { get; set; }
    public bool EstActif { get; set; } = true;

    public TypeElementScolarite TypeElementScolarite { get; set; } = null!;
}
