using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Notes.Dtos;

public class SaisieNoteLigneDto
{
    public long NoteEvaluationId { get; set; }
    public long InscriptionId { get; set; }
    public string Matricule { get; set; } = string.Empty;
    public string NomComplet { get; set; } = string.Empty;
    public decimal? Valeur { get; set; }
    public StatutPresenceEvaluation StatutPresence { get; set; } = StatutPresenceEvaluation.Present;
    public string? Observation { get; set; }
    public decimal? CreditsSemestreAcquis { get; set; }
    public decimal? CreditsSemestreRequis { get; set; }
    public bool EstEligibleSessionRattrapage { get; set; } = true;
}
