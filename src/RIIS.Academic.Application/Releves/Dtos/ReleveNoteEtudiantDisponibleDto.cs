namespace RIIS.Academic.Application.Releves.Dtos;

public class ReleveNoteEtudiantDisponibleDto
{
    public long InscriptionId { get; set; }
    public string Matricule { get; set; } = string.Empty;
    public string EtudiantNomComplet { get; set; } = string.Empty;
    public string ParcoursLibelle { get; set; } = string.Empty;
    public string NiveauLibelle { get; set; } = string.Empty;
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
}
