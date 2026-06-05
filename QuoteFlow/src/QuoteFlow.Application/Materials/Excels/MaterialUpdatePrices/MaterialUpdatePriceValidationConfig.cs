using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialUpdatePrices;
public class MaterialUpdatePriceValidationConfig : ExcelValidationConfig
{
    public MaterialUpdatePriceValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetUpdatePrice,
                startCell: MaterialConsts.ExcelUpdatePriceStartCell,
                endCell: MaterialConsts.ExcelUpdatePriceEndCell,
                startColumn: MaterialConsts.ExcelMaterialCodeColumn,
                endColumn: MaterialConsts.ExcelSellingPrice5Column,
                hasHeader: false
            ));
    }
}