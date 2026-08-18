using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.ProcesVerbaux.Dtos;

public class ProcesVerbalDto
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long ClassePedagogiqueId { get; set; }
    public long? SemestrePedagogiqueId { get; set; }
    public long? CycleFormationId { get; set; }
    public string AnneeAcademiqueLibelle { get; set; } = string.Empty;
    public string CycleFormationLibelle { get; set; } = string.Empty;
    public string ParcoursLibelle { get; set; } = string.Empty;
    public string ClassePedagogiqueLibelle { get; set; } = string.Empty;
    public string SemestrePedagogiqueLibelle { get; set; } = string.Empty;
    public byte? SemestreNumero { get; set; }
    public TypeProcesVerbal Type { get; set; }
    public string TypeLibelle => Type switch
    {
        TypeProcesVerbal.ControleContinu => "Contrôle continu",
        TypeProcesVerbal.SessionNormale => "Session normale",
        TypeProcesVerbal.SessionRattrapage => "Session de rattrapage",
        TypeProcesVerbal.Definitif => "Définitif",
        _ => Type.ToString()
    };
    public string? CodeSession { get; set; }
    public string Titre { get; set; } = string.Empty;
    public DateTime DateEditionUtc { get; set; }
    public bool EstDefinitif { get; set; }
    public string StatutLibelle => EstDefinitif ? "Définitif" : "Provisoire";
    public string? Observation { get; set; }
    public int NombreLignes { get; set; }
    public decimal? MoyenneMin { get; set; }
    public decimal? MoyenneMax { get; set; }
    public int NombreValides { get; set; }
    public int NombreRattrapage { get; set; }
    public int NombreNonDeliberes { get; set; }
    public List<ProcesVerbalLigneDto> Lignes { get; set; } = [];
}
