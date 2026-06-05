using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse_Change;

public class SaleOrderGICInternalUseChangeExcelConfig : ExcelValidationConfig
{
    public SaleOrderGICInternalUseChangeExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Data_IU_Change",
                startCell: "B3",
                endCell: "AA1000",
                startColumn: "B",
                endColumn: "AA",
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}
