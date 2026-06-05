using System;
using Volo.Abp.Application.Dtos;

namespace QuoteFlow.SaleOrders;

public class GetSaleOrdersInput : PagedAndSortedResultRequestDto
{

    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? MaterialType { get; set; }

    public string? DPONo { get; set; }
    public string? MaterialCode { get; set; }
    public string? InvoiceNo { get; set; }
    public string? LstSO { get; set; }
    public string? BuyerType { get; set; }
    public Guid? BuyerId { get; set; }
    public string? BuyerCode { get; set; }
    public string? BuyerName { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public string? StatusCode { get; set; }
    public string? Model { get; set; }
    public string? TaxCode { get; set; } //pending
    public Guid? BuyerTypeId { get; set; } //BuyerType
    public DateTime? SODateFrom { get; set; }
    public DateTime? SODateTo { get; set; }
    public DateTime? VATDateFrom { get; set; }
    public DateTime? VATDateTo { get; set; }
    public string? MaterialGroup { get; set; }
    public string? SOType { get; set; }
    public virtual bool? CompletelyClosed { get; set; }
    public string? GicType { get; set; }
    public string? GicProcess { get; set; }

    public GetSaleOrdersInput()
    {

    }
}