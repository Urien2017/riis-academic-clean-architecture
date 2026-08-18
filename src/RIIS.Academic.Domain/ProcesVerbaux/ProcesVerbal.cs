namespace RIIS.Academic.Domain;

public class ProcesVerbal
{
    public long Id { get; set; }
    public long AnneeAcademiqueId { get; set; }
    public long ParcoursAcademiqueId { get; set; }
    public long ClassePedagogiqueId { get; set; }
    public long? SemestrePedagogiqueId { get; set; }
    public TypeProcesVerbal Type { get; set; }
    public string? CodeSession { get; set; }
    public required string Titre { get; set; }
    public DateTime DateEditionUtc { get; set; } = DateTime.UtcNow;
    public bool EstDefinitif { get; set; }
    public string? CheminFichier { get; set; }
    public string? Observation { get; set; }

    public AnneeAcademique AnneeAcademique { get; set; } = null!;
    public ParcoursAcademique ParcoursAcademique { get; set; } = null!;
    public ClassePedagogique ClassePedagogique { get; set; } = null!;
    public SemestrePedagogique? SemestrePedagogique { get; set; }
    public ICollection<ProcesVerbalLigne> Lignes { get; set; } = [];
}
