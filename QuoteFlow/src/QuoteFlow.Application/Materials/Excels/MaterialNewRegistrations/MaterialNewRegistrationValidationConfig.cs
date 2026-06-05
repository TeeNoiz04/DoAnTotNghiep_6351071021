using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialNewRegistrations;
public class MaterialNewRegistrationValidationConfig : ExcelValidationConfig
{
    public MaterialNewRegistrationValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetNewRegistration,
                startCell: MaterialConsts.ExcelNewRegistrationStartCell,
                endCell: MaterialConsts.ExcelNewRegistrationEndCell,
                startColumn: MaterialConsts.ExcelRegistrationDateColumn,
                endColumn: MaterialConsts.ExcelSellingPrice5_NewRegistrationColumn,
                hasHeader: false
            ));
    }
}