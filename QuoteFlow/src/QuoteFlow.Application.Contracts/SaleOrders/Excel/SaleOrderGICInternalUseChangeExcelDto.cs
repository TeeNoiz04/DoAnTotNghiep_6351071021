using System;

namespace QuoteFlow.SaleOrders.Excel;

public class SaleOrderGICInternalUseChangeExcelDto
{
    public string? MaterialCode { get; set; }
    public string? ModelName { get; set; }
    public string? SOType { get; set; }
    public string? SONo { get; set; }
    public decimal? GICSAPLandingCost { get; set; }
    public decimal? GICAmountSAPLandingCost { get; set; }
    public string? GICPORNo { get; set; }
    public string? GICPRNo { get; set; }
    public string? GICGivNo { get; set; }
    public DateTime? GICGivDate { get; set; }
    public string? GICSalesPIC { get; set; }
    public string? GICLocation { get; set; }
    public string? GICReservationNo { get; set; }

    public string? Note { get; set; }
    public string? GICNo { get; set; }
}
