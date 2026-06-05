namespace QuoteFlow.DistributorTargets;

public static class DistributorTargetConsts
{
    private const string DefaultSorting = "{0}BuyerCode asc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "DistributorTarget." : string.Empty);
    }

    public const int BuyerCodeMaxLength = 100;
    public const int BuyerNameMaxLength = 400;
    public const int MaterialTypeMaxLength = 50;
}