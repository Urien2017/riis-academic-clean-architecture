namespace RIIS.Academic.Application.Releves.Dtos;

public class ReleveNoteLigneDto
{
    public string UniteEnseignementLibelle { get; set; } = string.Empty;
    public string? ElementConstitutifCode { get; set; }
    public string ElementConstitutifLibelle { get; set; } = string.Empty;
    public string Session { get; set; } = string.Empty;
    public decimal? NoteSur20 { get; set; }
    public string Decision { get; set; } = string.Empty;
    public decimal CreditsCapitalises { get; set; }
    public string Grade { get; set; } = string.Empty;
}
