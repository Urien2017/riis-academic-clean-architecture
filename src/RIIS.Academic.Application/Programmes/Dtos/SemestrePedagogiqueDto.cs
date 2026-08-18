namespace RIIS.Academic.Application.Programmes.Dtos;

public class SemestrePedagogiqueDto
{
    public long Id { get; set; }
    public long MaquettePedagogiqueId { get; set; }
    public string MaquettePedagogiqueLibelle { get; set; } = string.Empty;
    public long? NiveauEtudeId { get; set; }
    public string? NiveauEtudeLibelle { get; set; }
    public byte Numero { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public decimal CreditsAttendus { get; set; } = 30m;
    public short VolumeHoraireAttendu { get; set; } = 450;
    public short OrdreAffichage { get; set; }
    public int NombreUnitesEnseignement { get; set; }
}
