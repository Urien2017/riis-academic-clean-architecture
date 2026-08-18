# Seeders des programmes officiels

Le seeder officiel des programmes se trouve dans :

`src/RIIS.Academic.Infrastructure/Persistence/Seeders/RiisAcademicOfficialProgrammeSeeder.cs`

Il lit la ressource embarquée :

`src/RIIS.Academic.Infrastructure/Persistence/Seeders/Data/official-programmes-2025-2026.json`

## Hiérarchie créée

Le seeder respecte la hiérarchie métier actuelle :

```text
CycleFormationFiliere
  -> MaquettePedagogique
      -> SemestrePedagogique
          -> UniteEnseignement
              -> ElementConstitutif
```

## Données présentes

La première version contient les programmes BTS 2025-2026 extraits du fichier source `programme-bts1.json`.

Elle contient maintenant :

- 9 maquettes BTS ;
- 2 maquettes PREPA officielles extraites du fichier `Programme_PREPA_avec_UE 2026.docx` ;
- 4 maquettes LICENCE de transition ;
- 67 semestres ;
- 480 UE ;
- 867 EC.

Attention : la source disponible ne contient pas le semestre 2 pour `BTS-ELT`. Le seeder ne le fabrique pas automatiquement.

## Cas PREPA

Les maquettes PREPA ajoutées sont :

- `PREPA-MANAGEMENT-ECONOMIE` ;
- `PREPA-INGENIEUR`.

Elles sont générées depuis le programme pédagogique PREPA officiel fourni dans
`Programme_PREPA_avec_UE 2026.docx`. Les crédits des UE correspondent à la somme
des crédits des EC du document source. Les volumes horaires absents dans le
document restent à `0` dans la ressource seedée.

## Cas LICENCE

Quatre maquettes LICENCE de transition ont été ajoutées pour couvrir les étudiants présents dans `student-lists-2025-2026.json` :

- `LICENCE-BAT` ;
- `LICENCE-COFI` ;
- `LICENCE-GESTION` ;
- `LICENCE-IGL`.

Elles permettent d'éviter qu'une inscription LICENCE se retrouve sans maquette correspondante lors des imports et des traitements académiques.

Comme pour PREPA, elles devront être remplacées ou corrigées dès réception du programme LMD officiel.

## Règle importante

Le seeder ne crée pas de données pédagogiques artificielles :

- pas de `UE-TEST-*` ;
- pas de `UE-NOTES-*` ;
- pas de `EC-TEST-*` ;
- pas de maquette créée uniquement pour supporter des notes.

Si un parcours, une maquette, une UE ou un EC manque, la donnée source officielle doit être corrigée ou complétée avant d'insérer les notes.

## Appel manuel

Pour charger les programmes officiels :

```csharp
await RiisAcademicOfficialProgrammeSeeder.SeedOfficialProgrammes2025_2026Async(dbContext);
```
