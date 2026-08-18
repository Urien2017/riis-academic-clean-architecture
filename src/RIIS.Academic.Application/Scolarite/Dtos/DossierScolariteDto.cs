using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Dtos;

public class DossierScolariteDto
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public string EtudiantMatricule { get; set; } = string.Empty;
    public string EtudiantNomComplet { get; set; } = string.Empty;
    public DateOnly DateInscription { get; set; }
    public string AnneeAcademiqueCode { get; set; } = string.Empty;
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public string CycleCode { get; set; } = string.Empty;
    public string CycleLibelle { get; set; } = string.Empty;
    public string FiliereCode { get; set; } = string.Empty;
    public string FiliereLibelle { get; set; } = string.Empty;
    public string? SpecialiteCode { get; set; }
    public string? SpecialiteLibelle { get; set; }
    public int NiveauNumero { get; set; }
    public string NiveauLibelle { get; set; } = string.Empty;
    public StatutDossierScolarite StatutAdministratif { get; set; } = StatutDossierScolarite.EnPreparation;
    public StatutDossierScolarite StatutFinancier { get; set; } = StatutDossierScolarite.EnPreparation;
    public StatutDossierScolarite StatutGlobal { get; set; } = StatutDossierScolarite.EnPreparation;
    public DateTime? DateCreationUtc { get; set; }
    public DateTime? DateDernierRecalculUtc { get; set; }
    public string? Observation { get; set; }
    public bool Existe => Id > 0;
}
