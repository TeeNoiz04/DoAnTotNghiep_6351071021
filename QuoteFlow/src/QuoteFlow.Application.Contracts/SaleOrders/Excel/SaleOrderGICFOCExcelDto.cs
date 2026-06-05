using System;

namespace QuoteFlow.SaleOrders.Excel;

public class SaleOrderGICFOCExcelDto
{
    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? SOType { get; set; }
    public string? SONo { get; set; }
    public string? SAPSONo { get; set; }
    public decimal? GICSAPLandingCost { get; set; }
    public decimal? SOQty { get; set; }
    public decimal? GICAmountSAPLandingCost { get; set; }
    public string? GICPORNo { get; set; }
    public string? GICPRNo { get; set; }
    public string? GICGivNo { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string? Note { get; set; }
    public string? GICNo { get; set; }
    public string? BillingNo { get; set; }
    public string? DONo { get; set; }
    public string? ChangeNote { get; set; }
}
