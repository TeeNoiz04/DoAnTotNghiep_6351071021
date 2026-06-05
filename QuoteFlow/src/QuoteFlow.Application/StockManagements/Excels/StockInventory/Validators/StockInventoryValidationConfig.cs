using QuoteFlow.MaterialStockUploadDetails;
using QuoteFlow.Shared.Excels;

namespace QuoteFlow.StockManagements.Excels.StockInventory.Validators;
public class StockInventoryValidationConfig : ExcelValidationConfig
{
    public StockInventoryValidationConfig()
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
