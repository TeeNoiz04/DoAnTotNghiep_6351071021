using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialSAP;

public class MaterialSAPConfig : ExcelValidationConfig
{
    public MaterialSAPConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetMaterialSAP,
                startCell: MaterialConsts.ExcelMaterialSAPStartCell,
                endCell: MaterialConsts.ExcelMaterialSAPEndCell,
                startColumn: MaterialConsts.ExcelMaterialSAPStartColumn,
                endColumn: MaterialConsts.ExcelMaterialSAPEndColumn,
                hasHeader: false
            ));
    }
}
