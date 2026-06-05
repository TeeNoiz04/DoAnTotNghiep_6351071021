using QuoteFlow.Shared.Excels;

namespace QuoteFlow.StockTracings.Excels;

public class StockTracingReceiptConfig : ExcelValidationConfig
{
    public StockTracingReceiptConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                startCell: StockTracingConsts.ReceiptImportStartCell,
                endCell: StockTracingConsts.ReceiptImportEndCell,
                startColumn: StockTracingConsts.ReceiptImportStartColumn,
                endColumn: StockTracingConsts.ReceiptImportEndColumn,
                stopColumn: "B",
                hasHeader: false
            ));
    }
}