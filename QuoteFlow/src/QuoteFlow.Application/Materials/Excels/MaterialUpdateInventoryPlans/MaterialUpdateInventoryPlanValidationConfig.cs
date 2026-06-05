using QuoteFlow.Shared.Excels;

namespace QuoteFlow.Materials.Excels.MaterialUpdateInventoryPlans;
public class MaterialUpdateInventoryPlanValidationConfig : ExcelValidationConfig
{
    public MaterialUpdateInventoryPlanValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: MaterialConsts.ExcelImportSheetUpdateInventoryPlan,
                startCell: MaterialConsts.ExcelUpdateInventoryPlanStatCell,
                endCell: MaterialConsts.ExcelUpdateInventoryPlanEndCell,
                startColumn: MaterialConsts.ExcelGolfaCodeColumn,
                endColumn: MaterialConsts.ExcelStockWarningColumn,
                hasHeader: false
            ));
    }
}