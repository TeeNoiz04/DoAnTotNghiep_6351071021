using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers.Excels.PriceOfferDSs.Validators;

public class PriceOfferDSDetailValidationConfig : ExcelValidationConfig
{
    public PriceOfferDSDetailValidationConfig()
    {
        ApplyConfig(
            FromFixedStartCell(
                sheetName: PriceOfferConsts.ExcelImportSheetDS,
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
