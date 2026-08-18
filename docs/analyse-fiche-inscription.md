# Analyse de la fiche d'inscription

Document source : `D:\PROJETS\Gestion Notes\Fiche d'inscription.pdf`

## Champs visibles dans le formulaire

### En-tête établissement

- nom officiel : RAPUS INTERNATIONAL INSTITUTE SCHOOL ;
- numéro d'autorisation MINESUP ;
- registre commerce ;
- compte bancaire UBA ;
- téléphones ;
- boîte postale ;
- ville et pays ;
- réseaux sociaux ;
- email ;
- localisation.

Ces informations sont maintenant modélisées dans `Etablissements`.

### Identification de l'étudiant

- matricule ;
- photo 4 x 4 ;
- noms ;
- prénoms ;
- date et lieu de naissance ;
- sexe ;
- aptitude médicale ;
- nationalité ;
- région ;
- téléphone principal et secondaire ;
- email ;
- nom du père ;
- nom de la mère ;
- personne à contacter en cas d'urgence ;
- téléphone de la personne à contacter ;
- lieu de résidence.

Ces informations sont réparties entre `Etudiants` et `ContactsUrgence`.

### Inscription académique

- année académique ;
- cycle : BTS, Licence, Master ;
- niveau : 1 à 5 ;
- filière choisie ;
- spécialité.

Ces informations sont portées par `Inscriptions`, avec les référentiels :

- `AnneesAcademiques` ;
- `CyclesFormation` ;
- `NiveauxEtude` ;
- `Filieres` ;
- `Specialites`.

### Admission

- baccalauréat série ;
- année d'obtention ;
- mention ;
- diplôme d'entrée ;
- spécialité du diplôme d'entrée ;
- numéro d'équivalence ;
- diplôme d'équivalence.

Ces informations sont regroupées dans `DossiersAdmission`, lié en 1-1 à `Inscriptions`.

### Cadre administration et signatures

- mention spéciale ;
- tutelle académique ;
- observation ;
- code administratif ;
- signature administration RIIS ;
- nom et signature de l'étudiant ;
- lieu et date de signature.

Les champs administratifs restent dans `Inscriptions`.

Les signatures et dates de validation sont maintenant isolées dans `ValidationsInscriptions`, ce qui permettra plus tard de gérer :

- la préinscription ;
- la validation administrative ;
- la génération d'une fiche PDF officielle ;
- l'audit des validations.

## Affinage appliqué au modèle

Deux tables ont été ajoutées après relecture de la fiche :

- `Etablissements` ;
- `ValidationsInscriptions`.

Le modèle évite de stocker les signatures comme texte dans `Etudiants`. Les signatures sont rattachées à l'inscription annuelle, car un étudiant peut signer une fiche différente chaque année académique.
