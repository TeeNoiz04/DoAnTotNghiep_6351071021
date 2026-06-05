using System;

namespace QuoteFlow.SalesAssignments;
public class SaleReportByCustomer
{
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }

    public string? Nationality { get; set; }

    public string? CustomerType { get; set; }

    public string? Industry { get; set; }

    public string? TypeOfBusiness { get; set; }

    public string? KAType { get; set; }

    public string? KAClass { get; set; }

    public string? DPONo { get; set; }

    public string? Distributor { get; set; }

    public DateTime? DPODate { get; set; }

    public string? SPOCode { get; set; }

    public string? GolfaCode { get; set; }

    public string? Model { get; set; }

    public string? MaterialType { get; set; }

    public string? MaterialGroup { get; set; }


    public decimal? DPOQty { get; set; }

    public decimal? UnitStandardPrice { get; set; }

    public decimal? DPOUnitPrice { get; set; }

    public decimal? DPO_Amount { get; set; }

    public decimal? DiscountPercent { get; set; }

    public string? InvoiceNo { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal? InvoiceQty { get; set; }

    public decimal? AmountVATInvoice { get; set; }

    public decimal? UnitLandedCost { get; set; }

    public decimal? GPPercent { get; set; }
}
