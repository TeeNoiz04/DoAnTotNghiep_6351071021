namespace QuoteFlow.Materials.MaterialStocks.MaterialStockLockStocks;

public static class MaterialStockLockStockConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialStockLockStock." : string.Empty);
    }

    public const int GolfaCodeMaxLength = 50;
    public const int NoteMaxLength = 4000;
}