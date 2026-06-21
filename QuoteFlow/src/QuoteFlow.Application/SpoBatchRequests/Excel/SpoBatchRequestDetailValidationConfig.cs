using QuoteFlow.Shared.Excels;

namespace QuoteFlow.SpoBatchRequests.Excel;

public class SpoBatchRequestDetailValidationConfig : ExcelValidationConfig
{
    public SpoBatchRequestDetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: "BatchRequest",
                //specificHeader: PriceOfferConsts.ExcelModelNameHeader,
                startCell: "A2",
                endCell: "E10000",
                startColumn: "A",
                endColumn: "E",
                hasHeader: false
            ));
    }
}