using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Dtos;

public class FinanceDossierScolariteDto
{
    public long DossierScolariteId { get; set; }
    public decimal TotalPaiements { get; set; }
    public decimal TotalPaiementsAffectes { get; set; }
    public decimal TotalPaiementsNonAffectes { get; set; }
    public List<ElementFinancierDossierDto> Elements { get; set; } = [];
    public List<EcheanceFinanciereDossierDto> Echeances { get; set; } = [];
    public List<PaiementLibreDossierDto> Paiements { get; set; } = [];
    public List<AffectationPaiementDossierDto> Affectations { get; set; } = [];
}

public class ElementFinancierDossierDto
{
    public long Id { get; set; }
    public long TypeElementScolariteId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public decimal MontantAttendu { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantRestant { get; set; }
    public StatutElementScolarite Statut { get; set; }
}

public class EcheanceFinanciereDossierDto
{
    public long Id { get; set; }
    public long ElementScolariteEtudiantId { get; set; }
    public string ElementLibelle { get; set; } = string.Empty;
    public int Numero { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public DateOnly DateExigibilite { get; set; }
    public decimal MontantAttendu { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantRestant { get; set; }
    public StatutEcheanceScolarite Statut { get; set; }
}

public class PaiementLibreDossierDto
{
    public long Id { get; set; }
    public long DossierScolariteId { get; set; }
    public long TypeElementScolariteId { get; set; }
    public string TypeElementScolariteLibelle { get; set; } = string.Empty;
    public long TarifScolariteId { get; set; }
    public decimal TarifMontant { get; set; }
    public long ModePaiementScolariteId { get; set; }
    public DateOnly DatePaiement { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal Montant { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public string? ReferencePaiement { get; set; }
    public string? EncaissePar { get; set; }
    public string? Observation { get; set; }
    public decimal MontantAffecte { get; set; }
    public decimal MontantNonAffecte { get; set; }
    public StatutPaiementScolarite Statut { get; set; } = StatutPaiementScolarite.NonAffecte;
}

public class PaiementTypeElementTarifOptionDto
{
    public long TypeElementScolariteId { get; set; }
    public string TypeElementScolariteLibelle { get; set; } = string.Empty;
    public long TarifScolariteId { get; set; }
    public decimal TarifMontant { get; set; }
    public string Devise { get; set; } = "XAF";
    public string Libelle => $"{TypeElementScolariteLibelle} - {TarifMontant:N0} {Devise}";
}

public class AffectationPaiementDossierDto
{
    public long Id { get; set; }
    public long PaiementScolariteId { get; set; }
    public long EcheanceScolariteId { get; set; }
    public string PaiementLibelle { get; set; } = string.Empty;
    public string EcheanceLibelle { get; set; } = string.Empty;
    public decimal MontantAffecte { get; set; }
    public DateTime DateAffectationUtc { get; set; }
    public string? AffectePar { get; set; }
}
