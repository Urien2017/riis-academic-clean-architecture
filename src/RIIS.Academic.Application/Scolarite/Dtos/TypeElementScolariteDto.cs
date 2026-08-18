using RIIS.Academic.Domain;

namespace RIIS.Academic.Application.Scolarite.Dtos;

public class TypeElementScolariteDto
{
    public long Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Libelle { get; set; } = string.Empty;
    public CategorieTypeElementScolarite Categorie { get; set; } = CategorieTypeElementScolarite.Frais;
    public bool EstPayable { get; set; }
    public bool EstDocumentaire { get; set; }
    public bool EstSoumisValidation { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public int OrdreAffichage { get; set; }
    public bool EstActif { get; set; } = true;
}
