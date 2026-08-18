using System.Globalization;
using System.IO.Compression;
using System.Security;
using System.Text;
using RIIS.Academic.Application.ProcesVerbaux.Dtos;
using RIIS.Academic.Application.ProcesVerbaux.Services;

namespace RIIS.Academic.Infrastructure.Documents;

public class ProcesVerbalExcelExportService(IProcesVerbauxService procesVerbauxService) : IProcesVerbalExcelExportService
{
    private const string SheetName = "PV";

    public async Task<ProcesVerbalExcelExportDto?> ExporterProcesVerbalAsync(
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

        return new ProcesVerbalExcelExportDto
        {
            FileName = BuildFileName(procesVerbal),
            Content = BuildWorkbook(procesVerbal)
        };
    }

    private static byte[] BuildWorkbook(ProcesVerbalDto procesVerbal)
    {
        using var stream = new MemoryStream();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
        {
            AddEntry(archive, "[Content_Types].xml", ContentTypesXml);
            AddEntry(archive, "_rels/.rels", RootRelationshipsXml);
            AddEntry(archive, "docProps/app.xml", AppPropertiesXml);
            AddEntry(archive, "docProps/core.xml", BuildCorePropertiesXml());
            AddEntry(archive, "xl/workbook.xml", WorkbookXml);
            AddEntry(archive, "xl/_rels/workbook.xml.rels", WorkbookRelationshipsXml);
            AddEntry(archive, "xl/styles.xml", StylesXml);
            AddEntry(archive, "xl/worksheets/sheet1.xml", BuildWorksheetXml(procesVerbal));
        }

        return stream.ToArray();
    }

    private static string BuildWorksheetXml(ProcesVerbalDto procesVerbal)
    {
        var ecHeaders = BuildEcHeaders(procesVerbal);
        var totalColumns = ecHeaders.Count == 0 ? 6 : 2 + (ecHeaders.Count * 4) + 4;
        var lastColumn = ColumnName(totalColumns);
        var rows = new StringBuilder();
        var merges = new List<string>();

        rows.Append(Row(1, [TextCell(1, 1, procesVerbal.Titre.ToUpperInvariant(), 1)]));
        merges.Add($"A1:{lastColumn}1");

        rows.Append(Row(2, [TextCell(2, 1, $"{procesVerbal.TypeLibelle} - {procesVerbal.SemestrePedagogiqueLibelle}", 2)]));
        merges.Add($"A2:{lastColumn}2");

        rows.Append(Row(4, [
            TextCell(4, 1, "Cycle", 3),
            TextCell(4, 2, procesVerbal.CycleFormationLibelle, 4),
            TextCell(4, 4, "Parcours", 3),
            TextCell(4, 5, procesVerbal.ParcoursLibelle, 4)
        ]));
        rows.Append(Row(5, [
            TextCell(5, 1, "Classe", 3),
            TextCell(5, 2, procesVerbal.ClassePedagogiqueLibelle, 4),
            TextCell(5, 4, "Année académique", 3),
            TextCell(5, 5, procesVerbal.AnneeAcademiqueLibelle, 4)
        ]));
        rows.Append(Row(6, [
            TextCell(6, 1, "Session", 3),
            TextCell(6, 2, procesVerbal.CodeSession ?? string.Empty, 4),
            TextCell(6, 4, "Statut", 3),
            TextCell(6, 5, procesVerbal.StatutLibelle, 4)
        ]));
        rows.Append(Row(7, [
            TextCell(7, 1, "Étudiants", 3),
            NumberCell(7, 2, procesVerbal.NombreLignes, 4),
            TextCell(7, 4, "Décisions", 3),
            TextCell(7, 5, $"Validés : {procesVerbal.NombreValides} | Rattrapage : {procesVerbal.NombreRattrapage} | Non délibérés : {procesVerbal.NombreNonDeliberes}", 4)
        ]));

        if (ecHeaders.Count == 0)
        {
            BuildSimpleTable(procesVerbal, rows);
        }
        else
        {
            BuildDetailsTable(procesVerbal, ecHeaders, rows, merges);
        }

        var lastRow = 11 + Math.Max(1, procesVerbal.Lignes.Count) + 5;
        rows.Append(Row(lastRow - 2, [
            TextCell(lastRow - 2, 1, "Le responsable académique", 10),
            TextCell(lastRow - 2, 4, "Le jury", 10)
        ]));
        rows.Append(Row(lastRow, [
            TextCell(lastRow, 1, "____________________________", 6),
            TextCell(lastRow, 4, "____________________________", 6)
        ]));

        var mergeCellsXml = merges.Count == 0
            ? string.Empty
            : $"""<mergeCells count="{merges.Count}">{string.Join(string.Empty, merges.Select(x => $"<mergeCell ref=\"{x}\"/>"))}</mergeCells>""";

        return $"""
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"
           xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <sheetPr>
    <pageSetUpPr fitToPage="1"/>
  </sheetPr>
  <dimension ref="A1:{lastColumn}{lastRow}"/>
  <sheetViews>
    <sheetView workbookViewId="0">
      <pane xSplit="2" ySplit="11" topLeftCell="C12" activePane="bottomRight" state="frozen"/>
      <selection pane="bottomRight" activeCell="C12" sqref="C12"/>
    </sheetView>
  </sheetViews>
  <sheetFormatPr defaultRowHeight="18"/>
  {BuildColumnsXml(totalColumns, ecHeaders.Count)}
  <sheetData>
    {rows}
  </sheetData>
  {mergeCellsXml}
  <printOptions horizontalCentered="1"/>
  <pageMargins left="0.25" right="0.25" top="0.5" bottom="0.5" header="0.3" footer="0.3"/>
  <pageSetup paperSize="8" orientation="landscape" fitToWidth="1" fitToHeight="0"/>
</worksheet>
""";
    }

