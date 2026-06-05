using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferPPs.Validators;

public class PriceOfferPPCustomerValidationConfig : ExcelValidationConfig
{
    public PriceOfferPPCustomerValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetPP,
                startCell: PriceOfferConsts.ExcelCustomerStartCell,
                endCell: PriceOfferConsts.ExcelCustomerEndCell,
                startColumn: PriceOfferConsts.ExcelSalesChannelColumn,
                endColumn: PriceOfferConsts.ExcelSecCheckColumn,
                stopColumn: "C", // tax code
                hasHeader: false
            ));
    }
}
