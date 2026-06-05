namespace QuoteFlow.MaterialGroupBuyers;

public static class MaterialGroupBuyerConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialGroupBuyer." : string.Empty);
    }

    public const int MaterialGroupCodeMaxLength = 50;
    public const int BuyerShortNameMaxLength = 255;
}