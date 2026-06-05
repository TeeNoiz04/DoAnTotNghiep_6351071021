using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialUpdateWithoutPrices;
public class MaterialUpdateWithoutPriceValidationConfig : ExcelValidationConfig
{
    public MaterialUpdateWithoutPriceValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetWithoutUpdatePrice,
                startCell: MaterialConsts.ExcelUpdateWithoutPriceStatCell,
                endCell: MaterialConsts.ExcelUpdateWithoutPriceEndCell,
                startColumn: MaterialConsts.ExcelUpdateWithoutStartColumn,
                endColumn: MaterialConsts.ExcelUpdateWithoutEndColumn,
                hasHeader: false
            ));
    }
}