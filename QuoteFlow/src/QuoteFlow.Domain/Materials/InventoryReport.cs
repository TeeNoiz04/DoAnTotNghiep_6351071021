namespace QuoteFlow.Materials;

public class InventoryReport
{
    public string? GolfaCode { get; set; }
    public string? Model { get; set; }
    public string? Spec1 { get; set; }
    public string? SAP_Code { get; set; }
    public string? InventoryCategory { get; set; }
    public string? Material_Group { get; set; }
    public decimal? Standard_Price { get; set; }
    public decimal? LandedCost { get; set; }

    public decimal AvailableStock_Qty { get; set; }
    public decimal AvailableStock_Amount { get; set; }

    public decimal GKR_Qty { get; set; }
    public decimal GKR_Amount { get; set; }

    public decimal LockedStock_Qty { get; set; }
    public decimal LockedStock_Amount { get; set; }

    public decimal Inventory_Qty { get; set; }
    public decimal Inventory_Amount { get; set; }

    public decimal MEVNBackOrder_OnOrder_Qty { get; set; }
    public decimal MEVNBackOrder_OnOrder_Amount { get; set; }

    public decimal MEVNBackOrder_Locked_Qty { get; set; }
    public decimal MEVNBackOrder_Locked_Amount { get; set; }

    public decimal StockWarning_Qty { get; set; }
    public decimal StockWarning_Amount { get; set; }

    public decimal MEVNBackOrder_Qty { get; set; }
    public decimal MEVNBackOrder_Amount { get; set; }
}
