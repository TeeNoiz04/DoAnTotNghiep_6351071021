using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.Excel;
public class SaleOrderExcelConfig : ExcelValidationConfig
{
    public SaleOrderExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Import_SAP_SO",
                startCell: SaleOrderConsts.ImportStartCell,
                endCell: SaleOrderConsts.ImportEndCell,
                startColumn: SaleOrderConsts.ImportStartColumn,
                endColumn: SaleOrderConsts.ImportEndColumn,
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}
