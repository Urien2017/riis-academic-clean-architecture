namespace RIIS.Academic.Domain;

public class SemestrePedagogique
{
    public long Id { get; set; }
    public long MaquettePedagogiqueId { get; set; }
    public long? NiveauEtudeId { get; set; }
    public byte Numero { get; set; }
    public required string Libelle { get; set; }
    public decimal CreditsAttendus { get; set; } = 30m;
    public short VolumeHoraireAttendu { get; set; } = 450;
    public short OrdreAffichage { get; set; }

    public MaquettePedagogique MaquettePedagogique { get; set; } = null!;
    public NiveauEtude? NiveauEtude { get; set; }
    public ICollection<UniteEnseignement> UnitesEnseignement { get; set; } = [];
    public ICollection<ResultatSemestre> ResultatsSemestres { get; set; } = [];
    public ICollection<ProcesVerbal> ProcesVerbaux { get; set; } = [];
}
