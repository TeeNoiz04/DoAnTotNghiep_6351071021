using System;

namespace QuoteFlow.SaleOrders;
public class SaleOrderExportSAPGICData
{
    // --- General Information ---
    public string? GICNo { get; set; }                       // dpo.DPONo as GICNo
    public string? BuyerType { get; set; }                   // cte.BuyerType
    public string? BuyerShortName { get; set; }              // cte.BuyerCode as BuyerShortName
    public string? BuyerSAPCode { get; set; }                // b.BuyerSAPCode
    public DateTime? GICDate { get; set; }                   // dpo.OrderDate as GICDate
    public string? Process { get; set; }                     // dpo.GICProcess as Process
    public string? CostCenter { get; set; }                  // dpo.CostCenter
    public string? RefDoc { get; set; }                      // dpo.ReferenceDoc as RefDoc
    public string? MaterialType { get; set; }                // cte.MaterialType

    // --- Detail ---
    public int? ItemNo { get; set; }                         // ROW_NUMBER() OVER (...)
    public string? MaterialCode { get; set; }                // sod.GolfaCode as MaterialCode
    public string? MaterialGroup { get; set; }               // m.Material_Group as MaterialGroup
    public string? Model { get; set; }                       // m.Model
    public string? SAP_Code { get; set; }                    // m.SAP_Code
    public decimal? VAT { get; set; }                        // cte.SO_VAT as VAT
    public string? Spec1 { get; set; }                       // m.Spec1
    public decimal? DPOQty { get; set; }                     // dpo.Qty
    public string? SONo { get; set; }                        // cte.SONo
    public decimal? Qty { get; set; }                        // sod.Qty
    public decimal? UnitPrice { get; set; }                  // sod.Price as UnitPrice
    public decimal? Amount { get; set; }                     // sod.Amount
    public string? Note { get; set; }                        // sod.Note
    public string? ChangeNote { get; set; }                  // sod.ChangeNote

    // --- SAP/Intranet Information ---
    public string? PORNo { get; set; }                       // sod.GICPorNo as PORNo
    public string? PRNo { get; set; }                        // sod.GICPrNo as PRNo
    public decimal? SAPLandingCost { get; set; }             // sod.SAPLandingCost
    public decimal? AmountInLandingCost { get; set; }        // sod.SAPAmountLandingCost as AmountInLandingCost
    public string? GIVNo { get; set; }                       // case when cte.GICType = 'GIC-InternalUse' then sod.GICGivNo else cte.GICGivNo
    public DateTime? GIVDate { get; set; }                   // case when cte.GICType = 'GIC-InternalUse' then sod.GICGivDate else cte.GICGivDate
    public string? SAPSONo { get; set; }                     // cte.SAPSONo
    public string? SAPDONo { get; set; }                     // cte.SAPDONo
    public string? SAPBillingNo { get; set; }                // cte.SAPBillingNo
    public string? InvoiceNo { get; set; }                   // cte.SAPInvoice as InvoiceNo
    public DateTime? InvoiceDate { get; set; }               // cte.SAPInvoiceDate as InvoiceDate

    // --- Internal Use Information ---
    public string? ReservationNo { get; set; }               // sod.GICReservationNo as ReservationNo
    public string? Location { get; set; }                    // sod.GICLocation as Loctaion (typo in SP)
    public string? SalePIC { get; set; }                     // sod.GICSalePIC as SalePIC
    public string? AssetClass { get; set; }                  // CASE WHEN cte.StatusCode = 'CLOSED' THEN NULL ELSE sod.GICAssetClass
    public string? MainAssetCode { get; set; }               // CASE WHEN cte.StatusCode = 'CLOSED' THEN NULL ELSE sod.GICMainAssetCode
    public string? SubAssetCode { get; set; }                // CASE WHEN cte.StatusCode = 'CLOSED' THEN NULL ELSE sod.GICSubAssetCode
    public string? AssetName { get; set; }                   // CASE WHEN cte.StatusCode = 'CLOSED' THEN NULL ELSE sod.GICAssetName

    // --- Warranty Information ---
    public string? CustomerName { get; set; }                // dpo.CustomerName
    public string? DamagedProduct { get; set; }              // dpo.DamagedProduct
    public string? ProductSerialNo { get; set; }             // dpo.ProductSerialNo
    public string? MEVNSellingInvoiceNo { get; set; }        // dpo.MEVNSellingInvoiceNo

    // --- FOC Information ---
    public string? SalesOrg { get; set; }                    // '' as SalesOrg
    public string? OrderReason { get; set; }                 // dpo.OrderReason

    // --- Write Off Information ---
    public string? SOType { get; set; }                      // 'Z2WO' as SOType
    public string? Remark { get; set; }                      // dpo.Remark
    public string? Disposed { get; set; }                    // '' as Disposed
    public string? DeliveryRemarks { get; set; }             // '' as DeliveryRemarks
}