    private static void BuildSimpleTable(ProcesVerbalDto procesVerbal, StringBuilder rows)
    {
        rows.Append(Row(10, [
            TextCell(10, 1, "Rang", 5),
            TextCell(10, 2, "Noms et prénoms", 5),
            TextCell(10, 3, "Matricule", 5),
            TextCell(10, 4, "Moy Gle", 5),
            TextCell(10, 5, "Crédits", 5),
            TextCell(10, 6, "Décision", 5)
        ]));

        var rowIndex = 11;
        foreach (var ligne in procesVerbal.Lignes.OrderBy(x => x.Rang ?? int.MaxValue).ThenBy(x => x.EtudiantNomComplet))
        {
            rows.Append(Row(rowIndex, [
                NumberCell(rowIndex, 1, ligne.Rang, 6),
                TextCell(rowIndex, 2, ligne.EtudiantNomComplet, 7),
                TextCell(rowIndex, 3, ligne.Matricule, 6),
                NoteCell(rowIndex, 4, ligne.MoyenneGenerale),
                NumberCell(rowIndex, 5, ligne.CreditsAcquis, 6),
                TextCell(rowIndex, 6, ligne.DecisionJuryLibelle, 6)
            ]));
            rowIndex++;
        }
    }

    private static void BuildDetailsTable(
        ProcesVerbalDto procesVerbal,
        IReadOnlyCollection<ProcesVerbalElementConstitutifLigneDto> ecHeaders,
        StringBuilder rows,
        List<string> merges)
    {
        var firstHeader = new List<string>
        {
            TextCell(10, 1, "Matricules", 5),
            TextCell(10, 2, "Noms et prénoms", 5)
        };

        merges.Add("A10:A11");
        merges.Add("B10:B11");

        var columnIndex = 3;
        foreach (var ec in ecHeaders)
        {
            firstHeader.Add(TextCell(10, columnIndex, ec.ElementConstitutifLibelle, 5));
            merges.Add($"{ColumnName(columnIndex)}10:{ColumnName(columnIndex + 3)}10");
            columnIndex += 4;
        }

        firstHeader.Add(TextCell(10, columnIndex, "Crédits", 5));
        merges.Add($"{ColumnName(columnIndex)}10:{ColumnName(columnIndex)}11");
        columnIndex++;

        firstHeader.Add(TextCell(10, columnIndex, "Moy Gle", 5));
        merges.Add($"{ColumnName(columnIndex)}10:{ColumnName(columnIndex)}11");
        columnIndex++;

        firstHeader.Add(TextCell(10, columnIndex, "Rang", 5));
        merges.Add($"{ColumnName(columnIndex)}10:{ColumnName(columnIndex)}11");
        columnIndex++;

        firstHeader.Add(TextCell(10, columnIndex, "Décision", 5));
        merges.Add($"{ColumnName(columnIndex)}10:{ColumnName(columnIndex)}11");

        rows.Append(Row(10, firstHeader));

        var secondHeader = new List<string>
        {
            TextCell(11, 1, string.Empty, 5),
            TextCell(11, 2, string.Empty, 5)
        };

        foreach (var _ in ecHeaders)
        {
            secondHeader.Add(TextCell(11, secondHeader.Count + 1, "CCON", 5));
            secondHeader.Add(TextCell(11, secondHeader.Count + 1, "CC", 5));
            secondHeader.Add(TextCell(11, secondHeader.Count + 1, "SN", 5));
            secondHeader.Add(TextCell(11, secondHeader.Count + 1, "MOY", 5));
        }

        secondHeader.Add(TextCell(11, secondHeader.Count + 1, string.Empty, 5));
        secondHeader.Add(TextCell(11, secondHeader.Count + 1, string.Empty, 5));
        secondHeader.Add(TextCell(11, secondHeader.Count + 1, string.Empty, 5));
        secondHeader.Add(TextCell(11, secondHeader.Count + 1, string.Empty, 5));
        rows.Append(Row(11, secondHeader));

        var rowIndex = 12;
        foreach (var ligne in procesVerbal.Lignes.OrderBy(x => x.Rang ?? int.MaxValue).ThenBy(x => x.EtudiantNomComplet))
        {
            var cells = new List<string>
            {
                TextCell(rowIndex, 1, ligne.Matricule, 6),
                TextCell(rowIndex, 2, ligne.EtudiantNomComplet, 7)
            };

            columnIndex = 3;
            foreach (var ec in ecHeaders)
            {
                var detail = ligne.ElementsConstitutifs.FirstOrDefault(x => x.Key == ec.Key);
                cells.Add(NumberCell(rowIndex, columnIndex++, detail?.MoyenneCcon, 6));
                cells.Add(NumberCell(rowIndex, columnIndex++, detail?.MoyenneCc, 6));
                cells.Add(NumberCell(rowIndex, columnIndex++, detail?.MoyenneSn, 6));
                cells.Add(NoteCell(rowIndex, columnIndex++, detail?.MoyenneFinale));
            }

            cells.Add(NumberCell(rowIndex, columnIndex++, ligne.CreditsAcquis, 10));
            cells.Add(NoteCell(rowIndex, columnIndex++, ligne.MoyenneGenerale));
            cells.Add(NumberCell(rowIndex, columnIndex++, ligne.Rang, 6));
            cells.Add(TextCell(rowIndex, columnIndex, ligne.DecisionJuryLibelle, 6));
            rows.Append(Row(rowIndex, cells));
            rowIndex++;
        }
    }

