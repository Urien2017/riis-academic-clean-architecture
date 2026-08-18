using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using RIIS.Academic.Application.Releves.Dtos;
using RIIS.Academic.Application.Releves.Services;

namespace RIIS.Academic.Infrastructure.Documents;

public class ReleveNoteWordExportService(IRelevesNotesService relevesNotesService) : IReleveNoteWordExportService
{
    public async Task<ReleveNoteWordExportDto?> ExporterReleveAnnuelAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default)
    {
        var releve = await relevesNotesService.GenererReleveAnnuelAsync(inscriptionId, cancellationToken);

        if (releve is null)
        {
            return null;
        }

        return new ReleveNoteWordExportDto
        {
            FileName = BuildFileName(releve),
            Content = BuildDocument(releve)
        };
    }

    private static byte[] BuildDocument(ReleveNoteAnnuelDto releve)
    {
        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddEntry(archive, "[Content_Types].xml", ContentTypesXml);
            AddEntry(archive, "_rels/.rels", RelationshipsXml);
            AddEntry(archive, "word/_rels/document.xml.rels", DocumentRelationshipsXml);
            AddEntry(archive, "word/styles.xml", StylesXml);
            AddEntry(archive, "word/document.xml", BuildDocumentXml(releve));
        }

        return stream.ToArray();
    }

    private static string BuildDocumentXml(ReleveNoteAnnuelDto releve)
    {
        var body = new StringBuilder();

        body.Append(Paragraph("RÉPUBLIQUE DU CAMEROUN", "CenterSmall"));
        body.Append(Paragraph("Paix - Travail - Patrie", "CenterSmall"));
        body.Append(Paragraph("RELEVÉ DE NOTES ANNUEL / ANNUAL TRANSCRIPT", "Title"));
        body.Append(Paragraph($"{releve.CycleFormationLibelle} - {releve.AnneeAcademiqueLibelle}", "Subtitle"));
        body.Append(Paragraph(string.Empty, "Normal"));

        body.Append(InfoTable([
            ("Nom et prénoms", releve.EtudiantNomComplet),
            ("Matricule", releve.Matricule),
            ("Né(e) le", releve.DateNaissance == default ? string.Empty : releve.DateNaissance.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)),
            ("Lieu de naissance", releve.LieuNaissance),
            ("Filière", releve.FiliereLibelle),
            ("Spécialité", releve.SpecialiteLibelle ?? string.Empty),
            ("Niveau", releve.NiveauLibelle),
            ("Année académique", releve.AnneeAcademiqueLibelle)
        ]));

        foreach (var semestre in releve.Semestres.OrderBy(x => x.Numero))
        {
            body.Append(Paragraph(semestre.Libelle, "Heading1"));
            body.Append(NotesTable(semestre));
            body.Append(Paragraph(string.Empty, "Normal"));
        }

        body.Append(Paragraph("RÉCAPITULATIF", "Heading1"));
        body.Append(ResumeTable(releve));
        body.Append(Paragraph(string.Empty, "Normal"));
        body.Append(Paragraph($"DÉCISION : {releve.Decision}    MENTION : {releve.Mention}", "Decision"));
        body.Append(Paragraph(string.Empty, "Normal"));
        body.Append(Paragraph("Fait à Yaoundé, le ____________________", "Right"));
        body.Append(Paragraph("Le Directeur", "Right"));

        return $"""
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:body>
    {body}
    <w:sectPr>
      <w:pgSz w:w="12240" w:h="15840"/>
      <w:pgMar w:top="720" w:right="720" w:bottom="720" w:left="720" w:header="450" w:footer="450" w:gutter="0"/>
    </w:sectPr>
  </w:body>
</w:document>
""";
    }

    private static string NotesTable(ReleveNoteSemestreDto semestre)
    {
        var columnWidths = new[] { 1800, 850, 2850, 750, 750, 850, 850, 660 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));
        table.Append(Row([
            Cell("UE (Unité d'enseignement)", true, columnWidths[0]),
            Cell("Code", true, columnWidths[1]),
            Cell("EC (Élément constitutif)", true, columnWidths[2]),
            Cell("Session", true, columnWidths[3]),
            Cell("Note/20", true, columnWidths[4]),
            Cell("Décision", true, columnWidths[5]),
            Cell("Crédit/30", true, columnWidths[6]),
            Cell("Grade", true, columnWidths[7])
        ], "HeaderRow"));

        var lignes = semestre.Lignes.ToList();
        for (var index = 0; index < lignes.Count; index++)
        {
            var ligne = lignes[index];
            var isFirstLineForUe = index == 0
                || !string.Equals(
                    lignes[index - 1].UniteEnseignementLibelle,
                    ligne.UniteEnseignementLibelle,
                    StringComparison.Ordinal);

            table.Append(Row([
                Cell(
                    isFirstLineForUe ? ligne.UniteEnseignementLibelle : string.Empty,
                    width: columnWidths[0],
                    vMerge: isFirstLineForUe ? "restart" : "continue"),
                Cell(ligne.ElementConstitutifCode ?? string.Empty, width: columnWidths[1]),
                Cell(ligne.ElementConstitutifLibelle, width: columnWidths[2]),
                Cell(ligne.Session, width: columnWidths[3]),
                Cell(FormatNote(ligne.NoteSur20), width: columnWidths[4]),
                Cell(ligne.Decision, width: columnWidths[5]),
                Cell(FormatCredit(ligne.CreditsCapitalises), width: columnWidths[6]),
                Cell(ligne.Grade, width: columnWidths[7])
            ]));
        }

        table.Append(Row([
            Cell("Total", true, columnWidths[0]),
            Cell(string.Empty, width: columnWidths[1]),
            Cell(string.Empty, width: columnWidths[2]),
            Cell(string.Empty, width: columnWidths[3]),
            Cell(FormatNote(semestre.TotalNotes), true, columnWidths[4]),
            Cell(string.Empty, width: columnWidths[5]),
            Cell(FormatCredit(semestre.CreditsCapitalises), true, columnWidths[6]),
            Cell(string.Empty, width: columnWidths[7])
        ], "TotalRow"));

        table.Append(Row([
            Cell("Moyenne", true, columnWidths[0]),
            Cell(string.Empty, width: columnWidths[1]),
            Cell(string.Empty, width: columnWidths[2]),
            Cell(string.Empty, width: columnWidths[3]),
            Cell(FormatNote(semestre.Moyenne), true, columnWidths[4]),
            Cell(string.Empty, width: columnWidths[5]),
            Cell(FormatCredit(semestre.CreditsCapitalises), true, columnWidths[6]),
            Cell(semestre.Grade, true, columnWidths[7])
        ], "TotalRow"));

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string ResumeTable(ReleveNoteAnnuelDto releve)
    {
        var columnWidths = new[] { 2200, 1800, 2200, 3160 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));
        table.Append(Row([
            Cell("Période", true, columnWidths[0]),
            Cell("Moyenne", true, columnWidths[1]),
            Cell("Crédits capitalisés", true, columnWidths[2]),
            Cell("Décision / Mention", true, columnWidths[3])
        ], "HeaderRow"));

        foreach (var item in releve.Resume)
        {
            var decision = item.Libelle == "ANNUELLE"
                ? $"{releve.Decision} - {releve.Mention}"
                : string.Empty;

            table.Append(Row([
                Cell(item.Libelle, width: columnWidths[0]),
                Cell(FormatNote(item.Moyenne), width: columnWidths[1]),
                Cell(FormatCredit(item.CreditsCapitalises), width: columnWidths[2]),
                Cell(decision, width: columnWidths[3])
            ]));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string InfoTable(IReadOnlyCollection<(string Label, string Value)> items)
    {
        var columnWidths = new[] { 2200, 7160 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));

        foreach (var item in items)
        {
            table.Append(Row([
                Cell(item.Label, true, columnWidths[0]),
                Cell(item.Value, width: columnWidths[1])
            ]));
        }

        table.Append("</w:tbl>");
        table.Append(Paragraph(string.Empty, "Normal"));
        return table.ToString();
    }

    private static string TableStart(int[] columnWidths)
    {
        var grid = string.Join(string.Empty, columnWidths.Select(width => $"<w:gridCol w:w=\"{width}\"/>"));
        var tableWidth = columnWidths.Sum().ToString(CultureInfo.InvariantCulture);

        return $"""
<w:tbl>
  <w:tblPr>
    <w:tblW w:w="{tableWidth}" w:type="dxa"/>
    <w:jc w:val="center"/>
    <w:tblBorders>
      <w:top w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:left w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:bottom w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:right w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:insideH w:val="single" w:sz="4" w:space="0" w:color="D9D9D9"/>
      <w:insideV w:val="single" w:sz="4" w:space="0" w:color="D9D9D9"/>
    </w:tblBorders>
    <w:tblLayout w:type="fixed"/>
    <w:tblCellMar>
      <w:top w:w="80" w:type="dxa"/>
      <w:left w:w="90" w:type="dxa"/>
      <w:bottom w:w="80" w:type="dxa"/>
      <w:right w:w="90" w:type="dxa"/>
    </w:tblCellMar>
  </w:tblPr>
  <w:tblGrid>{grid}</w:tblGrid>
""";
    }

    private static string Row(IReadOnlyCollection<string> cells, string? style = null)
    {
        var shading = style switch
        {
            "HeaderRow" => "E8F0FE",
            "TotalRow" => "F4F6F8",
            _ => null
        };

        var rowCells = string.Join(string.Empty, cells.Select(cell => ApplyCellShading(cell, shading)));
        return $"<w:tr>{rowCells}</w:tr>";
    }

    private static string ApplyCellShading(string cell, string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return cell;
        }

        return cell.Replace("<w:vAlign", $"<w:shd w:fill=\"{color}\"/><w:vAlign", StringComparison.Ordinal);
    }

    private static string Cell(
        string text,
        bool bold = false,
        int? width = null,
        string? vMerge = null)
    {
        var properties = new StringBuilder("<w:tcPr>");

        if (width is not null)
        {
            properties.Append($"<w:tcW w:w=\"{width}\" w:type=\"dxa\"/>");
        }

        if (!string.IsNullOrWhiteSpace(vMerge))
        {
            properties.Append(vMerge == "continue"
                ? "<w:vMerge/>"
                : $"<w:vMerge w:val=\"{vMerge}\"/>");
        }

        properties.Append("<w:vAlign w:val=\"center\"/></w:tcPr>");

        return $"""
<w:tc>
  {properties}
  {Paragraph(text, bold ? "TableBold" : "TableText")}
</w:tc>
""";
    }

    private static string Paragraph(string text, string style)
    {
        var paragraphProperties = style switch
        {
            "Title" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"160\"/></w:pPr>",
            "Subtitle" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"180\"/></w:pPr>",
            "Heading1" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:before=\"180\" w:after=\"100\"/></w:pPr>",
            "Right" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:before=\"120\" w:after=\"80\"/></w:pPr>",
            "CenterSmall" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"20\"/></w:pPr>",
            "Decision" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:before=\"180\" w:after=\"120\"/></w:pPr>",
            "TableText" or "TableBold" => "<w:pPr><w:spacing w:after=\"0\"/></w:pPr>",
            _ => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"100\"/></w:pPr>"
        };

        var runProperties = style switch
        {
            "Title" => "<w:rPr><w:b/><w:sz w:val=\"28\"/><w:color w:val=\"1F4E79\"/></w:rPr>",
            "Subtitle" => "<w:rPr><w:b/><w:sz w:val=\"22\"/><w:color w:val=\"404040\"/></w:rPr>",
            "Heading1" => "<w:rPr><w:b/><w:sz w:val=\"22\"/><w:color w:val=\"1F4E79\"/></w:rPr>",
            "CenterSmall" => "<w:rPr><w:sz w:val=\"18\"/><w:color w:val=\"404040\"/></w:rPr>",
            "Decision" => "<w:rPr><w:b/><w:sz w:val=\"22\"/><w:color w:val=\"1F4E79\"/></w:rPr>",
            "TableBold" => "<w:rPr><w:b/><w:sz w:val=\"18\"/></w:rPr>",
            "TableText" => "<w:rPr><w:sz w:val=\"18\"/></w:rPr>",
            _ => "<w:rPr><w:sz w:val=\"20\"/></w:rPr>"
        };

        return $"<w:p>{paragraphProperties}<w:r>{runProperties}<w:t xml:space=\"preserve\">{Escape(text)}</w:t></w:r></w:p>";
    }

    private static string FormatNote(decimal? value)
        => value is null ? string.Empty : value.Value.ToString("0.##", CultureInfo.InvariantCulture);

    private static string FormatCredit(decimal value)
        => value == decimal.Truncate(value)
            ? value.ToString("00", CultureInfo.InvariantCulture)
            : value.ToString("0.##", CultureInfo.InvariantCulture);

    private static string BuildFileName(ReleveNoteAnnuelDto releve)
    {
        var name = string.Join(
            "-",
            new[] { "releve", releve.Matricule, releve.EtudiantNomComplet, releve.AnneeAcademiqueLibelle }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(Slug));

        return $"{name}.docx";
    }

    private static string Slug(string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var chars = normalized
            .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            .Select(c => char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-')
            .ToArray();

        var slug = new string(chars);
        while (slug.Contains("--", StringComparison.Ordinal))
        {
            slug = slug.Replace("--", "-", StringComparison.Ordinal);
        }

        return slug.Trim('-');
    }

    private static string Escape(string value)
        => SecurityElement.Escape(value) ?? string.Empty;

    private static void AddEntry(ZipArchive archive, string path, string content)
    {
        var entry = archive.CreateEntry(path, CompressionLevel.Optimal);
        using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        writer.Write(content);
    }

    private const string ContentTypesXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
  <Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/>
</Types>
""";

    private const string RelationshipsXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
</Relationships>
""";

    private const string DocumentRelationshipsXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships"/>
""";

    private const string StylesXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:docDefaults>
    <w:rPrDefault>
      <w:rPr>
        <w:rFonts w:ascii="Arial" w:hAnsi="Arial"/>
        <w:sz w:val="20"/>
      </w:rPr>
    </w:rPrDefault>
  </w:docDefaults>
</w:styles>
""";
}
