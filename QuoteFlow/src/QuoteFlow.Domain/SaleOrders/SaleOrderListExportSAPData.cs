namespace QuoteFlow.SaleOrders;
public class SaleOrderListExportSAPData
{
    // --- Sale Order Information ---
    public string? SONo { get; set; }
    public string? SaleOrderType { get; set; }
    public string? SalesOrg { get; set; }
    public string? DistributionChannel { get; set; }
    public string? SoldTo { get; set; }
    public string? ShipTo { get; set; }
    public string? SoldToName1 { get; set; }
    public string? PartnerFunction { get; set; }
    public string? SalesEmployeeCode1 { get; set; }
    public string? SalesEmployeeCode2 { get; set; }
    public string? SalesEmployeeCode3 { get; set; }
    public string? DPONo { get; set; }
    public string? CustomerRefDate { get; set; }
    public string? Incoterm1 { get; set; }
    public string? PricingDate { get; set; }
    public string? RequestedDeliveryDate { get; set; }
    public string? OrderReason { get; set; }
    public string? DocumentCurrency { get; set; }
    public string? ExchangeRate { get; set; }
    public string? HeaderText01 { get; set; }
    public string? HeaderText02 { get; set; }
    public string? HeaderText03 { get; set; }

    // --- Sale Order Items information ---
    public string? ItemNo { get; set; }
    public string? MaterialNo { get; set; }
    public int? Qty { get; set; }
    public string? MaterialTaxRate { get; set; }
    public string? SalesUnit { get; set; }
    public string? Description { get; set; }
    public string? Plant { get; set; }
    public string? StockName { get; set; }
    public string? ValuationType { get; set; }
    public string? CustomerReferenceitem { get; set; }
    public string? ItemText01 { get; set; }
    public string? Amount1 { get; set; }
    public string? Amount2 { get; set; }
}
