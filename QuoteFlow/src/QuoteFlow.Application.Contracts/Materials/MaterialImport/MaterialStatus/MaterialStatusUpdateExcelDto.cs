using System;

namespace QuoteFlow.Materials.MaterialImport.MaterialStatus;

public class MaterialStatusUpdateExcelDto
{
    public Guid? Id { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public DateTime? AcceptanceDate { get; set; }
    public DateTime? ActiveDate { get; set; }
    public string? Action { get; set; }
    public string? Source { get; set; }
    public string? Reason { get; set; }
    public string? FactoryRefDoc { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
