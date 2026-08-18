namespace RIIS.Academic.Application.Releves.Dtos;

public class ReleveNoteResumeDto
{
    public string Libelle { get; set; } = string.Empty;
    public decimal? Moyenne { get; set; }
    public decimal CreditsCapitalises { get; set; }
}
