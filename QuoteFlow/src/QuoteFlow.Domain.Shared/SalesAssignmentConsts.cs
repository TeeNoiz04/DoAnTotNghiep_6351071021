namespace QuoteFlow;

public static class SalesAssignmentConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "Buyer Sale PIC." : string.Empty);
    }

    public const int CodeMaxLength = 50;
    public const int SaleUserNameMaxLength = 500;
    public const int MaterialTypeMaxLength = 50;
    public const int BuyerShortNameMaxLength = 255;
    public const int SaleFullNameMaxLength = 500;
}
