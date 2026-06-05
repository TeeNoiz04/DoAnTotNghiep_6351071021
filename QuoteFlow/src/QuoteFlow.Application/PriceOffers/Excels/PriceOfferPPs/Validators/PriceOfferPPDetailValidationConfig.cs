using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;

public class PriceOfferPPDetailValidationConfig : ExcelValidationConfig
{
    public PriceOfferPPDetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetPP,
                //specificHeader: PriceOfferConsts.ExcelModelNameHeader,
                stopColumn: "B",
                startCell: "A28",
                endCell: "P2000",
                startColumn: PriceOfferConsts.ExcelModelNameColumn,
                endColumn: PriceOfferConsts.ExcelCompetitorPriceColumn,
                hasHeader: false
            ));
    }
}
