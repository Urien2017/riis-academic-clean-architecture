using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Inscriptions.Dtos;

public class InscriptionDto
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public long EtudiantId { get; set; }
    public string EtudiantMatricule { get; set; } = string.Empty;
    public string EtudiantNomComplet { get; set; } = string.Empty;
    public long ParcoursAcademiqueId { get; set; }
    public string ParcoursAcademiqueLibelle { get; set; } = string.Empty;
    public long NiveauEtudeId { get; set; }
    public string NiveauEtudeLibelle { get; set; } = string.Empty;
    public long? MaquettePedagogiqueId { get; set; }
    public string? MaquettePedagogiqueLibelle { get; set; }
    public long? ClassePedagogiqueId { get; set; }
    public string? ClassePedagogiqueLibelle { get; set; }
    public DateOnly DateInscription { get; set; }
    public StatutInscription Statut { get; set; } = StatutInscription.EnAttente;
    public string? MentionSpeciale { get; set; }
    public string? TutelleAcademique { get; set; }
    public string? Observation { get; set; }
    public string? CodeAdministration { get; set; }
}
