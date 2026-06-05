using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialStatus;

public class MaterialStatusConfig : ExcelValidationConfig
{
    public MaterialStatusConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetMaterialStatus,
                startCell: MaterialConsts.ExcelMaterialStatusStartCell,
                endCell: MaterialConsts.ExcelMaterialStatusEndCell,
                startColumn: MaterialConsts.ExcelMaterialStatusStartColumn,
                endColumn: MaterialConsts.ExcelMaterialStatusEndColumn,
                hasHeader: false
            ));
    }
}
