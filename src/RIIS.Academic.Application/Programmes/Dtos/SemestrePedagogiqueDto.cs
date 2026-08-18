namespace RIIS.Academic.Application.Programmes.Dtos;

public class SemestrePedagogiqueDto
{
    public long Id { get; set; }
    public long MaquettePedagogiqueId { get; set; }
    public string MaquettePedagogiqueLibelle { get; set; } = string.Empty;
    public byte NumeroSemestre { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public decimal CreditsUE { get; set; }
    public short VolumeHoraireUE { get; set; }
    public short OrdreAffichage { get; set; }
    public int NombreUnitesEnseignement { get; set; }
}
