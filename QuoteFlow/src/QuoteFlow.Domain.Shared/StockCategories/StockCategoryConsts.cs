namespace QuoteFlow.StockCategories;

public static class StockCategoryConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "StockCategory." : string.Empty);
    }

    public const int StockCodeMaxLength = 50;
    public const int SAPCodeMaxLength = 50;
    public const int StockNameMaxLength = 500;
    public const int NoteMaxLength = 4000;
}