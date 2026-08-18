# Analyse des PV et règles de calcul des notes

Documents sources analysés :

- `PROCES VERBAL DES NOTES DES CONTROLES CONTINUS*.docx`
- `PROCES VERBAL DES NOTES DU SEMESTRE *.docx`
- `PROCES VERBAL DES NOTES DE RATTRAPAGE SEM *.docx`

## Structure observée dans les PV

Les fichiers Word montrent trois familles principales :

1. PV des contrôles continus :
   - colonnes par EC : `CCON`, `CC`, `Moy` ;
   - la moyenne affichée est une moyenne intermédiaire des contrôles.
2. PV de session normale :
   - colonnes par EC : `CC`, `SN`, `Moy` ;
   - certains tableaux ajoutent `Moy Gle` et `Rang`.
3. PV de rattrapage :
   - colonnes par EC avec uniquement la note de rattrapage ;
   - les cellules `//` représentent l'absence justifiée.

Le modèle doit donc conserver les notes détaillées, et ne pas stocker uniquement une moyenne finale.

## Règles métier confirmées

- Une note appartient à un EC.
- Un EC appartient à une UE.
- Une UE appartient à un semestre.
- Moyenne CCON d'un EC = somme des notes CCON / nombre de CCON.
- Moyenne CC d'un EC = somme des notes CC / nombre de CC.
- Moyenne SN d'un EC = note de session normale du semestre concerné.
- Moyenne SR d'un EC = note de rattrapage du même EC.
- Moyenne semestrielle d'un EC avant rattrapage :
  `Moy.CCON * 20% + Moy.CC * 10% + Moy.SN * 70%`.
- Moyenne semestrielle d'un EC après rattrapage :
  `Moy.CCON * 20% + Moy.CC * 10% + Moy.SR * 70%`.
- Le SR remplace seulement la partie SN évaluée à 70%.
- Un étudiant va au rattrapage pour tout EC dont la moyenne retenue est inférieure à `10/20`.
- Les crédits sont portés par les EC.
- Les crédits acquis sont additionnés.
- Il faut `30` crédits pour valider un semestre.
- Il faut `60` crédits pour valider une année.
- Un étudiant peut avoir une moyenne semestrielle suffisante, mais aller au rattrapage si le quota de crédits validés est insuffisant.
- Le coefficient d'un EC est `1` et ne doit pas influencer le calcul de la moyenne.
- Un PV concerne toute la classe et conserve les détails des colonnes de notes.

## Mapping des sessions normales

Pour la PREPA :

- première année : `SN1` et `SN2` ;
- deuxième année : `SN3` et `SN4`.

Par extension, le numéro de session normale correspond au numéro du semestre pédagogique.

## Tables ajoutées pour les notes et PV

- `ClassesPedagogiques`
- `EvaluationsAcademiques`
- `NotesEvaluations`
- `ResultatsElementsConstitutifs`
- `ResultatsUnitesEnseignement`
- `ResultatsSemestres`
- `ResultatsAnnuels`
- `ProcesVerbaux`
- `ProcesVerbauxLignes`

## Rôle des principales tables

### `EvaluationsAcademiques`

Décrit une évaluation prévue pour un EC :

- CCON ;
- CC ;
- SN ;
- SR.

Le champ `Numero` permet de gérer `CCON1`, `CCON2`, `SN1`, `SN2`, `SN3`, etc.

### `NotesEvaluations`

Stocke la note brute d'un étudiant pour une évaluation donnée.

Le champ `StatutPresence` permet de distinguer :

- présent ;
- absence justifiée ;
- absence non justifiée ;
- dispense.

### `ResultatsElementsConstitutifs`

Stocke les moyennes calculées par EC :

- `MoyenneControleContinu`
- `MoyenneControleConnaissance`
- `MoyenneSessionNormale`
- `MoyenneSessionRattrapage`
- `MoyenneAvantRattrapage`
- `MoyenneApresRattrapage`
- `MoyenneRetenue`
- `CreditsAcquis`
- `StatutValidation`
- `EstEligibleRattrapage`

### `ResultatsUnitesEnseignement`

Agrège les résultats des EC d'une UE.

Les crédits d'une UE sont la somme des crédits acquis dans ses EC.

### `ResultatsSemestres`

Agrège les résultats d'un semestre :

- moyenne CCON ;
- moyenne CC ;
- moyenne SN ;
- moyenne SR ;
- moyenne semestrielle retenue ;
- crédits acquis ;
- crédits requis, par défaut `30` ;
- rang ;
- décision du jury.

### `ResultatsAnnuels`

Agrège deux semestres d'un même niveau :

- moyenne annuelle ;
- crédits acquis ;
- crédits requis, par défaut `60` ;
- rang ;
- décision du jury.

### `ProcesVerbaux` et `ProcesVerbauxLignes`

Un PV est rattaché à une classe pédagogique et éventuellement à un semestre.

`ProcesVerbauxLignes` conserve une ligne officielle par étudiant. Le champ `DetailsNotesJson` permet de figer le détail variable des colonnes du PV, car les documents Word n'ont pas exactement les mêmes colonnes selon le type de PV et la filière.

## Point de vigilance pour la suite

La formule exacte utilisée dans certains anciens PV de session normale semble parfois afficher une moyenne simple `CC/SN`, alors que la règle métier confirmée pour l'application est :

`CCON 20% + CC 10% + SN ou SR 70%`.

Les anciens PV doivent donc être traités comme sources historiques de structure et non comme vérité absolue de calcul.
