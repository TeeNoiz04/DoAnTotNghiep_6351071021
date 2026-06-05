using QuoteFlow.Shared.Excels;

namespace QuoteFlow.DPOs.Excels.ImportDPO.Validators;

public class ImportDPODetailValidationConfig : ExcelValidationConfig
{
    public ImportDPODetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: DPOConsts.ExcelImportSheetName,
                startCell: DPOConsts.ExcelDetailsStartCell,
                endCell: DPOConsts.ExcelDetailsEndCell,
                startColumn: "A",
                endColumn: "N",
                hasHeader: false,
                stopColumn: DPOConsts.ExcelDetailsStopColumn
            ));
    }
}