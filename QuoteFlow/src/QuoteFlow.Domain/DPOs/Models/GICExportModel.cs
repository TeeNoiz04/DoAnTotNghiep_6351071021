using System;

namespace QuoteFlow.DPOs.Models;

public class GICExportModel
{
    public int RowNo { get; set; }

    public string? GICNo { get; set; }
    public string? GICType { get; set; }
    public string? GICProcess { get; set; }
    public string? CostCenter { get; set; }

    public string? GolfaCode { get; set; }
    public string? Model { get; set; }

    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? MaterialType { get; set; }
    public string? Material_Group { get; set; }

    public decimal Qty { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Amount { get; set; }

    public string? BuyerShortName { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? RequestedETA { get; set; }

    // GIC Warranty
    public string? CustomerName { get; set; }
    public string? DamagedProduct { get; set; }
    public string? ProductSerialNo { get; set; }
    public string? MEVNSellingInvoiceNo { get; set; }

    // GIC Sponsor
    public string? OrderReason { get; set; }

    public string? Status { get; set; }
    public DateTime CreationTime { get; set; }

    // SO
    public string? SONo { get; set; }
    public DateTime? SODate { get; set; }
    public decimal? SOQty { get; set; }

    // PO
    public string? PONo { get; set; }
    public DateTime? PODate { get; set; }
    public decimal? POQty { get; set; }
    public decimal? LockShipmentQty { get; set; }
    public decimal? LockShipmentImportedQty { get; set; }

    public string? MachineNumber { get; set; }
    public string? STCReply { get; set; }

    // Invoice
    public string? InvoiceNo { get; set; }
    public decimal? QtyAllocation { get; set; }
    public string? CDNo { get; set; }
    public string? BillNo { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public DateTime? StockDate { get; set; }

    // SAP
    public string? SAPCode { get; set; }
    public string? POSAPNo { get; set; }
    public string? SAPSONo { get; set; }
    public string? DOSAPNo { get; set; }
    public string? SAPBillingNo { get; set; }
    public string? SAPInvoiceNo { get; set; }
    public DateTime? SAPInvoiceDate { get; set; }

    public string? Remark { get; set; }
}