    private static List<ProcesVerbalElementConstitutifLigneDto> BuildEcHeaders(ProcesVerbalDto procesVerbal)
        => procesVerbal.Lignes
            .SelectMany(x => x.ElementsConstitutifs)
            .GroupBy(x => x.Key)
            .Select(x => x.First())
            .ToList();

    private static string BuildColumnsXml(int totalColumns, int ecCount)
    {
        var columns = new StringBuilder("<cols>");
        columns.Append("""<col min="1" max="1" width="16" customWidth="1"/>""");
        columns.Append("""<col min="2" max="2" width="32" customWidth="1"/>""");

        if (ecCount > 0)
        {
            var ecStart = 3;
            var ecEnd = 2 + (ecCount * 4);
            columns.Append($"""<col min="{ecStart}" max="{ecEnd}" width="9" customWidth="1"/>""");
            columns.Append($"""<col min="{ecEnd + 1}" max="{totalColumns - 1}" width="11" customWidth="1"/>""");
            columns.Append($"""<col min="{totalColumns}" max="{totalColumns}" width="18" customWidth="1"/>""");
        }
        else
        {
            columns.Append("""<col min="3" max="3" width="16" customWidth="1"/>""");
            columns.Append("""<col min="4" max="5" width="11" customWidth="1"/>""");
            columns.Append("""<col min="6" max="6" width="18" customWidth="1"/>""");
        }

        columns.Append("</cols>");
        return columns.ToString();
    }

