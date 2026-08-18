
namespace RIIS.Academic.Domain;

public abstract class AuditableEntity : Entity
{
    public DateTime CreeLeUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ModifieLeUtc { get; set; }
}
