using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using RIIS.Academic.Application.ProcesVerbaux.Dtos;
using RIIS.Academic.Application.ProcesVerbaux.Services;

namespace RIIS.Academic.Infrastructure.Documents;

public class ProcesVerbalWordExportService(IProcesVerbauxService procesVerbauxService) : IProcesVerbalWordExportService
{
    public async Task<ProcesVerbalWordExportDto?> ExporterProcesVerbalAsync(
        long procesVerbalId,
        CancellationToken cancellationToken = default)
    {
        var procesVerbal = await procesVerbauxService.GetProcesVerbalAsync(
            procesVerbalId,
            cancellationToken);

        if (procesVerbal is null)
        {
            return null;
        }

        return new ProcesVerbalWordExportDto
        {
            FileName = BuildFileName(procesVerbal),
            Content = BuildDocument(procesVerbal)
        };
    }

    private static byte[] BuildDocument(ProcesVerbalDto procesVerbal)
    {
        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddEntry(archive, "[Content_Types].xml", ContentTypesXml);
            AddEntry(archive, "_rels/.rels", RelationshipsXml);
            AddEntry(archive, "word/_rels/document.xml.rels", DocumentRelationshipsXml);
            AddEntry(archive, "word/styles.xml", StylesXml);
            AddEntry(archive, "word/document.xml", BuildDocumentXml(procesVerbal));
        }

