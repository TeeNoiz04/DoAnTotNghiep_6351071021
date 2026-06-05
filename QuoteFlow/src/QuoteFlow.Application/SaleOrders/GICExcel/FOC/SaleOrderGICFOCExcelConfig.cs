using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.GICExcel.FOC;

public class SaleOrderGICFOCExcelConfig : ExcelValidationConfig
{
    public SaleOrderGICFOCExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Data_FOC",
                startCell: "B3",
                endCell: "AG1000",
                startColumn: "B",
                endColumn: "AG",
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}
