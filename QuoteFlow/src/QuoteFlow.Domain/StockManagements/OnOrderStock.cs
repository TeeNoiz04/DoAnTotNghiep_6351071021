using System;

namespace QuoteFlow.StockManagements;
public class OnOrderStock
{
    public string? PONo { get; set; }
    public string? GolfaCode { get; set; }
    public int? QtyAvailable { get; set; }
    public DateTime? PODate { get; set; }
    public string? MachineNo { get; set; }
    public string? SupplierReply { get; set; }
    public string? MEVNAddRequest { get; set; }
    public string? MEVNRequest { get; set; }

}
