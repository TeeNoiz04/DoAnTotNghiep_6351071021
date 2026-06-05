using QuoteFlow.Shared;
using System;
using Volo.Abp.Domain.Entities;

namespace QuoteFlow.DPOs.DPODetails;

public class DPODetailDto : ExtendedFullAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public Guid DPOId { get; set; }
    public string DpoNo { get; set; } = null!;
    public string DpoRemark { get; set; } = null!;
    public string DpoOrderDate { get; set; } = null!;
    public string? Status { get; set; }
    public string? MaterialStatus { get; set; }
    public virtual int? RowNo { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public int? Qty { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Amount { get; set; }
    public decimal? AmountIncludeExtraFee { get; set; }

    public DateTime? RequestedETA { get; set; }
    public Guid? SPOId { get; set; }
    public string? SPOCode { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public int? LockStock { get; set; }
    public int? LockStockSO { get; set; }
    public int? LockShipment { get; set; }
    public int? Delivered { get; set; }
    public int? NeedDelivery { get; set; }
    public string? Note { get; set; }
    public string? ConfirmNoted { get; set; }
    public string? OrderReason { get; set; }
    public virtual string? AccountNo { get; set; }
    public virtual decimal? Extrafee { get; set; }
    public virtual decimal? ExtrafeeUsedInSO { get; set; }
    public virtual decimal? ExtrafeeAvailable { get; set; }
    public virtual string? ExtrafeeNote { get; set; }
    public virtual decimal AvailableStockQty { get; set; }
    public virtual decimal OnOrderStockAvailable { get; set; }

    public string ConcurrencyStamp { get; set; } = null!;

}