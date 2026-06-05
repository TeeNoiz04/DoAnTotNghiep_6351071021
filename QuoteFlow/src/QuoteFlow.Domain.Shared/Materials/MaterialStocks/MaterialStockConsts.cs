namespace QuoteFlow.Materials.MaterialStocks;

public static class MaterialStockConsts
{
    private const string DefaultSorting = "{0}CreationTime desc";

    public static string GetDefaultSorting(bool withEntityName)
    {
        return string.Format(DefaultSorting, withEntityName ? "MaterialStock." : string.Empty);
    }

    public const int GolfaCodeMaxLength = 50;
    public const int ModelMaxLength = 100;
    public const int NoteMaxLength = 4000;

}