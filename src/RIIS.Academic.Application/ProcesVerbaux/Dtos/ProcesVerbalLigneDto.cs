using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.ProcesVerbaux.Dtos;

public class ProcesVerbalLigneDto
{
    public long Id { get; set; }
    public long ProcesVerbalId { get; set; }
    public long InscriptionId { get; set; }
    public string Matricule { get; set; } = string.Empty;
    public string EtudiantNomComplet { get; set; } = string.Empty;
    public decimal? MoyenneGenerale { get; set; }
    public decimal? CreditsAcquis { get; set; }
    public int? Rang { get; set; }
    public DecisionAcademique DecisionJury { get; set; } = DecisionAcademique.NonDeliberee;
    public List<ProcesVerbalElementConstitutifLigneDto> ElementsConstitutifs { get; set; } = [];
    public string DecisionJuryLibelle => DecisionJury switch
    {
        DecisionAcademique.Valide => "Validé",
        DecisionAcademique.AutoriseRattrapage => "Rattrapage",
        DecisionAcademique.Ajoune => "Ajourné",
        _ => "Non délibéré"
    };
}
