using System;

namespace QuoteFlow.DPOs;

public class DPOExportDataDto
{
    // Green Section (#32cd32)
    public string DPONo { get; set; } = null!;
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? MaterialType { get; set; }
    public string? MaterialGroup { get; set; }
    public decimal? Qty { get; set; }
    public decimal? Price { get; set; }
    public decimal? Amount { get; set; }
    public string? Distributor { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestedETA { get; set; }
    public string? ProjectCode { get; set; }
    public string? ProjectName { get; set; }
    public string? Customer { get; set; }
    public string? Status { get; set; }

    // Blue Section (#6495ed)
    public string? SONo { get; set; }
    public DateTime? SODate { get; set; }
    public decimal? SOQty { get; set; }

    // Turquoise Section (#40e0d0)
    public string? PONo { get; set; }
    public DateTime? PODate { get; set; }
    public decimal? POQty { get; set; }
    public decimal? LockshipmentQty { get; set; }
    public decimal? LockshipmentQtyImported { get; set; }
    public string? MachineNumber { get; set; }
    public string? STCReply { get; set; }

    // Pink Section (#fcc1cb)
    public string? InvoiceNo { get; set; }
    public decimal? QtyAllocation { get; set; }
    public string? CDNo { get; set; }
    public string? BillNo { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public DateTime? StockDate { get; set; }

    // Orange Section (#ffa500)
    public string? SAPCode { get; set; }
    public string? POSAPNo { get; set; }
    public string? SAPSONo { get; set; }
    public string? DOSAPNo { get; set; }
    public string? SAPBillingNo { get; set; }
    public string? SAPInvoiceNo { get; set; }
    public DateTime? SAPInvoiceDate { get; set; }
    public string? Remark { get; set; }
}