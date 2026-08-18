using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Xml.Linq;
using RIIS.Academic.Application.ProcesVerbaux.Dtos;
using RIIS.Academic.Application.ProcesVerbaux.Services;

namespace RIIS.Academic.Infrastructure.Documents;

public class ProcesVerbalTemplateWordExportService(IProcesVerbauxService procesVerbauxService)
    : IProcesVerbalTemplateWordExportService
{
    private static readonly XNamespace W = "http://schemas.openxmlformats.org/wordprocessingml/2006/main";

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
            Content = BuildDocumentFromTemplate(procesVerbal)
        };
    }

    private static byte[] BuildDocumentFromTemplate(ProcesVerbalDto procesVerbal)
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
                    ReplaceTextPlaceholders(documentXml, procesVerbal);
                    ReplacePvTablePlaceholder(documentXml, BuildTable(procesVerbal));

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
            Path.Combine(AppContext.BaseDirectory, "Documents", "Templates", "ProcesVerbalTemplate.docx"),
            Path.Combine(AppContext.BaseDirectory, "RIIS.Academic.Infrastructure", "Documents", "Templates", "ProcesVerbalTemplate.docx"),
            Path.Combine(Directory.GetCurrentDirectory(), "Documents", "Templates", "ProcesVerbalTemplate.docx"),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "RIIS.Academic.Infrastructure", "Documents", "Templates", "ProcesVerbalTemplate.docx")
        };

        var templatePath = candidates.FirstOrDefault(File.Exists);

        return templatePath
            ?? throw new FileNotFoundException(
                "Le canevas Word 'ProcesVerbalTemplate.docx' est introuvable. Vérifiez qu'il est copié dans Documents/Templates.",
                "ProcesVerbalTemplate.docx");
    }

    private static void ReplaceTextPlaceholders(XDocument documentXml, ProcesVerbalDto procesVerbal)
    {
        var replacements = new Dictionary<string, string>
        {
            ["{{PV_TYPE}}"] = procesVerbal.TypeLibelle,
            ["{{SEMESTRE_LIBELLE}}"] = procesVerbal.SemestrePedagogiqueLibelle,
            ["{{ANNEE_ACADEMIQUE}}"] = procesVerbal.AnneeAcademiqueLibelle,
            ["{{CYCLE_FORMATION}}"] = procesVerbal.CycleFormationLibelle,
            ["{{PARCOURS}}"] = procesVerbal.ParcoursLibelle,
            ["{{NIVEAU}}"] = InferNiveau(procesVerbal),
            ["{{CLASSE_PEDAGOGIQUE}}"] = procesVerbal.ClassePedagogiqueLibelle,
            ["{{SEMESTRE_NUMERO}}"] = procesVerbal.SemestreNumero is null
                ? string.Empty
                : $"S{procesVerbal.SemestreNumero.Value.ToString(CultureInfo.InvariantCulture)}",
            ["{{SESSION}}"] = procesVerbal.CodeSession ?? string.Empty,
            ["{{STATUT}}"] = procesVerbal.StatutLibelle,
            ["{{NOMBRE_ETUDIANTS}}"] = procesVerbal.NombreLignes.ToString(CultureInfo.InvariantCulture),
            ["{{NOMBRE_VALIDES}}"] = procesVerbal.NombreValides.ToString(CultureInfo.InvariantCulture),
            ["{{NOMBRE_RATTRAPAGE}}"] = procesVerbal.NombreRattrapage.ToString(CultureInfo.InvariantCulture),
            ["{{OBSERVATION}}"] = procesVerbal.Observation ?? string.Empty
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

    private static void ReplacePvTablePlaceholder(XDocument documentXml, XElement pvTable)
    {
        var placeholderTable = documentXml
            .Descendants(W + "tbl")
            .FirstOrDefault(table => table
                .Descendants(W + "t")
                .Any(text => text.Value.Contains("{{PV_TABLE}}", StringComparison.Ordinal)));

        if (placeholderTable is null)
        {
            throw new InvalidOperationException("Le placeholder '{{PV_TABLE}}' est introuvable dans le canevas Word.");
        }

        placeholderTable.ReplaceWith(pvTable);
    }

    private static XElement BuildTable(ProcesVerbalDto procesVerbal)
    {
        var ecHeaders = BuildEcHeaders(procesVerbal);
        var tableXml = ecHeaders.Count == 0
            ? BuildSimpleTableXml(procesVerbal)
            : BuildDetailsTableXml(procesVerbal, ecHeaders);

        return XElement.Parse(tableXml, LoadOptions.PreserveWhitespace);
    }

    private static string BuildDetailsTableXml(
        ProcesVerbalDto procesVerbal,
        IReadOnlyCollection<ProcesVerbalElementConstitutifLigneDto> ecHeaders)
    {
        var columnWidths = BuildDetailsColumnWidths(ecHeaders.Count);
        var table = new StringBuilder();

        table.Append(TableStart(columnWidths));

        var firstHeaderCells = new List<string>
        {
            Cell("Matricules", columnWidths[0], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Noms et prénoms", columnWidths[1], bold: true, fill: "1F4E79", textColor: "FFFFFF")
        };

        var columnIndex = 2;
        foreach (var ec in ecHeaders)
        {
            var spanWidth = columnWidths.Skip(columnIndex).Take(4).Sum();
            firstHeaderCells.Add(Cell(ec.ElementConstitutifLibelle, spanWidth, bold: true, fill: "1F4E79", textColor: "FFFFFF", gridSpan: 4));
            columnIndex += 4;
        }

        firstHeaderCells.Add(Cell("Crédits", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
        firstHeaderCells.Add(Cell("Moy Gle", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
        firstHeaderCells.Add(Cell("Rang", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
        firstHeaderCells.Add(Cell("Décision", columnWidths[columnIndex], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
        table.Append(Row(firstHeaderCells, repeatHeader: true));

        var secondHeaderCells = new List<string>
        {
            Cell(string.Empty, columnWidths[0], fill: "1F4E79", textColor: "FFFFFF"),
            Cell(string.Empty, columnWidths[1], fill: "1F4E79", textColor: "FFFFFF")
        };

        columnIndex = 2;
        foreach (var _ in ecHeaders)
        {
            secondHeaderCells.Add(Cell("CCON", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
            secondHeaderCells.Add(Cell("CC", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
            secondHeaderCells.Add(Cell("SN", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
            secondHeaderCells.Add(Cell("MOY", columnWidths[columnIndex++], bold: true, fill: "1F4E79", textColor: "FFFFFF"));
        }

        secondHeaderCells.Add(Cell(string.Empty, columnWidths[columnIndex++], fill: "1F4E79", textColor: "FFFFFF"));
        secondHeaderCells.Add(Cell(string.Empty, columnWidths[columnIndex++], fill: "1F4E79", textColor: "FFFFFF"));
        secondHeaderCells.Add(Cell(string.Empty, columnWidths[columnIndex++], fill: "1F4E79", textColor: "FFFFFF"));
        secondHeaderCells.Add(Cell(string.Empty, columnWidths[columnIndex], fill: "1F4E79", textColor: "FFFFFF"));
        table.Append(Row(secondHeaderCells, repeatHeader: true));

        foreach (var ligne in procesVerbal.Lignes
            .OrderBy(x => x.Rang ?? int.MaxValue)
            .ThenBy(x => x.EtudiantNomComplet))
        {
            var cells = new List<string>
            {
                Cell(ligne.Matricule, columnWidths[0]),
                Cell(ligne.EtudiantNomComplet, columnWidths[1], align: "left")
            };

            columnIndex = 2;
            foreach (var ec in ecHeaders)
            {
                var detail = ligne.ElementsConstitutifs.FirstOrDefault(x => x.Key == ec.Key);
                cells.Add(Cell(FormatNote(detail?.MoyenneCcon), columnWidths[columnIndex++]));
                cells.Add(Cell(FormatNote(detail?.MoyenneCc), columnWidths[columnIndex++]));
                cells.Add(Cell(FormatNote(detail?.MoyenneSn), columnWidths[columnIndex++]));
                cells.Add(Cell(FormatNote(detail?.MoyenneFinale), columnWidths[columnIndex++], bold: true, textColor: GetNoteColor(detail?.MoyenneFinale)));
            }

            cells.Add(Cell(FormatCredit(ligne.CreditsAcquis), columnWidths[columnIndex++], bold: true));
            cells.Add(Cell(FormatNote(ligne.MoyenneGenerale), columnWidths[columnIndex++], bold: true, textColor: GetNoteColor(ligne.MoyenneGenerale)));
            cells.Add(Cell(ligne.Rang?.ToString(CultureInfo.InvariantCulture) ?? string.Empty, columnWidths[columnIndex++]));
            cells.Add(Cell(ligne.DecisionJuryLibelle, columnWidths[columnIndex]));
            table.Append(Row(cells));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static string BuildSimpleTableXml(ProcesVerbalDto procesVerbal)
    {
        var columnWidths = new[] { 900, 4600, 1700, 1400, 1400, 2200 };
        var table = new StringBuilder();
        table.Append(TableStart(columnWidths));

        table.Append(Row([
            Cell("Rang", columnWidths[0], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Noms et prénoms", columnWidths[1], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Matricule", columnWidths[2], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Moy Gle", columnWidths[3], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Crédits", columnWidths[4], bold: true, fill: "1F4E79", textColor: "FFFFFF"),
            Cell("Décision", columnWidths[5], bold: true, fill: "1F4E79", textColor: "FFFFFF")
        ], repeatHeader: true));

        foreach (var ligne in procesVerbal.Lignes
            .OrderBy(x => x.Rang ?? int.MaxValue)
            .ThenBy(x => x.EtudiantNomComplet))
        {
            table.Append(Row([
                Cell(ligne.Rang?.ToString(CultureInfo.InvariantCulture) ?? string.Empty, columnWidths[0]),
                Cell(ligne.EtudiantNomComplet, columnWidths[1], align: "left"),
                Cell(ligne.Matricule, columnWidths[2]),
                Cell(FormatNote(ligne.MoyenneGenerale), columnWidths[3], bold: true, textColor: GetNoteColor(ligne.MoyenneGenerale)),
                Cell(FormatCredit(ligne.CreditsAcquis), columnWidths[4]),
                Cell(ligne.DecisionJuryLibelle, columnWidths[5])
            ]));
        }

        table.Append("</w:tbl>");
        return table.ToString();
    }

    private static int[] BuildDetailsColumnWidths(int ecCount)
    {
        const int tableWidth = 15600;
        var fixedWidths = new[] { 1150, 3300, 850, 850, 700, 1300 };
        var fixedTotal = fixedWidths.Sum();
        var ecColumnCount = ecCount * 4;
        var ecTotal = Math.Max(0, tableWidth - fixedTotal);
        var ecWidth = ecColumnCount == 0 ? 0 : ecTotal / ecColumnCount;
        var remainder = ecColumnCount == 0 ? 0 : ecTotal % ecColumnCount;

        var widths = new List<int> { fixedWidths[0], fixedWidths[1] };

        for (var index = 0; index < ecColumnCount; index++)
        {
            widths.Add(ecWidth + (index < remainder ? 1 : 0));
        }

        widths.AddRange(fixedWidths.Skip(2));
        return widths.ToArray();
    }

    private static string TableStart(int[] columnWidths)
    {
        var tableWidth = columnWidths.Sum().ToString(CultureInfo.InvariantCulture);
        var grid = string.Join(string.Empty, columnWidths.Select(width => $"<w:gridCol w:w=\"{width}\"/>"));

        return $"""
<w:tbl xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
  <w:tblPr>
    <w:tblW w:w="{tableWidth}" w:type="dxa"/>
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
      <w:top w:w="80" w:type="dxa"/>
      <w:left w:w="80" w:type="dxa"/>
      <w:bottom w:w="80" w:type="dxa"/>
      <w:right w:w="80" w:type="dxa"/>
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
        int? gridSpan = null)
    {
        var properties = new StringBuilder("<w:tcPr>");
        properties.Append($"<w:tcW w:w=\"{width}\" w:type=\"dxa\"/>");

        if (gridSpan is not null)
        {
            properties.Append($"<w:gridSpan w:val=\"{gridSpan}\"/>");
        }

        if (!string.IsNullOrWhiteSpace(fill))
        {
            properties.Append($"<w:shd w:fill=\"{fill}\"/>");
        }

        properties.Append("<w:vAlign w:val=\"center\"/></w:tcPr>");

        return $"""
<w:tc>
  {properties}
  {Paragraph(text, bold, align, textColor)}
</w:tc>
""";
    }

    private static string Paragraph(string text, bool bold, string align, string? textColor)
    {
        var boldXml = bold ? "<w:b/>" : string.Empty;
        var color = textColor ?? "000000";

        return $"""
<w:p>
  <w:pPr>
    <w:jc w:val="{align}"/>
    <w:spacing w:after="0"/>
  </w:pPr>
  <w:r>
    <w:rPr>
      {boldXml}
      <w:sz w:val="13"/>
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

    private static List<ProcesVerbalElementConstitutifLigneDto> BuildEcHeaders(ProcesVerbalDto procesVerbal)
        => procesVerbal.Lignes
            .SelectMany(x => x.ElementsConstitutifs)
            .GroupBy(x => x.Key)
            .Select(x => x.First())
            .ToList();

    private static string InferNiveau(ProcesVerbalDto procesVerbal)
    {
        if (procesVerbal.SemestreNumero is null)
        {
            return string.Empty;
        }

        var niveau = ((procesVerbal.SemestreNumero.Value - 1) / 2) + 1;
        return $"Niveau {niveau.ToString(CultureInfo.InvariantCulture)}";
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
            new[] { "pv-modele", procesVerbal.AnneeAcademiqueLibelle, procesVerbal.CodeSession, procesVerbal.ClassePedagogiqueLibelle }
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
}
