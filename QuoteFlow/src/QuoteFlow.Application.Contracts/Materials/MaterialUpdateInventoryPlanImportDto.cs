using System;

namespace QuoteFlow.Materials;
public class MaterialUpdateInventoryPlanImportDto
{
    public Guid? Id { get; set; }
    public string GolfaCode { get; set; } = null!;
    public string Model { get; set; } = null!;
    public string? InventoryCategory { get; set; }
    public int? StockWarning { get; set; }
    public string? ConcurrencyStamp { get; set; }
    public int? CurrentStockWarning { get; set; }
}
