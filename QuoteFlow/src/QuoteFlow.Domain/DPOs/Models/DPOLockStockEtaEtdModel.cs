using System;

namespace QuoteFlow.DPOs.Models;

public class DPOLockStockEtaEtdModel
{
    public string InvoiceNo { get; set; } = null!;
    public string PONo { get; set; } = null!;
    public string MaterialCode { get; set; } = null!;
    public decimal Qty_Allocation { get; set; }
    public string? ShipmentMethod { get; set; }
    public DateTime? ETA { get; set; }
    public DateTime? ETD { get; set; }
    public string? Status { get; set; }
}