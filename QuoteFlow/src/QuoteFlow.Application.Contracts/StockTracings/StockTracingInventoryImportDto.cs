using System;

namespace QuoteFlow.StockTracings;

public class StockTracingInventoryImportDto
{
    public int? RowNo { get; set; }
    public string? WareHouse { get; set; }
    public string? BUs { get; set; }
    public string? Customer { get; set; }
    public string? Category { get; set; }
    public string? SKUCode { get; set; }
    public string? SKUName { get; set; }
    public string? Quality { get; set; }
    public string? Warranty { get; set; }
    public string? Unit { get; set; }
    public string? Series { get; set; }
    public string? OrginCode { get; set; }
    public DateTime? ProductionDate { get; set; }
    public string? GolfaCode { get; set; }
    public string? Note { get; set; }
}
