namespace RIIS.Academic.Application.Releves.Dtos;

public class ReleveNoteAnnuelDto
{
    public long InscriptionId { get; set; }
    public string Titre { get; set; } = "RELEVÉ DE NOTES ANNUEL/ANNUAL TRANSCRIPT";
    public string CycleFormationCode { get; set; } = string.Empty;
    public string CycleFormationLibelle { get; set; } = string.Empty;
    public string EtudiantNomComplet { get; set; } = string.Empty;
    public string Matricule { get; set; } = string.Empty;
    public DateOnly DateNaissance { get; set; }
    public string LieuNaissance { get; set; } = string.Empty;
    public string FiliereLibelle { get; set; } = string.Empty;
    public string? SpecialiteLibelle { get; set; }
    public string NiveauLibelle { get; set; } = string.Empty;
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public List<ReleveNoteSemestreDto> Semestres { get; set; } = [];
    public List<ReleveNoteResumeDto> Resume { get; set; } = [];
    public decimal? MoyenneAnnuelle { get; set; }
    public decimal CreditsCapitalises { get; set; }
    public decimal CreditsRequis { get; set; } = 60m;
    public string Decision { get; set; } = string.Empty;
    public string Mention { get; set; } = string.Empty;
    public string DecisionMention => $"DECISION : {Decision} MENTION : {Mention}";
}
