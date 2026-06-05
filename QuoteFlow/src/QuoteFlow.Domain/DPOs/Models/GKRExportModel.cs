using System;

namespace QuoteFlow.DPOs.Models;
public class GKRExportModel
{
    // ---GKR Information---
    //public string? GKRDetailId { get; set; }
    public string? GKRNo { get; set; }
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? MaterialType { get; set; }
    public string? Material_Group { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal AmountIncludeExtrafee { get; set; }
    public string? BuyerShortName { get; set; }
    public string? Gkr_SalePicFullName { get; set; }
    public string? CustomerName { get; set; }
    public DateTime OrderDate { get; set; }
    public string? GKRStatus { get; set; }
    public string? Gkr_LinkedDpoNo { get; set; }
    public int Qty { get; set; }

    // ---LockStock---
    public int? GKRLockStock { get; set; }
    public int? DPOLockStock { get; set; }

    // ---LockShipment---
    public int? GKRLockShipment { get; set; }
    public int? DPOLockShipment { get; set; }
    public int? ReleasedQty { get; set; }

    // ---PO Information---
    public string? PONo { get; set; }
    public string? Cargo_MachineNumber { get; set; }
    public string? Cargo_STCReply { get; set; }

    // ---Invoice Information---
    public string? InvoiceNo { get; set; }
    public int? QtyAllocation { get; set; }
    public string? CDNo { get; set; }
    public string? BillNo { get; set; }
    public DateTime? ETD { get; set; }
    public DateTime? ETA { get; set; }
    public DateTime? StockDate { get; set; }
    public string? ShipmentMethod { get; set; }

    // ---Reason---
    public string? Gkr_Reason { get; set; }
    public DateTime CreationTime { get; set; }
    public string? SAPCode { get; set; }
    public string? POSAPNo { get; set; }
    public int? POQty { get; set; }
    public int? LockshipmentQty { get; set; }
    public int? LockShipmentImportedQty { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public DateTime? PODate { get; set; }

}
