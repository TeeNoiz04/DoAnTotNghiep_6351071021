namespace QuoteFlow.StockManagements;
public class DataMaterialOverallStockReportDto
{
    public string? MaterialType { get; set; }
    public string? Material_Group { get; set; }
    public decimal? Available_Stock { get; set; }
    public decimal? Keeping_Stock { get; set; }
    public decimal? On_Order_Stock { get; set; }
    public decimal? StockWarning { get; set; }
}
