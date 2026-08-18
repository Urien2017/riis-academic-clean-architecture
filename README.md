
# RIIS Academic - Prototype Clean Architecture

Ce prototype reprend les modèles créés pendant l'analyse des documents RIIS, avec :

- une classe par fichier `.cs` ;
- des dossiers de regroupement par thème ;
- le namespace racine `RIIS.Academic` ;
- aucune classe de domaine marquée `sealed` ;
- une séparation Clean Architecture : `Domain`, `Application`, `Infrastructure`, `Api`, `Web`.

## Projets

- `RIIS.Academic.Domain` : entités métier et enums.
- `RIIS.Academic.Application` : contrats applicatifs et services métier purs.
- `RIIS.Academic.Infrastructure` : Entity Framework Core, SQL Server, configurations et migrations.
- `RIIS.Academic.Api` : API REST ASP.NET Core.
- `RIIS.Academic.Web` : prototype Blazor Web App InteractiveServer.

## Relation cycle / filière / spécialité ouverte

Les filières et spécialités ne sont pas rattachées définitivement à un cycle. Elles sont ouvertes par année académique via :

```text
AnneeAcademique
  -> CycleFormationFiliere
      -> CycleFormation
      -> Filiere
      -> Specialite nullable
```

`Inscription`, `ClassePedagogique` et `MaquettePedagogique` pointent maintenant vers `CycleFormationFiliere`. Cela évite les incohérences du type : une inscription PREPA associée à une filière ouverte uniquement en BTS.

## Commandes utiles

```powershell
dotnet restore RIIS.Academic.slnx
dotnet build RIIS.Academic.slnx
```

Les documents d'analyse sont dans le dossier `docs`.