        return stream.ToArray();
    }

    private static string BuildDocumentXml(ProcesVerbalDto procesVerbal)
    {
        var body = new StringBuilder();
        var ecHeaders = BuildEcHeaders(procesVerbal);

        body.Append(Paragraph("RIIS ACADEMIC", "CenterSmall"));
        body.Append(Paragraph("GESTION DES NOTES, PV ET RELEVÉS", "CenterSmall"));
        body.Append(Paragraph(procesVerbal.Titre, "Title"));
        body.Append(Paragraph($"{procesVerbal.TypeLibelle} - {procesVerbal.SemestrePedagogiqueLibelle}", "Subtitle"));
        body.Append(Paragraph(string.Empty, "Normal"));

        body.Append(InfoTable([
            ("Cycle", procesVerbal.CycleFormationLibelle),
            ("Parcours", procesVerbal.ParcoursLibelle),
            ("Classe", procesVerbal.ClassePedagogiqueLibelle),
            ("Année académique", procesVerbal.AnneeAcademiqueLibelle),
            ("Session", procesVerbal.CodeSession ?? string.Empty),
            ("Statut", procesVerbal.StatutLibelle),
            ("Étudiants", procesVerbal.NombreLignes.ToString(CultureInfo.InvariantCulture)),
            ("Décisions", $"Validés : {procesVerbal.NombreValides} | Rattrapage : {procesVerbal.NombreRattrapage} | Non délibérés : {procesVerbal.NombreNonDeliberes}")
        ]));

        body.Append(Paragraph(string.Empty, "Normal"));
        body.Append(ecHeaders.Count == 0
            ? SimpleLignesTable(procesVerbal)
            : DetailsTable(procesVerbal, ecHeaders));

        body.Append(Paragraph(string.Empty, "Normal"));
        body.Append(SignatureTable());

        return $"""
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:body>
    {body}
    <w:sectPr>
      <w:pgSz w:w="23840" w:h="16840" w:orient="landscape"/>
      <w:pgMar w:top="540" w:right="540" w:bottom="540" w:left="540" w:header="360" w:footer="360" w:gutter="0"/>
    </w:sectPr>
  </w:body>
</w:document>
""";
    }

    private static string DetailsTable(
        ProcesVerbalDto procesVerbal,
        IReadOnlyCollection<ProcesVerbalElementConstitutifLigneDto> ecHeaders)
    {
        var columnWidths = BuildDetailsColumnWidths(ecHeaders.Count);
        var table = new StringBuilder();

        table.Append(TableStart(columnWidths));

        var firstHeaderCells = new List<string>
        {
            Cell("Matricules", true, width: columnWidths[0]),
            Cell("Noms et prénoms", true, width: columnWidths[1])
        };

        foreach (var ec in ecHeaders)
        {
            firstHeaderCells.Add(Cell(ec.ElementConstitutifLibelle, true, gridSpan: 4));
        }

        firstHeaderCells.Add(Cell("Crédits", true));
        firstHeaderCells.Add(Cell("Moy Gle", true));
        firstHeaderCells.Add(Cell("Rang", true));
        firstHeaderCells.Add(Cell("Décision", true));
        table.Append(Row(firstHeaderCells, "HeaderRow"));

        var secondHeaderCells = new List<string>
        {
            Cell(string.Empty),
            Cell(string.Empty)
        };

        foreach (var _ in ecHeaders)
        {
            secondHeaderCells.Add(Cell("CCON", true));
            secondHeaderCells.Add(Cell("CC", true));
            secondHeaderCells.Add(Cell("SN", true));
            secondHeaderCells.Add(Cell("MOY", true));
        }

        secondHeaderCells.Add(Cell(string.Empty));
        secondHeaderCells.Add(Cell(string.Empty));
        secondHeaderCells.Add(Cell(string.Empty));
        secondHeaderCells.Add(Cell(string.Empty));
        table.Append(Row(secondHeaderCells, "HeaderRow"));

        foreach (var ligne in procesVerbal.Lignes
            .OrderBy(x => x.Rang ?? int.MaxValue)
            .ThenBy(x => x.EtudiantNomComplet))
        {
            var cells = new List<string>
            {
                Cell(ligne.Matricule),
                Cell(ligne.EtudiantNomComplet, align: "left")
            };

            foreach (var ec in ecHeaders)
            {
                var detail = ligne.ElementsConstitutifs.FirstOrDefault(x => x.Key == ec.Key);
                cells.Add(Cell(FormatNote(detail?.MoyenneCcon)));
                cells.Add(Cell(FormatNote(detail?.MoyenneCc)));
                cells.Add(Cell(FormatNote(detail?.MoyenneSn)));
                cells.Add(Cell(FormatNote(detail?.MoyenneFinale), bold: true, textColor: GetNoteColor(detail?.MoyenneFinale)));
            }

            cells.Add(Cell(FormatCredit(ligne.CreditsAcquis), true));
            cells.Add(Cell(FormatNote(ligne.MoyenneGenerale), true, textColor: GetNoteColor(ligne.MoyenneGenerale)));
            cells.Add(Cell(ligne.Rang?.ToString(CultureInfo.InvariantCulture) ?? string.Empty));
            cells.Add(Cell(ligne.DecisionJuryLibelle));
            table.Append(Row(cells));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string SimpleLignesTable(ProcesVerbalDto procesVerbal)
    {
        var table = new StringBuilder();
        table.Append(TableStart([1400, 5200, 1300, 1300, 1100, 1800], "12100"));
        table.Append(Row([
            Cell("Rang", true),
            Cell("Noms et prénoms", true),
            Cell("Matricule", true),
            Cell("Moy Gle", true),
            Cell("Crédits", true),
            Cell("Décision", true)
        ], "HeaderRow"));

        foreach (var ligne in procesVerbal.Lignes
            .OrderBy(x => x.Rang ?? int.MaxValue)
            .ThenBy(x => x.EtudiantNomComplet))
        {
            table.Append(Row([
                Cell(ligne.Rang?.ToString(CultureInfo.InvariantCulture) ?? string.Empty),
                Cell(ligne.EtudiantNomComplet, align: "left"),
                Cell(ligne.Matricule),
                Cell(FormatNote(ligne.MoyenneGenerale), true, textColor: GetNoteColor(ligne.MoyenneGenerale)),
                Cell(FormatCredit(ligne.CreditsAcquis)),
                Cell(ligne.DecisionJuryLibelle)
            ]));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string InfoTable(IReadOnlyCollection<(string Label, string Value)> items)
    {
        var table = new StringBuilder();
        table.Append(TableStart([2200, 7200, 2200, 7200], "18800"));

        foreach (var pair in items.Chunk(2))
        {
            var cells = new List<string>();

            foreach (var item in pair)
            {
                cells.Add(Cell(item.Label, true, shading: "F4F6F8", align: "left"));
                cells.Add(Cell(item.Value, align: "left"));
            }

            if (pair.Length == 1)
            {
                cells.Add(Cell(string.Empty, true, shading: "F4F6F8"));
                cells.Add(Cell(string.Empty));
            }

            table.Append(Row(cells));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string SignatureTable()
    {
        var table = new StringBuilder();
        table.Append(TableStart([7000, 7000], "14000", borderColor: "FFFFFF"));
        table.Append(Row([
            Cell("Le responsable académique", true),
            Cell("Le jury", true)
        ]));
        table.Append(Row([
            Cell("\n\n\n____________________________"),
            Cell("\n\n\n____________________________")
        ]));
        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static List<ProcesVerbalElementConstitutifLigneDto> BuildEcHeaders(ProcesVerbalDto procesVerbal)
        => procesVerbal.Lignes
            .SelectMany(x => x.ElementsConstitutifs)
            .GroupBy(x => x.Key)
            .Select(x => x.First())
            .ToList();

    private static int[] BuildDetailsColumnWidths(int ecCount)
    {
        var widths = new List<int> { 1100, 3300 };

        for (var i = 0; i < ecCount; i++)
        {
            widths.AddRange([640, 640, 640, 700]);
        }

        widths.AddRange([850, 850, 700, 1300]);

        return widths.ToArray();
    }

    private static string TableStart(int[] columnWidths, string? tableWidth = null, string borderColor = "BFBFBF")
    {
        var grid = string.Join(string.Empty, columnWidths.Select(width => $"<w:gridCol w:w=\"{width}\"/>"));
        var effectiveTableWidth = tableWidth ?? columnWidths.Sum().ToString(CultureInfo.InvariantCulture);

        return $"""
<w:tbl>
  <w:tblPr>
    <w:tblW w:w="{effectiveTableWidth}" w:type="dxa"/>
    <w:tblBorders>
      <w:top w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
      <w:left w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
      <w:bottom w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
      <w:right w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
      <w:insideH w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
      <w:insideV w:val="single" w:sz="4" w:space="0" w:color="{borderColor}"/>
    </w:tblBorders>
    <w:tblLayout w:type="fixed"/>
    <w:tblCellMar>
      <w:top w:w="60" w:type="dxa"/>
      <w:left w:w="70" w:type="dxa"/>
      <w:bottom w:w="60" w:type="dxa"/>
      <w:right w:w="70" w:type="dxa"/>
    </w:tblCellMar>
  </w:tblPr>
  <w:tblGrid>{grid}</w:tblGrid>
""";
    }

    private static string Row(IReadOnlyCollection<string> cells, string? style = null)
    {
        var shading = style switch
        {
            "HeaderRow" => "1F4E79",
            "TotalRow" => "F4F6F8",
            _ => null
        };

        var textColor = style == "HeaderRow" ? "FFFFFF" : null;
        var rowCells = string.Join(string.Empty, cells.Select(cell => ApplyCellStyle(cell, shading, textColor)));
        return $"<w:tr>{rowCells}</w:tr>";
    }

    private static string ApplyCellStyle(string cell, string? shading, string? textColor)
    {
        if (!string.IsNullOrWhiteSpace(shading))
        {
            cell = cell.Replace("<w:vAlign", $"<w:shd w:fill=\"{shading}\"/><w:vAlign", StringComparison.Ordinal);
        }

        if (!string.IsNullOrWhiteSpace(textColor))
        {
            cell = cell.Replace("<w:color w:val=\"000000\"/>", $"<w:color w:val=\"{textColor}\"/>", StringComparison.Ordinal);
        }

        return cell;
    }

    private static string Cell(
        string text,
        bool bold = false,
        string? textColor = null,
        string? shading = null,
        string align = "center",
        int? gridSpan = null,
        string? vMerge = null,
        int? width = null)
    {
        var properties = new StringBuilder("<w:tcPr>");

        if (width is not null)
        {
            properties.Append($"<w:tcW w:w=\"{width}\" w:type=\"dxa\"/>");
        }

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

        if (!string.IsNullOrWhiteSpace(shading))
        {
            properties.Append($"<w:shd w:fill=\"{shading}\"/>");
        }

        properties.Append("<w:vAlign w:val=\"center\"/></w:tcPr>");

        return $"""
<w:tc>
  {properties}
  {Paragraph(text, bold ? "TableBold" : "TableText", align, textColor)}
</w:tc>
""";
    }

    private static string Paragraph(string text, string style, string align = "left", string? textColor = null)
    {
        var paragraphProperties = style switch
        {
            "Title" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"160\"/></w:pPr>",
            "Subtitle" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"180\"/></w:pPr>",
            "CenterSmall" => "<w:pPr><w:jc w:val=\"center\"/><w:spacing w:after=\"20\"/></w:pPr>",
            "TableText" or "TableBold" => $"<w:pPr><w:jc w:val=\"{align}\"/><w:spacing w:after=\"0\"/></w:pPr>",
            _ => "<w:pPr><w:spacing w:after=\"100\"/></w:pPr>"
        };

        var color = textColor ?? (style == "Title" || style == "Subtitle" ? "1F4E79" : "000000");
        var fontSize = style switch
        {
            "Title" => "26",
            "Subtitle" => "20",
            "CenterSmall" => "16",
            "TableBold" or "TableText" => "13",
            _ => "18"
        };
        var bold = style is "Title" or "Subtitle" or "TableBold";
        var boldXml = bold ? "<w:b/>" : string.Empty;

        return $"<w:p>{paragraphProperties}<w:r><w:rPr>{boldXml}<w:sz w:val=\"{fontSize}\"/><w:color w:val=\"{color}\"/></w:rPr>{TextRuns(text)}</w:r></w:p>";
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

    private static string FormatCredit(decimal? value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        return value.Value == decimal.Truncate(value.Value)
            ? value.Value.ToString("0", CultureInfo.InvariantCulture)
            : value.Value.ToString("0.##", CultureInfo.InvariantCulture);
    }

    private static string GetNoteColor(decimal? value)
    {
        if (value is null)
        {
            return "000000";
        }

        return value.Value >= 10m ? "008000" : "C00000";
    }

    private static string BuildFileName(ProcesVerbalDto procesVerbal)
    {
        var name = string.Join(
            "-",
            new[] { "pv", procesVerbal.AnneeAcademiqueLibelle, procesVerbal.CodeSession, procesVerbal.ClassePedagogiqueLibelle }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
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
        <w:rFonts w:ascii="Arial Narrow" w:hAnsi="Arial Narrow"/>
        <w:sz w:val="16"/>
      </w:rPr>
    </w:rPrDefault>
  </w:docDefaults>
</w:styles>
""";
}
