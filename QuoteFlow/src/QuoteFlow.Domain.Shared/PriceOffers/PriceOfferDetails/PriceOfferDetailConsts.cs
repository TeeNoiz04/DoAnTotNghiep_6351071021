using QuoteFlow.Shared.Models;

namespace QuoteFlow.PriceOffers.PriceOfferDetails;

public static class PriceOfferDetailConsts
{
    private const string DefaultSorting = "{0}GolfaCode desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "PriceOfferDetail." : string.Empty);
    }

    public const int GolfaCodeMaxLength = 50;
    public const int ModelNameMaxLength = 255;
    public const int SpecialSpec1MaxLength = 400;
    public const int SpecialSpec2MaxLength = 400;
    public const int CompetitorBrandMaxLength = 2048;
    public const int CompetitorModelMaxLength = 4000;
    public const int InputCurrencyMaxLength = 50;
    public const int AccountCodeMaxLength = 50;
    public const int NoteMaxLength = 4000;


    public const string DefaultStatus = QuoteFlowStatuses.Draft;
}