using System;

namespace QuoteFlow.SaleOrders.SaleOrderDetails;
public class SaleOrderExportData
{
    // --- Sale Order Info ---
    public string? SONo { get; set; }
    public string? BuyerType { get; set; }
    public string? BuyerName { get; set; }
    public string? StockName { get; set; }
    public string? SONote { get; set; }

    // --- Material Info ---
    public string? GolfaCode { get; set; }
    public string? SAP_Code { get; set; }
    public string? Model { get; set; }
    public string? MaterialType { get; set; }
    public string? Material_Group { get; set; }
    public string? Spec1 { get; set; }
    public string? Spec2 { get; set; }
    public string? Spec3 { get; set; }
    public decimal? SO_VAT { get; set; }
    public int Qty { get; set; }
    public decimal? Price { get; set; }
    public decimal? ExtraFee { get; set; }
    public decimal? Amount { get; set; }

    // --- DPO Info ---
    public string? DPONo { get; set; }
    public DateTime? DPO_Date { get; set; }
    public DateTime? RequestedETA { get; set; }

    // --- Customer Info ---
    public string? CustomerTaxCode { get; set; }
    public string? CustomerName { get; set; }
    public string? KeyAccount { get; set; }
    public string? KeyAccountClassBuyer { get; set; }

    // --- SPO Info ---
    public string? SPOCode { get; set; }
    public DateTime? SPODate { get; set; }
    public string? SPOType { get; set; }
    public string? SPOName { get; set; }
    public string? PanelBuilder { get; set; }
    public string? MEContractor { get; set; }
    public string? MainContractor { get; set; }
    public string? SIOEM { get; set; }
    public string? InvestorEndUser { get; set; }
    public string? Trading { get; set; }
    public string? EUIndustry { get; set; }

    // --- SAP References ---
    public string? SAPSONo { get; set; }
    public string? SAPDONo { get; set; }
    public string? SAPBillingNo { get; set; }
    public string? SAPInvoice { get; set; }
    public DateTime? SAPInvoiceDate { get; set; }
}





