using System;

namespace QuoteFlow.DPOs.Models;

public class GICFOCModel
{
    public string? GICNo { get; set; }

    public DateTime? GICDate { get; set; }

    public string? CostCenter { get; set; }

    public string? SaleOrg { get; set; }
    public string? BuyerCode { get; set; }

    public string? SAPBuyerCode { get; set; }
    public string? OrderReason { get; set; }
    public string? RefDoc { get; set; }

    public string? MaterialType { get; set; }
    public string? No { get; set; }

    public string? SAPCode { get; set; }
    public string? MaterialCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? SONO { get; set; }
    public decimal SOQty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}
