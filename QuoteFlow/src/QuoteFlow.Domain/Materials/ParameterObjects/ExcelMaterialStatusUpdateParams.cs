using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.Materials.ParameterObjects;

public class ExcelMaterialStatusUpdateParams
{
    public Guid? Id { get; set; }
    [Required]
    [StringLength(MaterialConsts.GolfaCodeMaxLength)]
    public string GolfaCode { get; set; } = null!;
    [Required]
    [StringLength(MaterialConsts.ModelMaxLength)]
    public string Model { get; set; } = null!;
    public DateTime? AcceptanceDate { get; set; }
    public DateTime? ActiveDate { get; set; }
    [StringLength(MaterialConsts.SourceMaxLength)]
    public string? Action { get; set; }
    public string? Source { get; set; }
    [StringLength(MaterialConsts.ReasonMaxLength)]
    public string? Reason { get; set; }
    [StringLength(MaterialConsts.Factory_nvarcharMaxLength)]
    public string? FactoryRefDoc { get; set; }
    public string? ConcurrencyStamp { set; get; }
}
