using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.DPOs.DPODetails;

public class GetDPODetailsInput : PagedAndSortedResultRequestDto
{
    public string? FilterText { get; set; }

    public Guid? DPOId { get; set; }
    public string? Status { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public int? QtyMin { get; set; }
    public int? QtyMax { get; set; }
    public decimal? UnitPriceMin { get; set; }
    public decimal? UnitPriceMax { get; set; }
    public decimal? AmountMin { get; set; }
    public decimal? AmountMax { get; set; }
    public DateTime? RequestedETAMin { get; set; }
    public DateTime? RequestedETAMax { get; set; }
    public Guid? SPOId { get; set; }
    public string? SPOCode { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public int? LockStockMin { get; set; }
    public int? LockStockMax { get; set; }
    public int? LockStockSOMin { get; set; }
    public int? LockStockSOMax { get; set; }
    public int? LockShipmentMin { get; set; }
    public int? LockShipmentMax { get; set; }
    public int? DeliveredMin { get; set; }
    public int? DeliveredMax { get; set; }
    public int? NeedDeliveryMin { get; set; }
    public int? NeedDeliveryMax { get; set; }
    public string? Note { get; set; }
    public string? OrderReason { get; set; }
    public GetDPODetailsInput()
    {

    }
}