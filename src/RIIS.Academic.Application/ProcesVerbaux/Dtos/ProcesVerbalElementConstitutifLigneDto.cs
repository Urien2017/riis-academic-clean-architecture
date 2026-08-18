namespace RIIS.Academic.Application.ProcesVerbaux.Dtos;

public class ProcesVerbalElementConstitutifLigneDto
{
    public string UniteEnseignementCode { get; set; } = string.Empty;
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public string ElementConstitutifCode { get; set; } = string.Empty;
    public string ElementConstitutifLibelle { get; set; } = string.Empty;
    public decimal Credits { get; set; }
    public decimal? MoyenneCcon { get; set; }
    public decimal? MoyenneCc { get; set; }
    public decimal? MoyenneSn { get; set; }
    public decimal? MoyenneFinale { get; set; }
    public decimal CreditsAcquis { get; set; }
    public string Decision { get; set; } = string.Empty;

    public string Key => $"{ElementConstitutifCode}|{ElementConstitutifLibelle}";
}
