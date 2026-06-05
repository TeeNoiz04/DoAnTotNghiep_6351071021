using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.GICExcel.WriteOff;

public class SaleOrderGICWriteOffExcelConfig : ExcelValidationConfig
{
    public SaleOrderGICWriteOffExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Data_WO",
                startCell: "B3",
                endCell: "U1000",
                startColumn: "B",
                endColumn: "U",
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}