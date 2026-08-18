using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Dtos;

public class AdministrationDossierScolariteDto
{
    public long DossierScolariteId { get; set; }
    public bool PiecesObligatoiresCompletes { get; set; }
    public bool PaiementInitialEffectue { get; set; }
    public bool SuiteParcoursAutorisee => PiecesObligatoiresCompletes || PaiementInitialEffectue;
    public decimal MontantPaiementsActifs { get; set; }
    public int NombrePiecesObligatoires { get; set; }
    public int NombrePiecesObligatoiresCompletes { get; set; }
    public string MotifAutorisation { get; set; } = string.Empty;
    public List<DocumentAdministratifDossierDto> Documents { get; set; } = [];
}

public class DocumentAdministratifDossierDto
{
    public long ElementScolariteEtudiantId { get; set; }
    public long TypeElementScolariteId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public bool EstObligatoire { get; set; }
    public bool EstDocumentaire { get; set; }
    public bool EstSoumisValidation { get; set; }
    public StatutElementScolarite ElementStatut { get; set; } = StatutElementScolarite.NonDemarre;
    public long? DocumentId { get; set; }
    public string NomDocument { get; set; } = string.Empty;
    public string? UrlFichier { get; set; }
    public StatutDocumentScolarite DocumentStatut { get; set; } = StatutDocumentScolarite.EnAttente;
    public DateTime? DateDepotUtc { get; set; }
    public DateTime? DateVerificationUtc { get; set; }
    public string? VerifiePar { get; set; }
    public long? ValidationId { get; set; }
    public StatutValidationScolarite ValidationStatut { get; set; } = StatutValidationScolarite.EnAttente;
    public DateTime? DateValidationUtc { get; set; }
    public string? ValidePar { get; set; }
    public string? MotifRejet { get; set; }
    public string? Observation { get; set; }
    public bool EstComplet { get; set; }
}
