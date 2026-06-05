using QuoteFlow.PriceOffers.PriceOfferDetails;
using QuoteFlow.Shared.Excels;

namespace QuoteFlow.PriceOffers;
public class ImportAddMoreItemsInput
{
    public ExcelValidationResult<PriceOfferDetailImportDto> ValidationResult { get; set; }
    public string? Comment { get; set; }
}
