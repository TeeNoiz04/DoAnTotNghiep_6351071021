using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialFactoryUpdateParams
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(MaterialConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;
    public int? ReferenceLeadTime { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int? Maxlot { get; set; }
    public string? ConcurrencyStamp { set; get; }
}
