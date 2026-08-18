namespace RIIS.Academic.Domain;

public class DossierAdmission
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public string? SerieBaccalaureat { get; set; }
    public short? AnneeObtentionBaccalaureat { get; set; }
    public string? MentionBaccalaureat { get; set; }
    public string? DiplomeEntree { get; set; }
    public string? SpecialiteDiplomeEntree { get; set; }
    public string? NumeroEquivalence { get; set; }
    public string? DiplomeEquivalence { get; set; }

    public Inscription Inscription { get; set; } = null!;
}
