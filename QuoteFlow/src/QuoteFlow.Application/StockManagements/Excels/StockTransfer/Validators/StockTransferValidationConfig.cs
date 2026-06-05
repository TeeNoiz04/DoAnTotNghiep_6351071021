using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;

namespace QuoteFlow.StockManagements.Excels.StockTransfer.Validators;
public class StockTransferValidationConfig : ExcelValidationConfig
{
    public StockTransferValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialStockUploadDetailConsts.ExcelImportSheet,
                //specificHeader: PriceOfferConsts.ExcelModelNameHeader,
                startCell: MaterialStockUploadDetailConsts.ExcelMaterialCodeStartCell,
                endCell: MaterialStockUploadDetailConsts.ExcelRemarkEndCell,
                startColumn: MaterialStockUploadDetailConsts.ExcelMaterialCodeColumn,
                endColumn: MaterialStockUploadDetailConsts.ExcelRemarkColumn,
                hasHeader: false
            ));
    }
}
