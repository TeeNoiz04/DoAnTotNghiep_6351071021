using System;

namespace QuoteFlow.Materials.MaterialImport.MaterialFactory;

public class MaterialFactoryUpdateExcelDto
{
    public Guid? Id { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public int? ReferenceLeadTime { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int? Maxlot { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
