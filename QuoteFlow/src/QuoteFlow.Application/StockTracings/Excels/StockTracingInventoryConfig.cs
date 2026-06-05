using QuoteFlow.Shared.Excels;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingInventoryConfig : ExcelValidationConfig
{
    public StockTracingInventoryConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                startCell: StockTracingConsts.InventoryImportStartCell,
                endCell: StockTracingConsts.InventoryImportEndCell,
                startColumn: StockTracingConsts.InventoryImportStartColumn,
                endColumn: StockTracingConsts.InventoryImportEndColumn,
                stopColumn: "B",
                hasHeader: false
            ));
    }
}