    private static string Row(int rowIndex, IReadOnlyCollection<string> cells)
        => $"""<row r="{rowIndex}">{string.Join(string.Empty, cells)}</row>""";

    private static string TextCell(int rowIndex, int columnIndex, string value, int styleIndex)
        => $"""<c r="{ColumnName(columnIndex)}{rowIndex}" t="inlineStr" s="{styleIndex}"><is><t xml:space="preserve">{Escape(value)}</t></is></c>""";

    private static string NumberCell(int rowIndex, int columnIndex, int? value, int styleIndex)
        => value is null
            ? EmptyCell(rowIndex, columnIndex, styleIndex)
            : $"""<c r="{ColumnName(columnIndex)}{rowIndex}" s="{styleIndex}"><v>{value.Value.ToString(CultureInfo.InvariantCulture)}</v></c>""";

    private static string NumberCell(int rowIndex, int columnIndex, decimal? value, int styleIndex)
        => value is null
            ? EmptyCell(rowIndex, columnIndex, styleIndex)
            : $"""<c r="{ColumnName(columnIndex)}{rowIndex}" s="{styleIndex}"><v>{value.Value.ToString("0.00", CultureInfo.InvariantCulture)}</v></c>""";

    private static string NoteCell(int rowIndex, int columnIndex, decimal? value)
    {
        if (value is null)
        {
            return EmptyCell(rowIndex, columnIndex, 6);
        }

        var styleIndex = value.Value >= 10m ? 8 : 9;
        return NumberCell(rowIndex, columnIndex, value, styleIndex);
    }

    private static string EmptyCell(int rowIndex, int columnIndex, int styleIndex)
        => $"""<c r="{ColumnName(columnIndex)}{rowIndex}" s="{styleIndex}"/>""";

    private static string ColumnName(int index)
    {
        var name = string.Empty;
        while (index > 0)
        {
            index--;
            name = (char)('A' + (index % 26)) + name;
            index /= 26;
        }

        return name;
    }

    private static string BuildFileName(ProcesVerbalDto procesVerbal)
    {
        var name = string.Join(
            "-",
            new[] { "pv", procesVerbal.AnneeAcademiqueLibelle, procesVerbal.CodeSession, procesVerbal.ClassePedagogiqueLibelle }
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x!)
                .Select(Slug));

        return $"{name}.xlsx";
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

    private static string BuildCorePropertiesXml()
        => $"""
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties"
                   xmlns:dc="http://purl.org/dc/elements/1.1/"
                   xmlns:dcterms="http://purl.org/dc/terms/"
                   xmlns:dcmitype="http://purl.org/dc/dcmitype/"
                   xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <dc:creator>RIIS Academic</dc:creator>
  <cp:lastModifiedBy>RIIS Academic</cp:lastModifiedBy>
  <dcterms:created xsi:type="dcterms:W3CDTF">{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</dcterms:created>
  <dcterms:modified xsi:type="dcterms:W3CDTF">{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}</dcterms:modified>
</cp:coreProperties>
""";

    private const string ContentTypesXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
  <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
  <Default Extension="xml" ContentType="application/xml"/>
  <Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
  <Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
  <Override PartName="/xl/workbook.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml"/>
  <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml"/>
  <Override PartName="/xl/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml"/>
</Types>
""";

    private const string RootRelationshipsXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="xl/workbook.xml"/>
  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>
  <Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>
</Relationships>
""";

    private const string WorkbookRelationshipsXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="worksheets/sheet1.xml"/>
  <Relationship Id="rId2" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
</Relationships>
""";

    private const string WorkbookXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"
          xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships">
  <workbookPr/>
  <sheets>
    <sheet name="PV" sheetId="1" r:id="rId1"/>
  </sheets>
  <definedNames>
    <definedName name="_xlnm.Print_Titles" localSheetId="0">PV!$10:$11</definedName>
  </definedNames>
</workbook>
""";

