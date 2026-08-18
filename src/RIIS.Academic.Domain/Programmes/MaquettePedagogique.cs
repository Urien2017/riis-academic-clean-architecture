using System.ComponentModel.DataAnnotations.Schema;

namespace RIIS.Academic.Domain;

public class MaquettePedagogique
{
    public long Id { get; set; }
    [NotMapped]
    public long ParcoursAcademiqueId { get; set; }
    public long CycleFormationId { get; set; }
    public long NiveauEtudeId { get; set; }
    public long FiliereId { get; set; }
    public long SpecialiteId { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public required string Version { get; set; }
    public StatutMaquettePedagogique Statut { get; set; } = StatutMaquettePedagogique.Brouillon;
    public DateOnly? DateDebutValidite { get; set; }
    public DateOnly? DateFinValidite { get; set; }
    public string? SourceDocument { get; set; }
    public string? Observation { get; set; }
    public DateTime CreeLeUtc { get; set; } = DateTime.UtcNow;

    public CycleFormation CycleFormation { get; set; } = null!;
    public NiveauEtude NiveauEtude { get; set; } = null!;
    public Filiere Filiere { get; set; } = null!;
    public Specialite Specialite { get; set; } = null!;
    public ICollection<SemestrePedagogique> Semestres { get; set; } = [];
    public ICollection<Inscription> Inscriptions { get; set; } = [];
}
