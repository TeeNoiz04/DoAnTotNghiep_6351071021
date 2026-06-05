using System;

namespace QuoteFlow.StockManagements;
public class LockShipmentDto
{
    public string? GolfaCode { get; set; }
    public int? Qty { get; set; } = 0;
    public int? QtyNeed { get; set; } = 0;
    public int? QtyDisposed { get; set; } = 0;
    public string? PONo { get; set; }
    public string? DPONo { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Created { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? Modified { get; set; }
    public string? MachineNo { get; set; }
    public string? SupplierReply { get; set; }
    public string? MEVNAddRequest { get; set; }
    public string? MEVNRequest { get; set; }
}
