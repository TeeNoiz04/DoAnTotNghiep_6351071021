namespace QuoteFlow.SaleOrderDetails;

public static class SaleOrderDetailConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "SaleOrderDetail." : string.Empty);
    }

    public const int StatusCodeMaxLength = 50;
    public const int GolfaCodeMaxLength = 50;
    public const int NoteMaxLength = 4000;
    public const int MaxExtrafee_NoteLength = 4000;
}