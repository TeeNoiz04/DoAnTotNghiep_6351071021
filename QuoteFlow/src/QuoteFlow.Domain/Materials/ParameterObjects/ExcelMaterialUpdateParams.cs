using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialUpdateParams
{
    public Guid Id { get; set; }
    [Required]
    [StringLength(MaterialConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;

    public string? SAPCode { get; set; }
    public string? DescriptionVN { get; set; }
    public string? ProductHiearchy { get; set; }
    public decimal? VAT { get; set; }
    public string? ConcurrencyStamp { set; get; }
}
