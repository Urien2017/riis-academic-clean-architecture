using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Xml.Linq;
using RIIS.Academic.Application.Releves.Dtos;
using RIIS.Academic.Application.Releves.Services;

namespace RIIS.Academic.Infrastructure.Documents;

public class ReleveNoteTemplateWordExportService(IRelevesNotesService relevesNotesService)
    : IReleveNoteTemplateWordExportService
{
    private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

    public async Task<ReleveNoteWordExportDto?> ExporterReleveAnnuelAsync(
        long inscriptionId,
        CancellationToken cancellationToken = default)
    {
        var releve = await relevesNotesService.GenererReleveAnnuelAsync(
            inscriptionId,
            cancellationToken);

        if (releve is null)
        {
            return null;
        }

        return new ReleveNoteWordExportDto
        {
            FileName = BuildFileName(releve),
            Content = BuildDocumentFromTemplate(releve)
        };
    }

    private static byte[] BuildDocumentFromTemplate(ReleveNoteAnnuelDto releve)
    {
        var templatePath = ResolveTemplatePath();
        using var output = new MemoryStream();

        using (var templateArchive = ZipFile.OpenRead(templatePath))
        using (var outputArchive = new ZipArchive(output, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var entry in templateArchive.Entries)
            {
                var outputEntry = outputArchive.CreateEntry(entry.FullName, CompressionLevel.Optimal);

                using var inputStream = entry.Open();
                using var outputStream = outputEntry.Open();

                if (entry.FullName.Equals("word/document.xml", StringComparison.OrdinalIgnoreCase))
                {
                    var documentXml = XDocument.Load(inputStream, LoadOptions.PreserveWhitespace);
                    ReplaceTextPlaceholders(documentXml, releve);
                    ReplaceReleveTablesPlaceholder(documentXml, BuildReleveElements(releve));

                    using var writer = new StreamWriter(outputStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
                    documentXml.Save(writer, SaveOptions.DisableFormatting);
                }
                else
                {
                    inputStream.CopyTo(outputStream);
                }
            }
        }

        return output.ToArray();
    }

    private static string ResolveTemplatePath()
    {
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, "Documents", "Templates", "ReleveNoteTemplate.docx"),
            Path.Combine(AppContext.BaseDirectory, "RIIS.Academic.Infrastructure", "Documents", "Templates", "ReleveNoteTemplate.docx"),
            Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Templates", "ReleveNoteTemplate.docx"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "RIIS.Academic.Infrastructure", "Documents", "Templates", "ReleveNoteTemplate.docx")
        };

        var templatePath = candidates.FirstOrDefault(File.Exists);

        return templatePath
            ?? throw new FileNotFoundException(
                "Le canevas Word 'ReleveNoteTemplate.docx' est introuvable. Vérifiez qu'il est copié dans Documents/Templates.",
                "ReleveNoteTemplate.docx");
    }

    private static void ReplaceTextPlaceholders(XDocument documentXml, ReleveNoteAnnuelDto releve)
    {
        var replacements = new Dictionary<string, string>
        {
            ["{{TITRE}}"] = releve.Titre,
            ["{{CYCLE_FORMATION}}"] = releve.CycleFormationLibelle,
            ["{{ANNEE_ACADEMIQUE}}"] = releve.AnneeAcademiqueLibelle,
            ["{{ETUDIANT_NOM_COMPLET}}"] = releve.EtudiantNomComplet,
            ["{{MATRICULE}}"] = releve.Matricule,
            ["{{DATE_NAISSANCE}}"] = releve.DateNaissance == default
                ? string.Empty
                : releve.DateNaissance.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            ["{{LIEU_NAISSANCE}}"] = releve.LieuNaissance,
            ["{{FILIERE}}"] = releve.FiliereLibelle,
            ["{{SPECIALITE}}"] = releve.SpecialiteLibelle ?? string.Empty,
            ["{{NIVEAU}}"] = releve.NiveauLibelle,
            ["{{MOYENNE_ANNUELLE}}"] = FormatNote(releve.MoyenneAnnuelle),
            ["{{CREDITS_CAPITALISES}}"] = FormatCredit(releve.CreditsCapitalises),
            ["{{CREDITS_REQUIS}}"] = FormatCredit(releve.CreditsRequis),
            ["{{DECISION}}"] = releve.Decision,
            ["{{MENTION}}"] = releve.Mention,
            ["{{DATE_EDITION}}"] = DateTime.Today.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
        };

        foreach (var textElement in documentXml.Descendants(W + "t"))
        {
            var value = textElement.Value;

            foreach (var replacement in replacements)
            {
                value = value.Replace(replacement.Key, replacement.Value, StringComparison.Ordinal);
            }

            textElement.Value = value;
        }
    }

    private static void ReplaceReleveTablesPlaceholder(
        XDocument documentXml,
        IReadOnlyCollection<XElement> releveElements)
    {
        var placeholderTable = documentXml
            .Descendants(W + "tbl")
            .FirstOrDefault(table => table
                .Descendants(W + "t")
                .Any(text => text.Value.Contains("{{RELEVE_TABLES}}", StringComparison.Ordinal)));

        if (placeholderTable is not null)
        {
            placeholderTable.ReplaceWith(releveElements.Cast<object>().ToArray());
            return;
        }

        var placeholderParagraph = documentXml
            .Descendants(W + "p")
            .FirstOrDefault(paragraph => paragraph
                .Descendants(W + "t")
                .Any(text => text.Value.Contains("{{RELEVE_TABLES}}", StringComparison.Ordinal)));

        if (placeholderParagraph is not null)
        {
            placeholderParagraph.ReplaceWith(releveElements.Cast<object>().ToArray());
            return;
        }

        throw new InvalidOperationException("Le placeholder '{{RELEVE_TABLES}}' est introuvable dans le canevas Word.");
    }

    private static List<XElement> BuildReleveElements(ReleveNoteAnnuelDto releve)
    {
        var elements = new List<XElement>();

        if (releve.Semestres.Count == 0)
        {
            elements.Add(ParseParagraph("Aucune note disponible pour cette inscription.", "Heading1"));
            return elements;
        }

        foreach (var semestre in releve.Semestres.OrderBy(x => x.Numero))
        {
            elements.Add(ParseParagraph(semestre.Libelle, "Heading1"));
            elements.Add(XElement.Parse(NotesTable(semestre), LoadOptions.PreserveWhitespace));
            elements.Add(ParseParagraph(string.Empty, "Normal"));
        }

        elements.Add(ParseParagraph("RÉCAPITULATIF", "Heading1"));
        elements.Add(XElement.Parse(ResumeTable(releve), LoadOptions.PreserveWhitespace));
        elements.Add(ParseParagraph(string.Empty, "Normal"));

        return elements;
    }

    private static XElement ParseParagraph(string text, string style)
        => XElement.Parse(Paragraph(text, style), LoadOptions.PreserveWhitespace);

    private static string NotesTable(ReleveNoteSemestreDto semestre)
    {
        var columnWidths = new[] { 1800, 850, 2850, 750, 750, 850, 850, 660 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));
        table.Append(Row([
            Cell("UE (Unité d'enseignement)", columnWidths[0], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Code", columnWidths[1], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("EC (Élément constitutif)", columnWidths[2], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Session", columnWidths[3], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Note/20", columnWidths[4], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Décision", columnWidths[5], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Crédit/30", columnWidths[6], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Grade", columnWidths[7], bold: true, fill: "1F4E79", textColor: "FFFFFF")
        ], repeatHeader: true));

        var lignesParUe = semestre.Lignes
            .GroupBy(x => x.UniteEnseignementLibelle)
            .ToList();

        if (lignesParUe.Count == 0)
        {
            table.Append(Row([
                Cell("Aucune note disponible", columnWidths.Sum(), gridSpan: columnWidths.Length, align: "left")
            ]));
        }
        else
        {
            foreach (var groupeUe in lignesParUe)
            {
                var lignes = groupeUe.ToList();

                for (var index = 0; index < lignes.Count; index++)
                {
                    var ligne = lignes[index];
                    var isFirstLineForUe = index == 0;

                    table.Append(Row([
                        Cell(
                            isFirstLineForUe ? ligne.UniteEnseignementLibelle : string.Empty,
                            columnWidths[0],
                            vMerge: isFirstLineForUe ? "restart" : "continue",
                            align: "left"),
                        Cell(ligne.ElementConstitutifCode ?? string.Empty, columnWidths[1]),
                        Cell(ligne.ElementConstitutifLibelle, columnWidths[2], align: "left"),
                        Cell(ligne.Session, columnWidths[3]),
                        Cell(FormatNote(ligne.NoteSur20), columnWidths[4]),
                        Cell(ligne.Decision, columnWidths[5]),
                        Cell(FormatCredit(ligne.CreditsCapitalises), columnWidths[6]),
                        Cell(ligne.Grade, columnWidths[7])
                    ]));
                }
            }
        }

        table.Append(Row([
            Cell("Total", columnWidths.Take(4).Sum(), bold: true, gridSpan: 4, fill: "F4F6F8", align: "right"),
            Cell(FormatNote(semestre.TotalNotes), columnWidths[4], bold: true, fill: "F4F6F8"),
            Cell(string.Empty, columnWidths[5], fill: "F4F6F8"),
            Cell(FormatCredit(semestre.CreditsCapitalises), columnWidths[6], bold: true, fill: "F4F6F8"),
            Cell(string.Empty, columnWidths[7], fill: "F4F6F8")
        ]));

        table.Append(Row([
            Cell("Moyenne", columnWidths.Take(4).Sum(), bold: true, gridSpan: 4, fill: "F4F6F8", align: "right"),
            Cell(FormatNote(semestre.Moyenne), columnWidths[4], bold: true, fill: "F4F6F8"),
            Cell(string.Empty, columnWidths[5], fill: "F4F6F8"),
            Cell(FormatCredit(semestre.CreditsCapitalises), columnWidths[6], bold: true, fill: "F4F6F8"),
            Cell(semestre.Grade, columnWidths[7], bold: true, fill: "F4F6F8")
        ]));

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string ResumeTable(ReleveNoteAnnuelDto releve)
    {
        var columnWidths = new[] { 2200, 1800, 2200, 3160 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));
        table.Append(Row([
            Cell("Période", columnWidths[0], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Moyenne", columnWidths[1], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Crédits capitalisés", columnWidths[2], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Décision / Mention", columnWidths[3], bold: true, fill: "1F4E79", textColor: "FFFFFF")
        ], repeatHeader: true));

        foreach (var item in releve.Resume)
        {
            var decision = item.Libelle == "ANNUELLE"
                ? $"{releve.Decision} - {releve.Mention}"
                : string.Empty;

            table.Append(Row([
                Cell(item.Libelle, columnWidths[0], align: "left"),
                Cell(FormatNote(item.Moyenne), columnWidths[1]),
                Cell(FormatCredit(item.CreditsCapitalises), columnWidths[2]),
                Cell(decision, columnWidths[3], align: "left")
            ]));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string TableStart(int[] columnWidths)
    {
        var tableWidth = columnWidths.Sum().ToString(CultureInfo.InvariantCulture);
        var grid = string.Join(string.Empty, columnWidths.Select(width => $"<w:gridCol w:w=\"{width}\"/>"));

        return $"""
<w:tbl xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:tblPr>
    <w:tblW w:w="{tableWidth}" w:type="dxa"/>
    <w:jc w:val="center"/>
    <w:tblBorders>
      <w:top w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:left w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:bottom w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:right w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:insideH w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
      <w:insideV w:val="single" w:sz="4" w:space="0" w:color="BFBFBF"/>
    </w:tblBorders>
    <w:tblLayout w:type="fixed"/>
    <w:tblCellMar>
      <w:top w:w="70" w:type="dxa"/>
      <w:left w:w="70" w:type="dxa"/>
      <w:bottom w:w="70" w:type="dxa"/>
      <w:right w:w="70" w:type="dxa"/>
    </w:tblCellMar>
  </w:tblPr>
  <w:tblGrid>{grid}</w:tblGrid>
""";
    }

    private static string Row(IReadOnlyCollection<string> cells, bool repeatHeader = false)
    {
        var rowProperties = repeatHeader
            ? """<w:trPr><w:tblHeader w:val="true"/></w:trPr>"""
            : string.Empty;

        return $"<w:tr>{rowProperties}{string.Join(string.Empty, cells)}</w:tr>";
    }

    private static string Cell(
        string text,
        int width,
        bool bold = false,
        string? textColor = null,
        string? fill = null,
        string align = "center",
        int? gridSpan = null,
        string? vMerge = null)
    {
        var properties = new StringBuilder("<w:tcPr>");
        properties.Append($"<w:tcW w:w=\"{width}\" w:type=\"dxa\"/>");

        if (gridSpan is not null)
        {
            properties.Append($"<w:gridSpan w:val=\"{gridSpan}\"/>");
        }

        if (!string.IsNullOrWhiteSpace(vMerge))
        {
            properties.Append(vMerge == "continue"
                ? "<w:vMerge/>"
                : $"<w:vMerge w:val=\"{vMerge}\"/>");
        }

        if (!string.IsNullOrWhiteSpace(fill))
        {
            properties.Append($"<w:shd w:fill=\"{fill}\"/>");
        }

        properties.Append("<w:vAlign w:val=\"center\"/></w:tcPr>");

        return $"""
<w:tc>
  {properties}
  {Paragraph(text, bold ? "TableBold" : "TableText", align, textColor)}
</w:tc>
""";
    }

    private static string Paragraph(
        string text,
        string style,
        string align = "left",
        string? textColor = null)
    {
        var paragraphProperties = style switch
        {
            "Heading1" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:before=\"180\" w:after=\"100\"/></w:pPr>",
            "TableText" or "TableBold" => $"<w:pPr><w:jc w:val=\"{align}\"/><w:spacing w:after=\"0\"/></w:pPr>",
            _ => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"100\"/></w:pPr>"
        };

        var bold = style is "Heading1" or "TableBold" ? "<w:b/>" : string.Empty;
        var size = style switch
        {
            "Heading1" => "22",
            "TableText" or "TableBold" => "17",
            _ => "20"
        };
        var color = textColor ?? (style == "Heading1" ? "1F4E79" : "000000");

        return $"""
<w:p xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  {paragraphProperties}
  <w:r>
    <w:rPr>
      {bold}
      <w:sz w:val="{size}"/>
      <w:color w:val="{color}"/>
    </w:rPr>
    {TextRuns(text)}
  </w:r>
</w:p>
""";
    }

    private static string TextRuns(string value)
    {
        var lines = value
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n');

        var text = new StringBuilder();

        for (var index = 0; index < lines.Length; index++)
        {
            if (index > 0)
            {
                text.Append("<w:br/>");
            }

            text.Append($"<w:t xml:space=\"preserve\">{Escape(lines[index])}</w:t>");
        }

        return text.ToString();
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
            new[] { "releve-modele", releve.Matricule, releve.EtudiantNomComplet, releve.AnneeAcademiqueLibelle }
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
}
