using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.GICExcel.InternalUse;

public class SaleOrderGICInternalUseExcelConfig : ExcelValidationConfig
{
    public SaleOrderGICInternalUseExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Data_IU",
                startCell: "B3",
                endCell: "AD1000",
                startColumn: "B",
                endColumn: "AD",
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}
