using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferNBs.Validators;

public class PriceOfferNBDetailValidationConfig : ExcelValidationConfig
{
    public PriceOfferNBDetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetNB,
                //specificHeader: PriceOfferConsts.ExcelModelNameHeader,
                stopColumn: "B", // material code
                startCell: "A4",
                endCell: "M2000",
                startColumn: PriceOfferConsts.ExcelModelNameColumn,
                endColumn: PriceOfferConsts.ExcelMEVNOfferPrice,
                hasHeader: false
            ));
    }
}
