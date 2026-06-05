using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferAPs.Validators;

public class PriceOfferAPDetailValidationConfig : ExcelValidationConfig
{
    public PriceOfferAPDetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetAP,
                //specificHeader: PriceOfferConsts.ExcelModelNameHeader,
                stopColumn: "B", // material code
                startCell: "A4",
                endCell: "P2000",
                startColumn: PriceOfferConsts.ExcelModelNameColumn,
                endColumn: PriceOfferConsts.ExcelCompetitorPriceColumn,
                hasHeader: false
            ));
    }
}
