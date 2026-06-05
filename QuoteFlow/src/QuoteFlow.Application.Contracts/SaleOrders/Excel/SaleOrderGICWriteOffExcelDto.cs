using System;

namespace QuoteFlow.SaleOrders.Excel;

public class SaleOrderGICWriteOffExcelDto
{
    public string? SOType { get; set; }

    public string? GICWONo { get; set; }
    public DateTime? GICWODate { get; set; }

    public string? CostCenter { get; set; }

    public string? MaterialType { get; set; }

    public string? No { get; set; }

    public string? SAPCode { get; set; }

    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }

    public string? Spec1 { get; set; }

    public string? SONo { get; set; }

    public decimal? SOQty { get; set; }

    public string? Note { get; set; }

    public decimal? SAPLandingCost { get; set; }

    public decimal? AmountInSAPLandingCost { get; set; }

    public string? PORNo { get; set; }

    public string? PRNo { get; set; }

    public string? GIVNo { get; set; }

    public DateTime? GIVDate { get; set; }
    public string? Disposed { get; set; }
}
