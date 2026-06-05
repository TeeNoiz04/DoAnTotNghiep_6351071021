using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferUpdateLandingCosts.Validators;

public class PriceOfferUpdateLandingCostValidationConfig : ExcelValidationConfig
{
    public PriceOfferUpdateLandingCostValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetUpdateLandingCost,
                startCell: PriceOfferConsts.ExcelUpdateLandingCostStartCell,
                endCell: PriceOfferConsts.ExcelUpdateLandingCostEndCell,
                startColumn: PriceOfferConsts.ExcelGolfaCodeColumn,
                endColumn: PriceOfferConsts.ExcelNewSaleOfferPriceColumn,
                stopColumn: "A", // golfa code
                hasHeader: false
            ));
    }
}
