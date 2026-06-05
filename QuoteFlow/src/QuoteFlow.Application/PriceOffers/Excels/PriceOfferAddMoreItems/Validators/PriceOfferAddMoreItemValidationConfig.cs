using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferAddMoreItems.Validators;
public class PriceOfferAddMoreItemValidationConfig : ExcelValidationConfig
{
    public PriceOfferAddMoreItemValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetAddMoreItem,
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
