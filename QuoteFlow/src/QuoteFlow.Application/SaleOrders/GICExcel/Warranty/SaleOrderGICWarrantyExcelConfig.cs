using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SaleOrders.GICExcel.Warranty;

public class SaleOrderGICWarrantyExcelConfig : ExcelValidationConfig
{
    public SaleOrderGICWarrantyExcelConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "Data_WR",
                startCell: "B3",
                endCell: "AK1000",
                startColumn: "B",
                endColumn: "AK",
                hasHeader: false
            //stopColumn: StockImportPriorityConsts.ExcelDetailsStopColumn
            ));
    }
}
