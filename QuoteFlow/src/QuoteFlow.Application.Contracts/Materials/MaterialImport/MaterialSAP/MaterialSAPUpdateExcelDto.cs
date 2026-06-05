using System;

namespace QuoteFlow.Materials.MaterialImport.MaterialSAP;

public class MaterialSAPUpdateExcelDto
{
    public Guid? Id { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? SAPCode { get; set; }
    public string? DescriptionVN { get; set; }
    public string? ProductHiearchy { get; set; }
    public decimal? VAT { get; set; }
    public string? ConcurrencyStamp { get; set; }
}
