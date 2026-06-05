using System;

namespace QuoteFlow.SaleOrders.ParameterObjects;
public class SaleOrderListExportSAPDataParams
{
    public string? Username { get; set; }
    public DateTime? OrderDateMin { get; set; }
    public DateTime? OrderDateMax { get; set; }
    public string? StatusCode { get; set; }
    public string? SONo { get; set; }
    public string? SOSAPNo { get; set; }
    public string? DPONo { get; set; }
    public string? DONo { get; set; }
    public string? MaterialCode { get; set; }

    public string? InvoiceNo { get; set; }
    public string? Model { get; set; }
    public string? BuyerType { get; set; }
    public string? BuyerCode { get; set; }
    public string? MaterialGroup { get; set; }
    public string? CustomerTaxCode { get; set; }
    public string? MaterialType { get; set; }
    public DateTime? SODateFrom { get; set; }
    public DateTime? SODateTo { get; set; }
    public DateTime? VATDateFrom { get; set; }
    public DateTime? VATDateTo { get; set; }
    public string? GicType { get; set; }
    public string? GicProcess { get; set; }
    public string? LstSO { get; set; }
    public Guid? BuyerId { get; set; }

    public bool HasFullBuyerAccess { get; set; }
}