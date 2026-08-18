namespace RIIS.Academic.Application.Programmes.Dtos;

public class SemestrePedagogiqueHierarchyDto
{
    public long Id { get; set; }
    public byte Numero { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public string? NiveauEtudeLibelle { get; set; }
    public decimal CreditsAttendus { get; set; }
    public short VolumeHoraireAttendu { get; set; }
    public short OrdreAffichage { get; set; }
    public List<UniteEnseignementHierarchyDto> UnitesEnseignement { get; set; } = [];
}
