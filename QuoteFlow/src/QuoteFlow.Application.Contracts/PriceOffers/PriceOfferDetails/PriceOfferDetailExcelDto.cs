using MiniExcelLibs.Attributes;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public class PriceOfferDetailExcelDto
{
    [ExcelColumnWidth(15)]
    [ExcelColumnName("No.")]
    public virtual int RowNo { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelColumnName("Golfa Code")]
    public virtual string? GolfaCode { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Model Name")]
    public virtual string? ModelName { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelColumnName("Special Spec 1")]
    public virtual string? SpecialSpec1 { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelColumnName("Special Spec 2")]
    public virtual string? SpecialSpec2 { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("DPO Used")]
    public virtual decimal? DpoUsed { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("DPO Used Amount")]
    public virtual decimal? DpoUsedAmount { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Quantity")]
    public virtual decimal Qty { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Standard Price")]
    public virtual decimal StandardPrice { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Standard Amount")]
    public virtual decimal StandardAmount { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Buyer Price")]
    public virtual decimal? BuyerPrice { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Requested Amount")]
    public virtual decimal? RequestedAmount { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelFormat("#,##0.00%")]
    [ExcelColumnName("Requested Discount Ratio")]
    public virtual decimal? RequestedDiscountRatio { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Price To Customer")]
    public virtual decimal? PriceToCustomer { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Price To Customer Amount")]
    public virtual decimal PriceToCustomerAmount { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("MEVN Offer Price")]
    public virtual decimal MEVNOfferPrice { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("MEVN Offer Amount")]
    public virtual decimal? MEVNOfferAmount { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelColumnName("Competitor Brand")]
    public virtual string? CompetitorBrand { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelColumnName("Competitor Model")]
    public virtual string? CompetitorModel { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Competitor Price")]
    public virtual decimal? CompetitorPrice { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelFormat("#,##0.00")]
    [ExcelColumnName("Landing Cost")]
    public virtual decimal? LandingCost { get; set; }

    //[ExcelColumnWidth(20)]
    //[ExcelFormat("#,##0.00")]
    //public virtual decimal? InputPrice { get; set; }

    //[ExcelColumnWidth(15)]
    //public virtual string? InputCurrency { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelFormat("#,##0.00%")]
    [ExcelColumnName("Price Offer Detail Margin")]
    public virtual decimal? PriceOfferDetailMargin { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelColumnName("Account Code")]
    public virtual string? AccountCode { get; set; }

    [ExcelColumnWidth(40)]
    [ExcelColumnName("Note")]
    public virtual string? Note { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelColumnName("Approval Status")]
    public virtual string? Status { get; set; }

    [ExcelColumnWidth(20)]
    [ExcelColumnName("Project Result")]
    public virtual string? ProjectResult { get; set; }

    [ExcelColumnWidth(25)]
    [ExcelFormat("#,##0.00%")]
    [ExcelColumnName("Actual Discount Ratio")]
    public virtual decimal? ActualDiscountRatio { get; set; }
}