SET XACT_ABORT ON;
SET NOCOUNT ON;
SET ANSI_NULLS ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET QUOTED_IDENTIFIER ON;
SET NUMERIC_ROUNDABORT OFF;

BEGIN TRANSACTION;

CREATE TABLE #Maquettes (Id bigint NOT NULL PRIMARY KEY);
CREATE TABLE #Semestres (Id bigint NOT NULL PRIMARY KEY);
CREATE TABLE #UnitesEnseignement (Id bigint NOT NULL PRIMARY KEY);
CREATE TABLE #ElementsConstitutifs (Id bigint NOT NULL PRIMARY KEY);
CREATE TABLE #EvaluationsAcademiques (Id bigint NOT NULL PRIMARY KEY);
CREATE TABLE #Inscriptions (Id bigint NOT NULL PRIMARY KEY);

INSERT INTO #Maquettes(Id)
SELECT DISTINCT m.Id
FROM dbo.MaquettesPedagogiques m
JOIN dbo.SemestresPedagogiques s ON s.MaquettePedagogiqueId = m.Id
JOIN dbo.UnitesEnseignement ue ON ue.SemestrePedagogiqueId = s.Id
WHERE ue.Code LIKE 'UE-NOTES-%'
   OR ue.Code LIKE 'UE-TEST-%';

INSERT INTO #Semestres(Id)
SELECT s.Id
FROM dbo.SemestresPedagogiques s
WHERE s.MaquettePedagogiqueId IN (SELECT Id FROM #Maquettes);

INSERT INTO #UnitesEnseignement(Id)
SELECT ue.Id
FROM dbo.UnitesEnseignement ue
WHERE ue.SemestrePedagogiqueId IN (SELECT Id FROM #Semestres);

INSERT INTO #ElementsConstitutifs(Id)
SELECT ec.Id
FROM dbo.ElementsConstitutifs ec
WHERE ec.UniteEnseignementId IN (SELECT Id FROM #UnitesEnseignement);

INSERT INTO #EvaluationsAcademiques(Id)
SELECT ev.Id
FROM dbo.EvaluationsAcademiques ev
WHERE ev.ElementConstitutifId IN (SELECT Id FROM #ElementsConstitutifs);

INSERT INTO #Inscriptions(Id)
SELECT i.Id
FROM dbo.Inscriptions i
WHERE i.MaquettePedagogiqueId IN (SELECT Id FROM #Maquettes);

UPDATE dbo.EvaluationsAcademiques
SET EvaluationRemplaceeId = NULL
WHERE EvaluationRemplaceeId IN (SELECT Id FROM #EvaluationsAcademiques);

DELETE FROM dbo.NotesEvaluations
WHERE EvaluationAcademiqueId IN (SELECT Id FROM #EvaluationsAcademiques);

DELETE FROM dbo.ResultatsElementsConstitutifs
WHERE ElementConstitutifId IN (SELECT Id FROM #ElementsConstitutifs);

DELETE FROM dbo.ResultatsUnitesEnseignement
WHERE UniteEnseignementId IN (SELECT Id FROM #UnitesEnseignement);

DELETE FROM dbo.ResultatsSemestres
WHERE SemestrePedagogiqueId IN (SELECT Id FROM #Semestres);

DELETE FROM dbo.ResultatsAnnuels
WHERE InscriptionId IN (SELECT Id FROM #Inscriptions);

DELETE pvl
FROM dbo.ProcesVerbauxLignes pvl
JOIN dbo.ProcesVerbaux pv ON pv.Id = pvl.ProcesVerbalId
WHERE pv.SemestrePedagogiqueId IN (SELECT Id FROM #Semestres);

DELETE FROM dbo.ProcesVerbaux
WHERE SemestrePedagogiqueId IN (SELECT Id FROM #Semestres);

DELETE FROM dbo.EvaluationsAcademiques
WHERE Id IN (SELECT Id FROM #EvaluationsAcademiques);

DELETE FROM dbo.ElementsConstitutifs
WHERE Id IN (SELECT Id FROM #ElementsConstitutifs);

DELETE FROM dbo.UnitesEnseignement
WHERE Id IN (SELECT Id FROM #UnitesEnseignement);

UPDATE dbo.Inscriptions
SET MaquettePedagogiqueId = NULL
WHERE MaquettePedagogiqueId IN (SELECT Id FROM #Maquettes);

UPDATE dbo.ClassesPedagogiques
SET MaquettePedagogiqueId = NULL
WHERE MaquettePedagogiqueId IN (SELECT Id FROM #Maquettes);

DELETE FROM dbo.SemestresPedagogiques
WHERE Id IN (SELECT Id FROM #Semestres);

DELETE FROM dbo.MaquettesPedagogiques
WHERE Id IN (SELECT Id FROM #Maquettes);

SELECT 'Maquettes supprimées' AS Objet, COUNT(*) AS Nombre FROM #Maquettes
UNION ALL SELECT 'Semestres supprimés', COUNT(*) FROM #Semestres
UNION ALL SELECT 'UE supprimées', COUNT(*) FROM #UnitesEnseignement
UNION ALL SELECT 'EC supprimés', COUNT(*) FROM #ElementsConstitutifs
UNION ALL SELECT 'Evaluations supprimées', COUNT(*) FROM #EvaluationsAcademiques
UNION ALL SELECT 'Inscriptions détachées des maquettes', COUNT(*) FROM #Inscriptions;

COMMIT TRANSACTION;
