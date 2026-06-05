using System;

namespace QuoteFlow.SaleOrders.Excel;
public class SaleOrderExcelDto
{
    public string? SONo { get; set; } // A
    public string? SAPSONo { get; set; } // B
    public string? SAPDONo { get; set; } // C
    public string? SAPBillingNo { get; set; } // D
    public string? SAPINV { get; set; } // E
    public DateTime? SAPINVDate { get; set; } // F
}