    private const string AppPropertiesXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties"
            xmlns:vt="http://schemas.openxmlformats.org/officeDocument/2006/docPropsVTypes">
  <Application>RIIS Academic</Application>
  <DocSecurity>0</DocSecurity>
  <ScaleCrop>false</ScaleCrop>
  <HeadingPairs>
    <vt:vector size="2" baseType="variant">
      <vt:variant><vt:lpstr>Worksheets</vt:lpstr></vt:variant>
      <vt:variant><vt:i4>1</vt:i4></vt:variant>
    </vt:vector>
  </HeadingPairs>
  <TitlesOfParts>
    <vt:vector size="1" baseType="lpstr">
      <vt:lpstr>PV</vt:lpstr>
    </vt:vector>
  </TitlesOfParts>
</Properties>
""";

    private const string StylesXml = """
<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<styleSheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
  <numFmts count="1">
    <numFmt numFmtId="164" formatCode="0.00"/>
  </numFmts>
  <fonts count="7">
    <font><sz val="10"/><name val="Calibri"/></font>
    <font><b/><sz val="16"/><color rgb="FF1F4E79"/><name val="Calibri"/></font>
    <font><b/><sz val="12"/><color rgb="FF1F4E79"/><name val="Calibri"/></font>
    <font><b/><sz val="10"/><name val="Calibri"/></font>
    <font><b/><sz val="10"/><color rgb="FFFFFFFF"/><name val="Calibri"/></font>
    <font><b/><sz val="10"/><color rgb="FF008000"/><name val="Calibri"/></font>
    <font><b/><sz val="10"/><color rgb="FFC00000"/><name val="Calibri"/></font>
  </fonts>
  <fills count="4">
    <fill><patternFill patternType="none"/></fill>
    <fill><patternFill patternType="gray125"/></fill>
    <fill><patternFill patternType="solid"><fgColor rgb="FFF4F6F8"/><bgColor indexed="64"/></patternFill></fill>
    <fill><patternFill patternType="solid"><fgColor rgb="FF1F4E79"/><bgColor indexed="64"/></patternFill></fill>
  </fills>
  <borders count="2">
    <border><left/><right/><top/><bottom/><diagonal/></border>
    <border>
      <left style="thin"><color rgb="FFBFBFBF"/></left>
      <right style="thin"><color rgb="FFBFBFBF"/></right>
      <top style="thin"><color rgb="FFBFBFBF"/></top>
      <bottom style="thin"><color rgb="FFBFBFBF"/></bottom>
      <diagonal/>
    </border>
  </borders>
  <cellStyleXfs count="1">
    <xf numFmtId="0" fontId="0" fillId="0" borderId="0"/>
  </cellStyleXfs>
  <cellXfs count="11">
    <xf numFmtId="0" fontId="0" fillId="0" borderId="0" xfId="0"/>
    <xf numFmtId="0" fontId="1" fillId="0" borderId="0" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
    <xf numFmtId="0" fontId="2" fillId="0" borderId="0" xfId="0" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
    <xf numFmtId="0" fontId="3" fillId="2" borderId="1" xfId="0" applyFill="1" applyBorder="1" applyAlignment="1"><alignment horizontal="left" vertical="center"/></xf>
    <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1" applyAlignment="1"><alignment horizontal="left" vertical="center"/></xf>
    <xf numFmtId="0" fontId="4" fillId="3" borderId="1" xfId="0" applyFill="1" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center" wrapText="1"/></xf>
    <xf numFmtId="164" fontId="0" fillId="0" borderId="1" xfId="0" applyNumberFormat="1" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
    <xf numFmtId="0" fontId="0" fillId="0" borderId="1" xfId="0" applyBorder="1" applyAlignment="1"><alignment horizontal="left" vertical="center"/></xf>
    <xf numFmtId="164" fontId="5" fillId="0" borderId="1" xfId="0" applyNumberFormat="1" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
    <xf numFmtId="164" fontId="6" fillId="0" borderId="1" xfId="0" applyNumberFormat="1" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
    <xf numFmtId="0" fontId="3" fillId="0" borderId="1" xfId="0" applyBorder="1" applyAlignment="1"><alignment horizontal="center" vertical="center"/></xf>
  </cellXfs>
  <cellStyles count="1">
    <cellStyle name="Normal" xfId="0" builtinId="0"/>
  </cellStyles>
</styleSheet>
""";
}
