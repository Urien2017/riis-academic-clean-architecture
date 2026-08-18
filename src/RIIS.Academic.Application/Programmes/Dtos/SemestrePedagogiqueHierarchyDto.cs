namespace RIIS.Academic.Application.Programmes.Dtos;

public class SemestrePedagogiqueHierarchyDto
{
    public long Id { get; set; }
    public byte NumeroSemestre { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public decimal CreditsUE { get; set; }
    public short VolumeHoraireUE { get; set; }
    public short OrdreAffichage { get; set; }
    public List<UniteEnseignementHierarchyDto> UnitesEnseignement { get; set; } = [];
}
