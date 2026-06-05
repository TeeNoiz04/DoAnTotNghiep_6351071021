using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialFactory;

public class MaterialFactoryConfig : ExcelValidationConfig
{
    public MaterialFactoryConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetMaterialFactory,
                startCell: MaterialConsts.ExcelMaterialFactoryStartCell,
                endCell: MaterialConsts.ExcelMaterialFactoryEndCell,
                startColumn: MaterialConsts.ExcelMaterialFactoryStartColumn,
                endColumn: MaterialConsts.ExcelMaterialFactoryEndColumn,
                hasHeader: false
            ));
    }
}
