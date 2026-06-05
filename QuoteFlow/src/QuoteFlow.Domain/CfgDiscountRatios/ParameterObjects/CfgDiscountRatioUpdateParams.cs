using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.CfgDiscountRatios.ParameterObjects;
public class CfgDiscountRatioUpdateParams
{
    [StringLength(CfgDiscountRatioConsts.Approval_TypeMaxLength)]
    public string? Approval_Type { get; set; }
    [StringLength(CfgDiscountRatioConsts.Product_TypeMaxLength)]
    public string? Product_Type { get; set; }
    [StringLength(CfgDiscountRatioConsts.AccountClassifyMaxLength)]
    public string? AccountClassify { get; set; }
    public decimal? Value_Min { get; set; }
    public decimal? Value_Max { get; set; }
    public decimal? DiscountRatio { get; set; }
    public string? Note { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;
}
