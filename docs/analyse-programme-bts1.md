# Analyse du programme BTS extrait du fichier Word

Document source : `D:\PROJETS\Gestion Notes\PROGRAMME DES SPÉCIALITÉS EN BTS1.docx`

## Structure détectée

Le document contient un grand tableau Word de 513 lignes et 16 colonnes physiques. Les colonnes visibles sont fusionnées ; les colonnes logiques exploitables sont :

- spécialité ;
- semestre ;
- code UE ;
- intitulé de l'unité d'enseignement ;
- cours / élément constitutif ;
- crédit ou coefficient de l'élément constitutif ;
- crédit total de l'UE ;
- volume horaire de l'élément constitutif ;
- volume horaire total de l'UE.

Le fichier intitulé BTS1 contient aussi les semestres 3 et 4. Le modèle CodeFirst retient donc une maquette BTS complète sur 4 semestres, avec rattachement possible aux niveaux d'étude :

- semestres 1 et 2 : Niveau 1 ;
- semestres 3 et 4 : Niveau 2.

## Spécialités détectées

1. Banque et finance
2. Comptabilité et gestion des entreprises
3. Gestion des ressources humaines
4. Génie logiciel
5. Réseaux et Sécurité
6. Bâtiment
7. Travaux publics
8. Géomètre Topographe
9. Electrotechnique

## Modélisation retenue

La structure pédagogique est séparée de l'inscription administrative :

```text
CycleFormation
  -> Specialite
      -> MaquettePedagogique
          -> SemestrePedagogique
              -> UniteEnseignement
                  -> ElementConstitutif
```

`Inscription` possède maintenant un lien optionnel vers `MaquettePedagogique`. Ce lien permettra de savoir quelle version du programme s'applique à un étudiant inscrit.

## Tables ajoutées

- `MaquettesPedagogiques`
- `SemestresPedagogiques`
- `UnitesEnseignement`
- `ElementsConstitutifs`

## Champs importants ajoutés

### MaquettePedagogique

- `CycleFormationId`
- `SpecialiteId`
- `Code`
- `Libelle`
- `Version`
- `Statut`
- `DateDebutValidite`
- `DateFinValidite`
- `SourceDocument`
- `Observation`

### SemestrePedagogique

- `MaquettePedagogiqueId`
- `NiveauEtudeId`
- `Numero`
- `Libelle`
- `CreditsAttendus`
- `VolumeHoraireAttendu`
- `OrdreAffichage`

### UniteEnseignement

- `SemestrePedagogiqueId`
- `Code`
- `Libelle`
- `Credits`
- `VolumeHoraire`
- `OrdreAffichage`
- `EstObligatoire`

### ElementConstitutif

- `UniteEnseignementId`
- `Code`
- `Libelle`
- `Type`
- `Credits`
- `Coefficient`
- `VolumeHoraire`
- `OrdreAffichage`
- `EstObligatoire`
- `Observation`

## Points à confirmer avant import complet des lignes du programme

1. Dans le document, la première valeur numérique après le cours semble représenter le crédit ou coefficient de l'EC. Pour les calculs de notes, le modèle conserve les deux champs `Credits` et `Coefficient`. À l'import, on pourra initialiser `Coefficient = Credits` si aucune autre règle officielle n'existe.
2. Le regroupement exact des spécialités dans les filières reste à confirmer. Pour l'instant, le modèle ne force pas une filière automatique.
3. Deux anomalies probables ont été repérées dans le document source :
   - CGE125, `Méthodologie de rédaction du rapport de stage` : la cellule contient `615`, probablement `15`.
   - Electrotechnique : plusieurs lignes affichent `Semestren1`, probablement `Semestre 2`.

## Lien avec les règles de notes déjà confirmées

Le modèle prépare la suite du module de notes :

- une note appartient à un élément constitutif ;
- un élément constitutif appartient à une UE ;
- une UE appartient à un semestre ;
- pour BTS, la formule confirmée reste : `CCON 20% + CC 10% + SN/SR 70%`.
