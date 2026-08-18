namespace RIIS.Academic.Application.Programmes.Dtos;

public class UniteEnseignementDto
{
    public long Id { get; set; }
    public long SemestrePedagogiqueId { get; set; }
    public string SemestrePedagogiqueLibelle { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public decimal Credits { get; set; }
    public short VolumeHoraire { get; set; }
    public short OrdreAffichage { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public int NombreElementsConstitutifs { get; set; }
}
