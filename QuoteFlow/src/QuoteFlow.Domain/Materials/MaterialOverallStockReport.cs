namespace QuoteFlow.Materials;
public class MaterialOverallStockReport
{
    public string? MaterialType { get; set; }
    public string? Material_Group { get; set; }
    public decimal? Available_Stock { get; set; }
    public decimal? Keeping_Stock { get; set; }
    public decimal? On_Order_Stock { get; set; }
    public decimal? StockWarning { get; set; }
}
