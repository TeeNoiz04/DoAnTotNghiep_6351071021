using QuoteFlow.Shared.Excels;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingDeliveryConfig : ExcelValidationConfig
{
    public StockTracingDeliveryConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                startCell: StockTracingConsts.DeliveryImportStartCell,
                endCell: StockTracingConsts.DeliveryImportEndCell,
                startColumn: StockTracingConsts.DeliveryImportStartColumn,
                endColumn: StockTracingConsts.DeliveryImportEndColumn,
                stopColumn: "B",
                hasHeader: false
            ));
    }
}