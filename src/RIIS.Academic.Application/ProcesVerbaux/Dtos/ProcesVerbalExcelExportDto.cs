namespace RIIS.Academic.Application.ProcesVerbaux.Dtos;

public class ProcesVerbalExcelExportDto
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public byte[] Content { get; set; } = [];
}
