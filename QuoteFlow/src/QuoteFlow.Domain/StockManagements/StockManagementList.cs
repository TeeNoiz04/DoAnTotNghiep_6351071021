using System;

namespace QuoteFlow.StockManagements;
public class StockManagementList
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? MaterialStatus { get; set; }
    public decimal Standard_Price { get; set; }
    public int? ReferenceLeadTime { get; set; }
    public string? CountryOfOrigin { get; set; }
    public int? WarrantyTime { get; set; }
    public int? Maxlot { get; set; }
    public string? MaterialType { get; set; }

    public decimal Stock_Qty { get; set; }
    public decimal Locked_Qty { get; set; }
    public decimal LockStockSO_Qty { get; set; }
    public decimal Available_Qty { get; set; }
    public decimal Lockshipment_Qty { get; set; }
    public decimal OnOderStock { get; set; }
    public string? StockCode { get; set; }
    public string? StockName { get; set; }
    public Guid? StockCategoryId { get; set; }
    public string? SAP_Code { get; set; }
    public string? Material_Group { get; set; }
}
