using QuoteFlow.SaleOrderDetails;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteFlow.SaleOrders.SaleOrderDetails.ParameterObjects;
public class SaleOrderDetailCreateParams
{
    [Required]
    public Guid SaleOrderId { get; set; }
    public Guid? DPODetailId { get; set; }
    [StringLength(SaleOrderDetailConsts.StatusCodeMaxLength)]
    public string? StatusCode { get; set; }
    [StringLength(SaleOrderDetailConsts.GolfaCodeMaxLength)]
    public string? GolfaCode { get; set; }
    public int? Qty { get; set; }
    public decimal? Price { get; set; }
    public decimal? Amount { get; set; }
    public decimal? VAT { get; set; }
    public Guid? StockCategoryId { get; set; }
    [StringLength(SaleOrderDetailConsts.NoteMaxLength)]
    public string? Note { get; set; }
    public string? Extrafee_Note { get; set; }
    public Guid? LockStockId { get; set; }
    public virtual decimal? Extrafee { get; set; }
    public virtual decimal? AmountIncludeExtrafee { get; set; }
    public virtual decimal? SAPLandingCost { get; set; }
    public virtual decimal? SAPAmountLandingCost { get; set; }
    public virtual string? GICPorNo { get; set; }
    public virtual string? GICPrNo { get; set; }
    public virtual string? GICSalePIC { get; set; }
    public virtual string? GICLocation { get; set; }
    public virtual string? GICReservationNo { get; set; }

    public virtual string? GICGivNo { get; set; }
    public virtual DateTime? GICGivDate { get; set; }
    public virtual string? ChangeNote { get; set; }

    public virtual string? Disposed { get; set; }
}
