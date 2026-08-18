namespace RIIS.Academic.Domain;

public class TypeElementScolarite
{
    public long Id { get; set; }
    public required string Code { get; set; }
    public required string Libelle { get; set; }
    public CategorieTypeElementScolarite Categorie { get; set; } = CategorieTypeElementScolarite.Frais;
    public bool EstPayable { get; set; }
    public bool EstDocumentaire { get; set; }
    public bool EstSoumisValidation { get; set; }
    public bool EstObligatoire { get; set; } = true;
    public int OrdreAffichage { get; set; }
    public bool EstActif { get; set; } = true;

    public ICollection<TarifScolarite> Tarifs { get; set; } = [];
    public ICollection<ElementScolariteEtudiant> ElementsEtudiants { get; set; } = [];
}
