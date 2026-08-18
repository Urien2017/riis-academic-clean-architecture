namespace RIIS.Academic.Domain;

public class ValidationInscription
{
    public long Id { get; set; }
    public long InscriptionId { get; set; }
    public string? LieuSignature { get; set; }
    public DateOnly? DateSignatureEtudiant { get; set; }
    public string? NomSignataireEtudiant { get; set; }
    public string? SignatureEtudiantUrl { get; set; }
    public string? NomSignataireAdministration { get; set; }
    public string? SignatureAdministrationUrl { get; set; }
    public DateOnly? DateValidationAdministration { get; set; }
    public string? Observation { get; set; }

    public Inscription Inscription { get; set; } = null!;
}
