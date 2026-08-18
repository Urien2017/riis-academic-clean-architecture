namespace RIIS.Academic.Application.Releves.Dtos;

public class ReleveNoteSemestreDto
{
    public byte Numero { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public List<ReleveNoteLigneDto> Lignes { get; set; } = [];
    public decimal TotalNotes { get; set; }
    public decimal? Moyenne { get; set; }
    public decimal CreditsCapitalises { get; set; }
    public decimal CreditsAttendus { get; set; } = 30m;
    public string Grade { get; set; } = string.Empty;
}
